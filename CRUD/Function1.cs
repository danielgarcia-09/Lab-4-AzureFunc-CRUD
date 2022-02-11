using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CRUD.Services;
using CRUD.Models;

namespace CRUD
{
    public class Function1
    {

        private readonly ITodoService _service;
        public Function1(ITodoService service)
        {
            this._service = service;
        }

        [FunctionName("GetTodo")]
        public async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Getting all todos.....");

            var todos = await _service.Get();

            return new OkObjectResult(todos);
        }

        [FunctionName("GetTodoById")]
        public async Task<IActionResult> GetById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "{id}")] HttpRequest req,
            ILogger log,
            int id)
        {
            log.LogInformation("Getting todo.....");

            var todo = await _service.GetById(id);

            return new OkObjectResult(todo);
        }

        [FunctionName("CreateTodo")]
        public async Task<IActionResult> Create([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Creating todo......");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var newTodo = JsonConvert.DeserializeObject<Todo>(requestBody);

            var todo = await _service.Create(newTodo);

            if (todo is null) return new BadRequestResult();

            return new OkObjectResult(todo);
        }

        [FunctionName("UpdateTodo")]
        public async Task<IActionResult> Update([HttpTrigger(AuthorizationLevel.Function, "put", Route = "{id}")] HttpRequest req,
            ILogger log,
            int id)
        {
            log.LogInformation("Updating todo......");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var newTodo = JsonConvert.DeserializeObject<Todo>(requestBody);

            var todo = await _service.Update(id, newTodo);

            if (todo is null) return new BadRequestResult();

            return new OkObjectResult(todo);
        }

        [FunctionName("Delete")]
        public async Task<IActionResult> Delete(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "{id}")] HttpRequest req,
            ILogger log,
            int id)
        {
            log.LogInformation("Deleting todo.....");

            var todo = await _service.Delete(id);

            if(todo) return new OkObjectResult(todo);

            return new BadRequestResult();
        }

    }
}
