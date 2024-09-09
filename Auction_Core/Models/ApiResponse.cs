using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Auction_Core.Models
{
    public class ApiResponse
    {
        public ApiResponse() 
        {
            ErrorMessages =new List<string>();
        }
        public HttpStatusCode StatusCode { get; set; }

        public bool isSucces { get; set; }

        public List<string> ErrorMessages { get; set; }

        public object Result { get; set; }
    }
}
