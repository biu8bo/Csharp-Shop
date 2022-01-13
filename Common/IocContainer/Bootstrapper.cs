
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Configuration;
using System.Web.Mvc;
using Unity.Mvc4;

namespace MVC×¿Ô½ÏîÄ¿
{
    public static class Bootstrapper
    {
        public static IUnityContainer container;
        public static T Resolve<T>()
        {
            try
            {
                return container.Resolve<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static IUnityContainer Initialise()
        {
            container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            RegisterTypes(container);
            // e.g. container.RegisterType<ITestService, TestService>();
            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            UnityConfigurationSection configuration = (UnityConfigurationSection)ConfigurationManager.GetSection(UnityConfigurationSection.SectionName);
            configuration.Configure(container, "IocService");
        }
    }
}