using Meeeeeediator.Api.InputFormatters;
using Meeeeeediator.Application.Behaviors;
using Meeeeeediator.Application.Post.DataAccess;
using Meeeeeediator.Application.Queries;
using Meeeeeediator.Core;
using Meeeeeediator.Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;

namespace Meeeeeediator.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.InputFormatters.Insert(0, new RawJsonBodyInputFormatter());
            });

            services.AddHttpClient("General");

            services.AddScoped(typeof(IBehavior<,>), typeof(PerformanceBehavior<,>));
            services.AddScoped(typeof(IBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddScoped(typeof(IBehavior<,>), typeof(TestBehavior<,>));

            services.AddScoped<IPostFetcher>(sp =>
                new PostFetcher(sp.GetRequiredService<IHttpClientFactory>().CreateClient("General")));

            services.AddMediator(typeof(EchoQueryHandler).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
