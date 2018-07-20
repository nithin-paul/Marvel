using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarvelAdmin.Models
{
    public class UserVM
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public List<string> Role { get; set; }
    }
}