using Autofac;
using Autofac.Integration.WebApi;
using GoodPractices_Model;
using GoodPractices_Engine;
using System.Reflection;
using System.Web.Http;

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

            builder.RegisterType<Validation>().As<IValidation>();
            builder.RegisterType<SchoolDBContext>().As<ISchoolDBContext>();
            builder.RegisterType<CourseEngine>();
            builder.RegisterType<TeacherEngine>();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }


}
