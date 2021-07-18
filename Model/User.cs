using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.Model
{
    public class User
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string Token { get; set; }

    }

    public class AuthecateRequest
    {
        public string userName { get; set; }
        public string password { get; set; }
    }
}
