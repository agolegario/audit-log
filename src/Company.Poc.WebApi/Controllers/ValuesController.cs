using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Company.Poc.WebApi.FakeRepository;
using Company.Poc.WebApi.Models;

namespace Company.Poc.WebApi.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly IProductRepository _repository;

        public ValuesController()
        {
            _repository = new ProductRepository();
        }

        [HttpGet]
        [Route("api/values")]
        public HttpResponseMessage Get()
        {
            try
            {
                var produtos = _repository.GetAll();
                return Request.CreateResponse(HttpStatusCode.OK, produtos);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Exception", ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ModelState);
            }
        }

        [HttpGet]
        [Route("api/values/{id}")]
        public async Task<HttpResponseMessage> Get(int id)
        {
            try
            {
                var product = _repository.GetById(id);
                return await Task<HttpResponseMessage>
                    .Factory
                    .StartNew(() =>
                        product == null
                            ? Request.CreateResponse(HttpStatusCode.BadRequest)
                            : Request.CreateResponse(HttpStatusCode.OK, product));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Exception", ex.Message);
                return await Task<HttpResponseMessage>.Factory.StartNew(() =>
                    Request.CreateResponse(HttpStatusCode.InternalServerError, ModelState));
            }
        }

        [HttpPost]
        [Route("api/values")]
        public HttpResponseMessage Post([FromBody] Product product)
        {
            try
            {
                return !ModelState.IsValid
                    ? Request.CreateResponse(HttpStatusCode.BadRequest, ModelState)
                    : Request.CreateResponse(HttpStatusCode.Created, _repository.Add(product));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Exception", ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ModelState);
            }
        }

        [HttpPut]
        [Route("api/values")]
        public HttpResponseMessage Put([FromBody] Product product)
        {
            try
            {
                return !ModelState.IsValid
                    ? Request.CreateResponse(HttpStatusCode.BadRequest, ModelState)
                    : Request.CreateResponse(HttpStatusCode.Accepted, _repository.Update(product));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Exception", ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ModelState);
            }
        }

        [HttpDelete]
        [Route("api/values/{id}")]
        public async Task<HttpResponseMessage> Delete(int id)
        {
            try
            {
                var product = _repository.GetById(id); 
                if(product == null) return await Task<HttpResponseMessage>.Factory.StartNew(() => Request.CreateResponse(HttpStatusCode.BadRequest));

                var isRemove = _repository.Remove(product);

                return await Task<HttpResponseMessage>
                    .Factory
                    .StartNew(() =>
                            isRemove
                            ? Request.CreateResponse(HttpStatusCode.OK)
                            : Request.CreateResponse(HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Exception", ex.Message);
                return await Task<HttpResponseMessage>.Factory.StartNew(() =>
                    Request.CreateResponse(HttpStatusCode.InternalServerError, ModelState));
            }
        }
    }
}