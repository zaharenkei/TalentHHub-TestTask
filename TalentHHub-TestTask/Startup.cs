using FluentValidation;
using FluentValidation.AspNetCore;
using TalentHHub_TestTask.Handlers.Models;
using TalentHHub_TestTask.Handlers;
using TalentHHub_TestTask.Infrastructure.Middleware;
using TalentHHub_TestTask.Infrastructure.Settings;

namespace TalentHHub_TestTask
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
            services.Configure<PriceSettings>(Configuration.GetSection("Price"));
            services.AddTransient<IHandler<CalculationRequest, CalculationResponse>, CalculateTotalChargeQuery>();
            services.AddValidatorsFromAssemblyContaining<Startup>();
            services.AddFluentValidationAutoValidation(configuration => configuration.DisableDataAnnotationsValidation = true);

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddLogging();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<LoggingMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
