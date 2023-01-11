using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MTCG.MODELS
{
    public class CurlResponse
    {
        public int Status { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public JsonObject? JsonList { get; set; }
        public bool Json { get; set; }

        public CurlResponse() { 
            Status = 0; 
            Success = false;
            Message = "Unknown Error";
            JsonList = null;
            Json = false;
        }

        public string parseDictionaryToString()
        {
            if (Json) 
                return JsonList.ToString();
            else 
                return "";
        }

        public void setResponse(int Status, bool Success, string Message)
        {
            this.Status = Status;
            this.Success = Success;
            this.Message = Message;
        }
    }
}
