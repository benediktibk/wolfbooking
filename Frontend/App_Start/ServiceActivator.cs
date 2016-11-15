using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Frontend.DependencyResolution;
using StructureMap;

namespace Frontend
{
    public class ServiceActivator : IHttpControllerActivator
    {
        private readonly Container _container;

        public ServiceActivator(HttpConfiguration configuration)
        {
            _container = new Container(new DefaultRegistry());
        }

        public IHttpController Create(HttpRequestMessage request
            , HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var controller = _container.GetInstance(controllerType) as IHttpController;
            return controller;
        }
    }
}