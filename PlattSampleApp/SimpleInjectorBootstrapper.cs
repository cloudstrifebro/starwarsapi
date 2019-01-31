using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;
using PlattSampleApp.Interfaces;
using PlattSampleApp.Mappings;
using PlattSampleApp.Services;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlattSampleApp
{
    public static class SimpleInjectorBootstrapper
    {
        public static void Start()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            // Register your types, for instance:
            container.Register<IStarWarsService, StarWarsService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new StarWarsMaps());
            });

            container.RegisterInstance<MapperConfiguration>(config);
            container.Register<IMapper>(() => config.CreateMapper(container.GetInstance));

            // Optionally verify the container.
            try
            {
                container.Verify();
            }
            catch (Exception ex)
            {
                //log the exception thrown by the container
                throw;
            }

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}