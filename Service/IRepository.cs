using LendAFriend.Service.Entities;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendAFriend.Service
{
    public interface IRepository
    {
        Task SaveApplicationAsync(ApplicationEntity application);
        Task<ApplicationEntity> GetApplicationAsync(Guid applicationId);
        Task<ApplicationEntity> GetApplicationSpecificPersonAsync(Guid applicationId, Guid personId);
        Task UpdatePersonAsync(PersonEntity person);
        Task SendEmailToPersonAsync(PersonEntity person);
    }
}
