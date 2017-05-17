using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LendAFriend.Service.Entities;

namespace LendAFriend.Models
{
    public class ApplicationMapper
    {
        public static ApplicationEntity Map(Guid id, ApplicationModel model)
        {
            return new ApplicationEntity()
            {
                ApplicationId = id,
                AmountTotal = model.AmountTotal,
                InterestTotal = model.InterestTotal,

                Persons = model.Persons.Select(p => new PersonEntity()
                {
                    Id = Guid.NewGuid(),
                    ApplicationId = id,
                    Borrower = p.Borrower,
                    ApprovedInterest = p.ApprovedInterest,
                    ApprovedSum = p.ApprovedSum,
                    Approved = p.Approved,
                    Contact = new ContactEntity
                    {
                        Email = p.Contact.Email,
                        Phone = p.Contact.Phone,
                        PersonId = p.Contact.PersonId,
                        FirstName = p.Contact.FirstName,
                        LastName = p.Contact.LastName
                    }
                }).ToList()
            };
        }
        public static ApplicationModel Map(ApplicationEntity entity)
        {
            return new ApplicationModel
            {
                AmountTotal = entity.AmountTotal,
                InterestTotal = entity.InterestTotal,

                Persons = entity.Persons.Select(p => new PersonModel
                {
                    Borrower = p.Borrower,
                    ApprovedInterest = p.ApprovedInterest,
                    ApprovedSum = p.ApprovedSum,
                    Approved = p.Approved,
                    Contact = new ContactModel
                    {
                        Email = p.Contact.Email,
                        Phone = p.Contact.Phone,
                        PersonId = p.Contact.PersonId,
                        FirstName = p.Contact.FirstName,
                        LastName = p.Contact.LastName
                    }
                }).ToList()
            };
        }

        public static PersonEntity Map(Guid applicationId, Guid id, PersonModel model)
        {
            return new PersonEntity
            {
                ApplicationId = applicationId,
                Id = id,
                Borrower = model.Borrower,
                ApprovedInterest = model.ApprovedInterest,
                ApprovedSum = model.ApprovedSum,
                Approved = model.Approved,
                Contact = new ContactEntity
                {
                    Email = model.Contact.Email,
                    Phone = model.Contact.Phone,
                    PersonId = model.Contact.PersonId,
                    FirstName = model.Contact.FirstName,
                    LastName = model.Contact.LastName
                }
            };
        }
    }
}