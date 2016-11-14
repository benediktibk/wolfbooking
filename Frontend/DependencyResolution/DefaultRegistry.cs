// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Web;
using Backend.Persistence;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using StructureMap;

namespace Frontend.DependencyResolution {
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
	
    public class DefaultRegistry : Registry {
        #region Constructors and Destructors

        public DefaultRegistry() {
            Scan(
                scan => {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
					scan.With(new ControllerConvention());
                });

            Scan(scan =>
            {
                scan.Assembly("Backend");
                scan.WithDefaultConventions();
            });

            var hostName = Environment.MachineName;
            var databaseConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[hostName].ConnectionString;
            For<WolfBookingContext>().Use<WolfBookingContext>()
                .Ctor<string>("databaseConnectionString").Is(databaseConnectionString);

            For<SignInManager<User, int>>().Use<WolfBookingSignInManager>();
            For<IUserStore<User, int>>().Use<WolfBookingUserStore>();
            For<UserManager<User, int>>().Use<WolfBookingUserManager>();//ctx => new WolfBookingUserManager(ctx.GetInstance<WolfBookingUserStore>(), null));
            //.Ctor<IDataProtectionProvider>("dataProtectionProvider").Is((IDataProtectionProvider)null);
            For<IDataProtectionProvider>().Use<DpapiDataProtectionProvider>();
            For<IRoleStore<WolfBookingRole, Int32>>().Use<WolfBookingRoleStore>();


            For<IAuthenticationManager>().Use(ctx => HttpContext.Current.GetOwinContext().Authentication);

            //(s => WolfBookingUserManager.Create(container.GetInstance<IDataProtectionProvider>(), container.GetInstance<WolfBookingUserStore>()));
            //For<RoleManager<WolfBookingRole, int>>().Use(new Rolema)

            //public static WolfBookingSignInManager SignInManager => HttpContext.Current.GetOwinContext().Get<WolfBookingSignInManager>();
            //        public static WolfBookingUserManager UserManager => HttpContext.Current.GetOwinContext().GetUserManager<WolfBookingUserManager>();
            //        public static WolfBookingContext DbContext => StructuremapMvc.StructureMapDependencyScope.Container.HttpContext.Current.GetOwinContext().Get<WolfBookingContext>();

            //        public static RoleManager<WolfBookingRole, int> RoleManager => new RoleManager<WolfBookingRole, int>(new WolfBookingRoleStore(DbContext));


        }

    #endregion
}
}