using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.Model
{
    public class ImportRequest
    { 
        public int cardNo { get; set; }
        public string cardValue { get; set; }
        public DateTime expiryDate { get; set; }
        public string mobile { get; set; }
        public string remarks { get; set; }
    }
}
