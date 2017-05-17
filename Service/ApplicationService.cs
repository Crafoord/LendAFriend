using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LendAFriend.Service.Entities;

namespace LendAFriend.Service
{
    public class ApplicationService : IApplicationService
    {
        private readonly IRepository _repository;

        public ApplicationService(IRepository repository)
        {
            _repository = repository;
        }

        public Task SaveApplicatonAsync(ApplicationEntity application)
        {
            var result = _repository.SaveApplicationAsync(application);

            foreach (var person in application.Persons)
            {
                if (!person.Borrower)
                {
                      _repository.SendEmailToPersonAsync(person);
                }
            }

            return result;
        }

        public Task<ApplicationEntity> GetApplicationAsync(Guid applicationId)
        {
            return _repository.GetApplicationAsync(applicationId);
        }

        public Task<ApplicationEntity> GetApplicationSpecificPersonAsync(Guid applicationId, Guid personId)
        {
            return _repository.GetApplicationSpecificPersonAsync(applicationId, personId);
        }

        public Task UpdateApplicationStatusSpecificPersonAsync(PersonEntity person)
        {
            return _repository.UpdatePersonAsync(person);
        }
    }
}
