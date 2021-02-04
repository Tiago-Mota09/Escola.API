using Angis.CTe.API.Filters;
using AutoMapper;
using Dapper;
using Escola.API.Business;
using Escola.API.Data.Repositories;
using Escola.API.Domain.Models.Request;
using Escola.API.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using static Escola.API.Filters.ValidateModelFilter;

namespace Escola.API
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
            #region :: FluentValidation ::
            services.AddMvc(options => { options.Filters.Add(typeof(ValidateModelAttribute)); }).AddFluentValidation();
            services.AddScoped<IValidator<AlunoRequest>, AlunoValidator>();
            services.AddScoped<IValidator<AlunoUpdateRequest>, AlunoUpdateValidator>();
            services.AddScoped<IValidator<UnidadeRequest>, UnidadeValidator>();
            services.AddScoped<IValidator<UnidadeUpdateRequest>, UnidadeUpdateValidator>();
            services.AddScoped<IValidator<ProfessorRequest>, ProfessorValidator>();
            services.AddScoped<IValidator<ProfessorUpdateRequest>, ProfessorUpdateValidator>();
            services.AddScoped<IValidator<ProfessorAlunoRequest>, ProfessorAlunoValidator>();
            services.AddScoped<IValidator<ProfessorAlunoUpdateRequest>, ProfessorAlunoUpdateValidator>();
            #endregion

            #region :: Automapper ::
            services.AddAutoMapper(typeof(Startup));
            #endregion

            #region :: Swagger ::
            services.AddSwaggerGen(configuration =>
            {
                configuration.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Escola API",
                    Version = "v1"
                });

               var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
               var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                configuration.IncludeXmlComments(xmlPath);
            });
            #endregion

            #region :: Dapper ::
            services.AddScoped<AlunoRepository>();
            services.AddScoped<UnidadeRepository>();
            services.AddScoped<ProfessorRepository>();
            services.AddScoped<ProfessorAlunoRepository>();
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            #endregion

            #region :: Business ::
            services.AddTransient<AlunoBL>();
            services.AddTransient<UnidadeBL>();
            services.AddTransient<ProfessorBL>();
            services.AddTransient<ProfessorAlunoBL>();
            #endregion

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(conf =>
            {
                conf.SwaggerEndpoint("/swagger/v1/swagger.json", "Escola API");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
