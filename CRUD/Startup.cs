using CRUD.Context;
using CRUD.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(CRUD.Startup))]
namespace CRUD
{
    public class Startup : FunctionsStartup
    {
        IConfiguration Configuration;
        public override void Configure(IFunctionsHostBuilder builder)
        {
            Configuration = builder.GetContext().Configuration;

            var connectionString = Configuration["ConnectionString"];

            builder.Services.AddDbContext<TodoContext>(op => SqlServerDbContextOptionsExtensions.UseSqlServer(op, connectionString));  
            builder.Services.AddTransient<ITodoService, TodoService>();     
        }
    }
}
