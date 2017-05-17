using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LendAFriend.Models
{
    public class ContactModel
    {
        public string PersonId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}