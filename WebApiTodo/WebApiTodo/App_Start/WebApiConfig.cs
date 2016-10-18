using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApiTodo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var formatters = GlobalConfiguration.Configuration.Formatters; // Pegando a formatação de retorno
            var jsonFormatter = formatters.JsonFormatter; // Convertendo o retorno em Json
            var settings = jsonFormatter.SerializerSettings; // Serialização do retorno Json

            // Configurando o cabeçalho do retorno Json
            jsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;

            // Retirando a referencia dos objetos retornados no Json
            jsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            // Removendo o retorno em XML
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Indentando o Json
            settings.Formatting = Formatting.Indented;

            // Deixamos o cabeçalho no padrão de letra inicial minuscula
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Ativando o Cors para manipulação dos acessos a API
            config.EnableCors(); 

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
