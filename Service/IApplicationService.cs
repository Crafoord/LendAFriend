using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LendAFriend.Service.Entities;

namespace LendAFriend.Service
{
    public interface IApplicationService
    {
        Task SaveApplicatonAsync(ApplicationEntity application);
        Task<ApplicationEntity> GetApplicationAsync(Guid applicationId);

        Task<ApplicationEntity> GetApplicationSpecificPersonAsync(Guid applicationId, Guid personId);

        Task UpdateApplicationStatusSpecificPersonAsync(PersonEntity person);
    }
}
