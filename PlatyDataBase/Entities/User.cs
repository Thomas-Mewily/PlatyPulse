using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatyDataBase.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public int XP { get; set; }
        public DateTime BirthDay { get; set; }
        public DateTime DateJoined { get; set; }
    }

}
