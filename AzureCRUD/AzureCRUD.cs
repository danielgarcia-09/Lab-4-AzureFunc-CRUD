using System;
using System.Net;
using System.Threading.Tasks;
using AzureCRUD.Model.Database;
using AzureCRUD.Model.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureCRUD
{
    public  class AzureCRUD
    {
        public SqlHelper _helper; 

        public AzureCRUD( )
        {
            _helper = new SqlHelper(Environment.GetEnvironmentVariable("ConnectionString"));
        }

        [Function("GetTodos")]
        public async Task<HttpResponseData> Get([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("GetTodos");
            logger.LogInformation("Get all Todos");

            var todos = await _helper.Get();

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(todos);

            return (todos is not null) ? response : req.CreateResponse(HttpStatusCode.NotFound);

        }


        [Function("GetTodoById")]
        public async Task<HttpResponseData> GetById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "todo/{id}")] HttpRequestData req,
            FunctionContext executionContext, int id)
        {
            var logger = executionContext.GetLogger("GetTodos");
            logger.LogInformation("Getting Todo by Id");

            var todo = await _helper.GetById(id);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(todo);

            return (todo is not null) ? response : req.CreateResponse(HttpStatusCode.NotFound);

        }

        [Function("CreateTodo")]
        public async Task<HttpResponseData> Create([HttpTrigger(AuthorizationLevel.Function, "post" )] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("GetTodos");
            logger.LogInformation("Creating Todo");

            var created = await _helper.Create(req.Body);

            var response = created ? req.CreateResponse(HttpStatusCode.OK) : req.CreateResponse(HttpStatusCode.BadRequest);

            return response;
        }

        [Function("UpdateTodo")]
        public async Task<HttpResponseData> Update(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "todo/{id}")]
            HttpRequestData req, FunctionContext executionContext, int id)
        {
            var logger = executionContext.GetLogger("GetTodos");
            logger.LogInformation("Updating Todo");

            var updated = await _helper.Update(id, req.Body);

            if (updated is not null)
            {
                var response = req.CreateResponse(HttpStatusCode.OK);

                await response.WriteAsJsonAsync(updated);

                return response;
            }
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }

        [Function("DeleteTodo")]
        public async Task<HttpResponseData> Delete(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "todo/{id}")]
            HttpRequestData req, FunctionContext executionContext, int id)
        {
            var logger = executionContext.GetLogger("GetTodos");
            logger.LogInformation("Deleting Todo");

            var deleted = await _helper.Delete(id);

            return deleted ? req.CreateResponse(HttpStatusCode.OK) : req.CreateResponse(HttpStatusCode.NotFound);

        }
    }
}
