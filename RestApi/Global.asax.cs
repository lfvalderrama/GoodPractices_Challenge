using Autofac;
using Autofac.Integration.WebApi;
using GoodPractices_Model;
using GoodPractices_Engine;
using System.Reflection;
using System.Web.Http;
using RestApi.Helpers;

namespace RestApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            BindDependencies();

            var config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }

        private void BindDependencies()
        {
            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<Validation>().As<IValidation>().InstancePerRequest();
            builder.RegisterType<SchoolDBContext>().As<ISchoolDBContext>().InstancePerRequest();
            builder.RegisterType<CodeHandler>().As<ICodeHandler>().InstancePerRequest();
            builder.RegisterType<CourseEngine>().InstancePerRequest();
            builder.RegisterType<TeacherEngine>().InstancePerRequest();
            builder.RegisterType<SubjectEngine>().InstancePerRequest();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }


}
