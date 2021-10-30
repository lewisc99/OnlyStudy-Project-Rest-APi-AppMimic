using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mimic_Api.Helpers;
using Mimic_Api.V1.Repositories;
using Mimic_Api.V1.Repositories.Contracts;
using MimicApi.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mimic_Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //to configure autoMap, first you need to install the nuget packet AutoMapper
            //then you have to create a configuration to active the automap follow below


            //auto mapper e imagina que tem duas classes que nao tem nenhum relacionamento entre elas, mas que tem propriedades em comum
            // pode utilizar  o automapper para colocar a primeira classe e coloca as propriedades de outro objeto em outra claasse,
            //ele mapeai as propriedades de um objeto para outro.


            // automapper configuração abaixo
            var config = new MapperConfiguration(cft =>
            {
                cft.AddProfile(new DTOMapperProfile()); //ai precisa criar uma classe para configurar aqui dentro.
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);




            /* services.AddControllers().AddNewtonsoftJson(options =>
             {
                 options.SerializerSettings.ReferenceLoopHandling =
                   Newtonsoft.Json.ReferenceLoopHandling.Ignore;
             }) //esse codigo vai ignorar a referncia sitrica na seriliza��o na resposta json no metodo Action.
             ; */
            services.AddControllers();

            services.AddDbContext<MimicContext>(opt =>
            {
                opt.UseSqlite("Data Source=Database\\Mimic.db");
            });
            services.AddScoped<IPalavraRepository, PalavraRepository>(); //this IServiceCollection services, Type serviceType //no caso primeiro um serviço de Tipo T depois a implemtação


            //adicionar versionamento de API, precisa antes baixar o pacote nuget, 

        /*  <PackageReference Include="Microsoft.AspNetCore.Mvc.Versioning" Version="5.0.0" />
            <PackageReference Include="Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer" Version="5.0.0" /> */

            services.AddApiVersioning( cft =>
            { //esse codigo abaixo pode ate deixar em branco, isso e apenas para informar a versão da APi.
                cft.ReportApiVersions = true; //quand coloca essa informação vai informar quais versão da API são suportadas no sistema


              //  cft.ApiVersionReader = new HeaderApiVersionReader("api-version"); //dessa forma no querystring ou url, ou Cabeçalho,
                //para o usuario escolher a versão ee precisa colocar api-version= + o numero da versão.

                cft.AssumeDefaultVersionWhenUnspecified = true; //caso a versão da API não seja especificada pode usar eesse comonda
                                                                //que vai usar a versão padrão ao iniciar que no caso é a abaixa 1.0

             

                cft.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0); 
            }
                
                );


}

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.UseStatusCodePages(); //quando fazer um request, e retornar exemplo 404
//usando esse metodo vai retornar uma mensagem mais informal para o lado do cliente.

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
}
}
}
