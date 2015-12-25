using System.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Net.Http;
using Backend.Business;
using Backend.Persistence;
using System.Net;

namespace Facade.Controllers
{
    public class BreadsController : ApiController
    {
        private static BreadFactory _breadFactory;
        private static BreadRepository _breadRepository;

        static BreadsController()
        {
            var databaseConnectionString = GetDatabaseConnectionString();
            _breadRepository = new BreadRepository(databaseConnectionString);
            _breadFactory = new BreadFactory(_breadRepository);
        }

        private static string GetDatabaseConnectionString()
        {
            var hostName = Environment.MachineName;
            return ConfigurationManager.ConnectionStrings[hostName].ConnectionString;
        }

        public IEnumerable<Models.Bread> GetCurrentAvailableBreads()
        {
            return _breadFactory.GetCurrentAvailableBreads().Select(x => new Models.Bread(x));
        }

        public Models.Bread GetBread(int id)
        {
            var bread = _breadFactory.Get(id);

            if (bread == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return new Models.Bread(bread);
        }
    }
}
