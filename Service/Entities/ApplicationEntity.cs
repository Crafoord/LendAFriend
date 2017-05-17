using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendAFriend.Service.Entities
{
    public class ApplicationEntity
    {
        public Guid ApplicationId { get; set; }
        public string AmountTotal { get; set; }
        public string InterestTotal { get; set; }
        public List<PersonEntity> Persons { get; set; }
    }
}
