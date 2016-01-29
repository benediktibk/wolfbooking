using Backend.Facade;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Frontend.Controllers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly BookingFacade _bookingFacade;

        public AuthorizationServerProvider(BookingFacade bookingFacade)
        {
            _bookingFacade = bookingFacade;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            if (!_bookingFacade.IsLoginValid(context.UserName, context.Password))
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));
            context.Validated(identity);
        }
    }
}