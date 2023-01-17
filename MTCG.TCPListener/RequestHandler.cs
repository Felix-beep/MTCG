using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.MODELS;
using MTCG.BL;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using System.Diagnostics;
using System.Security.Principal;
using MTCG.BL.CombatHandling;

namespace MTCG.TCPServer
{
    internal class RequestHandler
    {
        private CurlRequest Request;
        public CurlResponse Response { get; private set; }
        private string? User { get; set; }
        private string? Role { get; set; }

        public RequestHandler(CurlRequest request)
        {
            Request = request;
            Response = new();
        }

        public void HandleRequest()
        {
            if(Request == null)
            {
                return;
            }

            switch (Request.MethodType)
            {
                case "POST": handlePOST(); break;
                case "GET": handleGET(); break;
                case "PUT": handlePUT(); break;
                case "DELETE": handleDELETE(); break;
                default: Console.WriteLine("Uknown Method Name"); break;
            }
            return;
        }

        private void handlePOST()
        {
            string Password;
            string Username;
            switch (Request.Method)
            {
                // Register new User
                case "users":
                    {
                        try
                        {
                            Username = Request.DynamicJsonObject.Username.ToString();
                            Password = Request.DynamicJsonObject.Password.ToString();
                            
                        } catch
                        {
                            Response.Status = 409;
                            Response.Success = false;
                            return;
                        }
                        Response = UserHandler.CreateUser(Username, Password);
                        return;
                    }
                // Login with existing User
                case "sessions":
                    { 
                        try
                        {
                            Username = Request.DynamicJsonObject.Username.ToString();
                            Password = Request.DynamicJsonObject.Password.ToString();
                        }
                        catch
                        {
                            Response.Status = 409;
                            Response.Success = false;
                            return;
                        }
                        Response = UserHandler.LoginUser(Username, Password);
                        return; 
                    }
                // Create new Card Package
                case "packages":
                    {
                        if (!Authorize()) return;
                        if (Role != "Admin")
                        {
                            Response.Status = 411;
                            Response.Message = "Not enough Power.";
                        }
                        List<Tuple<string,string>> Cards = new();
                        try
                        {
                            foreach (var obj in Request.DynamicJsonObject)
                            {
                                string ID = obj.Id.ToString();
                                string Name = obj.Name.ToString();
                                Cards.Add(new Tuple<string, string>(ID, Name));
                            }
                        } catch
                        {
                            Response.Status = 409;
                            Response.Success = false;
                            return;
                        }

                        Response = PackHandler.CreatePackage(User, Cards);
                        return;
                    }
                // Aquire a package
                case "transactions":
                    {
                        if (!Authorize()) return;
                        Response = PackHandler.BuyPackage(User);
                        return;
                    }

                // Enter a lobby to start a battle
                case "battles":
                    {
                        if (!Authorize()) return;
                        Response = QueueHandler.EnterQueue(User);
                        return;
                    }

                // Create a new trading deal
                case "tradings":
                    {
                        if (!Authorize()) return;
                        if (Request.MethodInfo == null)
                        {
                            string Id;
                            string CardId;
                            string RatingS;
                            int Rating;
                            try
                            {
                                Id = Request.DynamicJsonObject.Id.ToString();
                                CardId = Request.DynamicJsonObject.CardToTrade.ToString();
                                RatingS = Request.DynamicJsonObject.Rating.ToString();
                                Rating = int.Parse(RatingS);
                            }
                            catch
                            {
                                Response.Status = 409;
                                Response.Success = false;
                                return;
                            }
                            Response = TradeHandler.CreateTrade(User, Id, CardId, Rating);
                        } else
                        {
                            string TradeId;
                            string CardId;
                            try
                            {
                                TradeId = Request.MethodInfo;
                                CardId = Request.DynamicJsonObject.ToString();
                            }
                            catch
                            {
                                Response.Status = 409;
                                Response.Success = false;
                                return;
                            }
                            Response = TradeHandler.BuyTrade(User, TradeId, CardId);
                        }
                        return;
                    }

                default: Console.WriteLine("Curl Parser not implemented yet"); return;
            }
        }

