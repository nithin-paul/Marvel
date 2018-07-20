using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvel.Model
{
    public class UserDTO
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public List<string> Role { get; set; }
    }
}
