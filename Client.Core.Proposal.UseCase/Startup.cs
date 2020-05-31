using InfluxDB.Client;
using InfluxDB.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Client.Core.Proposal.UseCase
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            var settings = Configuration.GetSection("influxDb").Get<InfluxSettings>();
            
            services
                .AddHttpContextAccessor()
                // not required to use these methods at all.  Simply convenience if a persons use case fits.
                .AddInfluxDb(builder =>
                {
                    // register an IInfluxClient for use in my DI lifetime.
                    builder
                        .Org(settings.Org)
                        .Bucket(settings.Bucket)
                        .ReadWriteTimeOut(TimeSpan.FromSeconds(10))
                        .Url(settings.Url)
                        .AuthenticateToken(settings.Token)
                        .LogLevel(InfluxDB.Client.Core.LogLevel.Headers)
                        // just added jitter interval here so i didn't have to port more code for the example.
                        // i'm not proposing this stay here in any way.
                        .JitterInterval(100);
                })
                // this class is independently useful and I would like to use it in some cases.
                .AddSingleton<IMeasurementMapper, MeasurementMapper>()
                .AddSingleton(provider =>
                {
                    // assume I have a write only use case to a single org/bucket that is configured above.
                    var apiClient = provider.GetRequiredService<IInfluxDBClient>();
                    return apiClient.GetWriteApi();
                })
                .AddSingleton(settings)
                .AddSingleton<IServiceImplementation, ServiceImplementation>()
                .AddControllers()
                .AddNewtonsoftJson()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
        }

        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseRouting()
                .UseEndpoints(x => x.MapControllers());

        }

    }
}