        private void handleGET()
        {
            switch (Request.Method)
            {
                // Gets infos of the User
                case "users":
                    {
                        string Username;
                        try
                        {
                            Username = Request.MethodInfo;
                        } catch
                        {
                            Response.Status = 409;
                            Response.Success = false;
                            return;
                        }
                        Response = UserHandler.GetUser(Username);
                        return;
                    }

                // List of Users cards
                case "cards":
                    {
                        if (!Authorize()) return;
                        Response = StackHandler.GetStack(User);
                        return;
                    }
                // Shows the Decks
                case "deck":
                    {
                        if (!Authorize()) return;
                        Response = DeckHandler.GetDeck(User);
                        return;
                    }

                // Gets the Users stats
                case "stats":
                    {
                        if (!Authorize()) return;
                        Response = StatsHandler.GetStats(User);
                        return;
                    }
                // Gets the global scoreboard
                case "score":
                    {
                        if (!Authorize()) return;
                        Response = StatsHandler.GetLeaderboard();
                        return;
                    }

                // gets the global trading deals
                case "tradings":
                    {
                        if(!Authorize()) return;
                        Response = TradeHandler.GetTradeDeals();
                        return;
                    }

                default: Console.WriteLine("Curl Parser not implemented yet"); return;
            }
        }
        private void handlePUT()
        {
            switch (Request.Method)
            {
                // updates the Users info
                case "users":
                    {
                        if(!Authorize()) return;
                        string UserToChange;
                        string Username;
                        string Bio;
                        string Image;
                        try
                        {
                            UserToChange = Request.MethodInfo;
                            Username = Request.DynamicJsonObject.Name.ToString();
                            Bio = Request.DynamicJsonObject.Bio.ToString();
                            Image = Request.DynamicJsonObject.Image.ToString();
                        } catch
                        {
                            Response.Status = 409;
                            Response.Success = false;
                            return;
                        }


                        if (Role != "Admin" && User != UserToChange)
                        {
                            Response.Status = 411;
                            Response.Message = "Not enough Power.";
                            return;
                        }

                        Response = UserHandler.UpdateUser(UserToChange, Username, Bio, Image);
                        return;
                    }

                // puts 4 cards into a deck
                case "deck":
                    {
                        if (!Authorize()) return;
                        List<string> CardIds = new();
                        try
                        {
                            foreach (var Card in Request.DynamicJsonObject)
                            {
                                CardIds.Add(Card.ToString());
                            }


                            Response = DeckHandler.CreateDeck(User, CardIds);

                        } catch {
                            Response.Status = 409;
                            Response.Success = false;
                            return;
                        }

                        return;
                    }
                default: Console.WriteLine("Curl Parser not implemented yet"); return;
            }
        }
        private void handleDELETE()
        {
            switch (Request.Method)
            {
                // "Accepts" a trading deal
                case "tradings":
                    {
                        string UUID;
                        if (!Authorize()) return;
                        try
                        {
                            UUID = Request.MethodInfo;
                        } catch
                        {
                            Response.Status = 409;
                            Response.Success = false;
                            return;
                        }
                        Response = TradeHandler.DeleteTrade(User, UUID);
                        return;
                    }
                default: Console.WriteLine("Curl Parser not implemented yet"); return;
            }
        }

        private bool Authorize()
        {
            if (Request.Token == null) return false;
            Console.WriteLine($"Trying to authorize {Request.Token}");
            bool validToken = false;
            string Username = TokenHandler.AuthenticateUser(Request.Token);
            if (Username != null)
            {
                User = Username;
                if(User == "admin")
                {
                    Role = "Admin";
                }
                Console.WriteLine($"Token found, {User}");
                validToken = true;
            } else
            {
                Response.Status = 409;
                Response.Message = "Invalid Authentication";
            }
            return validToken;
        }
    }
}
