using Backend.Facade;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Frontend.App_Start;
using Microsoft.AspNet.Identity.Owin;

namespace Frontend.Controllers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public AuthorizationServerProvider()
        {
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var bookingFacade = StructuremapMvc.StructureMapDependencyScope.Container.GetInstance<BookingFacade>();
            if (bookingFacade.IsLoginValid(context.UserName, context.Password)!=SignInStatus.Success)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var user = bookingFacade.GetUserByUsername(context.UserName);

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));

            foreach (var role in user.Roles)
                identity.AddClaim(new Claim(ClaimTypes.Role, role.Name));

            context.Validated(identity);
        }
    }
}