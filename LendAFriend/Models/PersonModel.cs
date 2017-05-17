using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LendAFriend.Models
{
    public class PersonModel
    {
        public bool Borrower { get; set; }
        public string ApprovedSum { get; set; }
        public string ApprovedInterest { get; set; }
        public ContactModel Contact { get; set; }
        public bool Approved { get; set; }
    }
}