using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Web.Http;

public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        // 🔁 Fix for Self-referencing Loop
        config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

        // 🔧 Optional: Remove XML
        var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes
            .FirstOrDefault(t => t.MediaType == "application/xml");
        if (appXmlType != null)
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

        // Default API route
        config.MapHttpAttributeRoutes();
        config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
        );
    }

}
