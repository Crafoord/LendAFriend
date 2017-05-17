using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using LendAFriend.Models;
using LendAFriend.Service;
using System.Web.Http.Cors;

namespace LendAFriend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ApplicationController : ApiController
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController() 
            : this(new ApplicationService(new Repository()))
        {
        }
        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [Route("api/create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public async Task<HttpResponseMessage> Post(ApplicationModel application)
        {
            var id = Guid.NewGuid();
            var entity = ApplicationMapper.Map(id, application);

            await _applicationService.SaveApplicatonAsync(entity);

            return Request.CreateResponse(HttpStatusCode.Created, id);
        }

        [Route("api/{id}/get")]
        [SwaggerResponse(HttpStatusCode.Found)]
        public async Task<HttpResponseMessage> GetApplication(Guid id)
        {
            var entity = await _applicationService.GetApplicationAsync(id);
            
            var result = ApplicationMapper.Map(entity);

            return Request.CreateResponse(HttpStatusCode.Found, result);
        }

        [Route("api/{id}/get/{personId}")]
        [SwaggerResponse(HttpStatusCode.Found)]
        public async Task<HttpResponseMessage> GetApplicationSpecificPerson(Guid id, Guid personId)
        {
            var entity = await _applicationService.GetApplicationSpecificPersonAsync(id, personId);

            var result = ApplicationMapper.Map(entity);

            return Request.CreateResponse(HttpStatusCode.Found, result);
        }

        [Route("api/{applicationId}/update/{id}")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<HttpResponseMessage> PutApplicationSpecificPerson(Guid applicationId, Guid id, [FromBody] PersonModel person)
        {
            var entity = ApplicationMapper.Map(applicationId, id, person);

            await _applicationService.UpdateApplicationStatusSpecificPersonAsync(entity);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
