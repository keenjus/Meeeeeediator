using Meeeeeediator.Api.InputFormatters;
using Meeeeeediator.Application.Behaviors;
using Meeeeeediator.Application.Post;
using Meeeeeediator.Application.Post.DataAccess;
using Meeeeeediator.Application.Post.Queries;
using Meeeeeediator.Application.Queries;
using Meeeeeediator.Core.DependencyInjection.Microsoft;
using Meeeeeediator.Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace Meeeeeediator.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.InputFormatters.Insert(0, new RawJsonBodyInputFormatter());
            });

            services.AddScoped(typeof(IBehavior<,>), typeof(PerformanceBehavior<,>));
            services.AddScoped(typeof(IBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddScoped(typeof(IBehavior<,>), typeof(CachingBehavior<,>));

            services.AddScoped<IPostFetcher, PostFetcher>();

            services.AddMemoryCache();

            services.AddMediator(typeof(EchoQueryHandler).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
