using AwesomeChat.Domain.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeChat.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        public string EmailAdress { get; set; }
        public string PlaceOfBirth { get; set; }
        public DateTime BirthOfDate { get; set; }
        public bool isActive { get; set; }
        public GenderEnum Gender { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<Message> Messages { get; set; }

    }
}
