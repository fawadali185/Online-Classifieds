using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin;
using System.Data.Entity.SqlServer.Utilities;
using System;
using System.Collections.Generic;
using ListHell.CODE;

namespace ListHell.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
           
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public virtual DbSet<CODE.Users> User { get; set; }
        public virtual DbSet<CODE.CustomRoles> Role { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("security");
            modelBuilder.Entity<ApplicationUser>().ToTable("Users", "security");
            modelBuilder.Entity<CustomRole>().ToTable("CustomRoles", "security");

        }
    }

    public class CustomUserManager : UserManager<CODE.Users>,IUserClaimStore<CODE.Users>
    {
      
        public CustomUserManager(IUserStore<CODE.Users> store)
            : base(store)
        {
            this.Store = store;
            
        }
        public static CustomUserManager Create()
        {
            var manager = new CustomUserManager(new CustomUserStore());
            return manager;
        }
        public async override System.Threading.Tasks.Task<IdentityResult> CreateAsync(CODE.Users user)
        {
           await this.Store.CreateAsync(user);
            IdentityResult result =  IdentityResult.Success;
            return  result;
           
        }
         public async override Task<CODE.Users> FindAsync(string userName, string password)
        {
           var v= this.Store.FindByNameAsync(userName);


            return null;

        }

        public override Task<ClaimsIdentity> CreateIdentityAsync(CODE.Users user, string authenticationType)
        {
           
          var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            //var identity = new ClaimsIdentity();
            //identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            var claims = new List<Claim>();
         //var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
         //role.Name = "auser";
         //roleManager.Create(role);

            claims.Add(new Claim(ClaimTypes.Name, user.Id));

           // foreach(var v in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, "auser"));
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
                //this.AddClaim(user.UserName, new Claim(v.Name, "true"));
            }
            AddClaimAsync(user, new Claim(ClaimTypes.Role, "auser"));
            
           
            var id = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            return Task.FromResult(id);
        }

        public Task<IList<Claim>> GetClaimsAsync(Users user)
        {
            throw new NotImplementedException();
        }

        public async Task AddClaimAsync(Users user, Claim claim)
        {
            return;
        }

        public Task RemoveClaimAsync(Users user, Claim claim)
        {
            throw new NotImplementedException();
        }

        Task IUserStore<Users, string>.CreateAsync(Users user)
        {
            throw new NotImplementedException();
        }

        Task IUserStore<Users, string>.UpdateAsync(Users user)
        {
            throw new NotImplementedException();
        }

        Task IUserStore<Users, string>.DeleteAsync(Users user)
        {
            throw new NotImplementedException();
        }
    }

    public class CustomSignInManager : SignInManager<CODE.Users, string>
    {
        public CustomSignInManager(CustomUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }
        public static CustomSignInManager Create(IdentityFactoryOptions<CustomSignInManager> options, IOwinContext context)
        {
            return new CustomSignInManager(context.GetUserManager<CustomUserManager>(), context.Authentication);
        }

        public async override Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            //  base.PasswordSignInAsync(userName, password, isPersistent, shouldLockout);

            //  CODE.Users u = await this.UserManager.FindByNameAsync(userName);
            //await  this.UserManager.CreateIdentityAsync(u, this.AuthenticationType);
            //  return ( SignInStatus.Success);


            if (UserManager == null)
            {
                return SignInStatus.Failure;
            }

            var user = await this.UserManager.FindByNameAsync(userName);
            if (user == null)
            {
                return SignInStatus.Failure;
            }

            if (UserManager.SupportsUserLockout
                && await UserManager.IsLockedOutAsync(user.Id))
            {
                return SignInStatus.LockedOut;
            }

            if (UserManager.SupportsUserPassword
                && await UserManager.CheckPasswordAsync(user, password)
                                    .WithCurrentCulture())
            {
                return await SignInOrTwoFactor(user, isPersistent);
            }
            if (shouldLockout && UserManager.SupportsUserLockout)
            {
                // If lockout is requested, increment access failed count
                // which might lock out the user
                await UserManager.AccessFailedAsync(user.Id);
                if (await UserManager.IsLockedOutAsync(user.Id))
                {
                    return SignInStatus.LockedOut;
                }
            }

            var userIdentity = await CreateUserIdentityAsync(user).WithCurrentCulture();

            // Clear any partial cookies from external or two factor partial sign ins
            AuthenticationManager.SignOut(
                DefaultAuthenticationTypes.ExternalCookie,
                DefaultAuthenticationTypes.TwoFactorCookie);
            
            if (isPersistent)
            {
                var rememberBrowserIdentity = AuthenticationManager
                    .CreateTwoFactorRememberBrowserIdentity(ConvertIdToString(user.Id));

                AuthenticationManager.SignIn(
                    new AuthenticationProperties { IsPersistent = isPersistent },
                    userIdentity,
                    rememberBrowserIdentity);
            }
            else
            {
                //AuthenticationManager.SignIn(
                //    new AuthenticationProperties { IsPersistent = isPersistent },
                //    userIdentity);
                AuthenticationManager.SignIn(userIdentity);
                return SignInStatus.Success;
            }
            return SignInStatus.Failure;
        }
        private async Task<SignInStatus> SignInOrTwoFactor(CODE.Users user, bool isPersistent)
        {
            var id = Convert.ToString(user.Id);

            if (UserManager.SupportsUserTwoFactor
                && await UserManager.GetTwoFactorEnabledAsync(user.Id)
                                    .WithCurrentCulture()
                && (await UserManager.GetValidTwoFactorProvidersAsync(user.Id)
                                     .WithCurrentCulture()).Count > 0
                    && !await AuthenticationManager.TwoFactorBrowserRememberedAsync(id)
                                                   .WithCurrentCulture())
            {
                var identity = new ClaimsIdentity(
                    DefaultAuthenticationTypes.TwoFactorCookie);

                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, id));

                AuthenticationManager.SignIn(identity);

                return SignInStatus.RequiresVerification;
            }
            await SignInAsync(user, isPersistent, false).WithCurrentCulture();
            return SignInStatus.Success;
        }
    }
}