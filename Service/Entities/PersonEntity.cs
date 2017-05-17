using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendAFriend.Service.Entities
{
    public class PersonEntity
    {
        public Guid ApplicationId { get; set; }
        public Guid Id { get; set; }
        public bool Borrower { get; set; }
        public string ApprovedSum { get; set; }
        public string ApprovedInterest { get; set; }
        public ContactEntity Contact { get; set; }
        public bool Approved { get; set; }
    }
}
