using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Backend.Persistence
{
    public class WolfBookingUserRole : IdentityUserRole<int> { }
    public class WolfBookingUserClaim : IdentityUserClaim<int> { }
    public class WolfBookingUserLogin : IdentityUserLogin<int> { }

    public class WolfBookingRole : IdentityRole<int, WolfBookingUserRole>
    {
        public WolfBookingRole() { }
        public WolfBookingRole(string name) { Name = name; }
    }

    public class WolfBookingUserStore : UserStore<User, WolfBookingRole, int,
        WolfBookingUserLogin, WolfBookingUserRole, WolfBookingUserClaim>
    {
        public WolfBookingUserStore(WolfBookingContext context)
            : base(context)
        {
        }
    }

    public class WolfBookingRoleStore : RoleStore<WolfBookingRole, int, WolfBookingUserRole>
    {
        public WolfBookingRoleStore(WolfBookingContext context)
            : base(context)
        {
        }
    }

    public class WolfBookingUserManager : UserManager<User, int>
    {
        public WolfBookingUserManager(IUserStore<User, int> store)
            : base(store)
        {
        }

        public static WolfBookingUserManager Create(
            IdentityFactoryOptions<WolfBookingUserManager> options, IOwinContext context)
        {
            var manager = new WolfBookingUserManager(
                new WolfBookingUserStore(context.Get<WolfBookingContext>()));
            // Configure validation logic for usernames 
            manager.UserValidator = new UserValidator<User, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords 
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            // Register two factor authentication providers. This application uses Phone 
            // and Emails as a step of receiving a code for verifying the user 
            // You can write your own provider and plug in here. 
            manager.RegisterTwoFactorProvider("PhoneCode",
                new PhoneNumberTokenProvider<User, int>
                {
                    MessageFormat = "Your security code is: {0}"
                });
            manager.RegisterTwoFactorProvider("EmailCode",
                new EmailTokenProvider<User, int>
                {
                    Subject = "Security Code",
                    BodyFormat = "Your security code is: {0}"
                });
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<User, int>(
                        dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.  
    public class WolfBookingSignInManager : SignInManager<User, int>
    {
        public WolfBookingSignInManager(WolfBookingUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager)
        { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((WolfBookingUserManager)UserManager);
        }

        public static WolfBookingSignInManager Create(IdentityFactoryOptions<WolfBookingSignInManager> options, IOwinContext context)
        {
            return new WolfBookingSignInManager(context.GetUserManager<WolfBookingUserManager>(), context.Authentication);
        }
    }
}
