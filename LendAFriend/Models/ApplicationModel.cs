using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LendAFriend.Models
{
    public class ApplicationModel
    {
        public string AmountTotal { get; set; }
        public string InterestTotal { get; set; }
        public List<PersonModel> Persons { get; set; }
    }
}