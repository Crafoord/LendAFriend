using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using LendAFriend.Service.Entities;
using Dapper;
using System.Data;
using System.Linq;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace LendAFriend.Service
{
    public class Repository : IRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["azureDB"].ConnectionString;

        public async Task SaveApplicationAsync(ApplicationEntity application)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(
                    "INSERT INTO Application_TB (ApplicationId, AmountTotal, InterestTotal ) VALUES (@ApplicationId, @AmountTotal, @InterestTotal)",
                    new { application.ApplicationId, application.AmountTotal, application.InterestTotal }
                );

                foreach (var person in application.Persons)
                {
                    await InsertPersonAsync(connection, person);
                }
            }
        }

        private async Task InsertPersonAsync(IDbConnection connection, PersonEntity person)
        {
            await connection.ExecuteAsync(
                @"INSERT INTO Loan_TB (Id, ApplicationId, Borrower)
                    VALUES (@Id, @ApplicationId, @Borrower) ",
                new { person.Id, person.ApplicationId, person.Borrower }
            );
            await connection.ExecuteAsync(
                @"INSERT INTO Contact_TB (Id, PersonId, Email, Phone)
                    VALUES (@Id, @PersonId, @Email, @Phone) ",
                new { person.Id, person.Contact.PersonId, person.Contact.Email, person.Contact.Phone }
            );
        }

        public async Task<ApplicationEntity> GetApplicationAsync(Guid applicationId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var application = (await connection.QueryAsync<ApplicationEntity>(
                    "SELECT * FROM Application_TB WHERE ApplicationId = @ApplicationId",
                    new { applicationId }
                )).SingleOrDefault();

                if (application == null)
                {
                    return null;
                }

                var persons = (await connection.QueryAsync<PersonEntity>(
                    "SELECT * FROM Loan_TB Where ApplicationId = @ApplicationId",
                    new { applicationId }
                )).ToList();

                foreach(var person in persons)
                {
                    person.Contact = (await connection.QueryAsync<ContactEntity>(
                        "SELECT * FROM Contact_TB WHERE Id = @Id",
                        new { person.Id }
                        )).SingleOrDefault();
                }

                application.Persons = persons;

                return application;
            }
        }

        public async Task<ApplicationEntity> GetApplicationSpecificPersonAsync(Guid applicationId, Guid personId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var application = (await connection.QueryAsync<ApplicationEntity>(
                    "SELECT * FROM Application_TB WHERE ApplicationId = @ApplicationId",
                    new { applicationId }
                )).SingleOrDefault();

                if (application == null)
                {
                    return null;
                }

                var persons = (await connection.QueryAsync<PersonEntity>(
                    "SELECT * FROM Loan_TB Where ApplicationId = @ApplicationId AND Id = @PersonId",
                    new { applicationId, personId }
                )).ToList();

                foreach (var person in persons)
                {
                    person.Contact = (await connection.QueryAsync<ContactEntity>(
                        "SELECT * FROM Contact_TB WHERE Id = @Id",
                        new { person.Id }
                        )).SingleOrDefault();
                }

                application.Persons = persons;

                return application;
            }
        }

        public async Task UpdatePersonAsync(PersonEntity person)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(
                    "UPDATE Loan_TB SET ApprovedSum = @ApprovedSum, ApprovedInterest = @ApprovedInterest, Approved = @Approved WHERE ApplicationId = @ApplicationId AND Id = @Id",
                    new { person.ApprovedSum, person.ApprovedInterest, person.Approved, person.ApplicationId, person.Id}
                );
            }
        }

        public async Task SendEmailToPersonAsync(PersonEntity person)
        {
            var apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
            var client = new SendGridClient(apiKey);

            string name = person.Contact.FirstName + " " + person.Contact.LastName;
            string guid = person.Id.ToString();
            string url = "http://lendafriend.azurewebsites.net/?person=" + guid;

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("magnus.crafoord@seb.se", "LendToAFriend"),
                Subject = "Help a friend and lend out your money!",
                PlainTextContent = "Hello, Email!",
                HtmlContent = "<strong>Hello, Email!</strong> " + url
            };
            msg.AddTo(new EmailAddress(person.Contact.Email, name));
            var response = await client.SendEmailAsync(msg);
        }
    }
}
