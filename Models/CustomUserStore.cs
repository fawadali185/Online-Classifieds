using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using ListHell.CODE;
using AutoMapper;
using System.Security.Claims;

namespace ListHell.Models
{
    public class CustomUserStore : PasswordValidator, IUserStore<CODE.Users>,IUserPasswordStore<CODE.Users>,IUserLockoutStore<CODE.Users,string>,IUserClaimStore<CODE.Users>,IUserRoleStore<Users>

    {
        private LH_newEntities database;

        public CustomUserStore()
        {
            this.database = new LH_newEntities();
        }

        public void Dispose()
        {
            this.database.Dispose();
        }
        private string generateName()
        {
            Guid g = Guid.NewGuid();
            return g.ToString();
        }
        public async  Task CreateAsync(CODE.Users user)
        {
            // TODO
            
          
            ListHell.User u= new ListHell.User();
            u.Id = generateName();
            u.password = user.password;
            u.UserName = user.UserName;
            u.UserName = user.Id;
           // var x = await database.Users.Where(w => w.UserName == user.UserName).FirstOrDefaultAsync();
          //  if (x != null) { return; }
          var r=  this.database.Users.Add(u);
           this.database.SaveChanges();

           // ThrowIfDisposed();
           // await UpdateSecurityStampInternal(user).ConfigureAwait(false);
            //var result = await UserValidator<CODE.Users>.ValidateAsync(user).ConfigureAwait(false);
            //if (!result.Succeeded)
            //{
            //    return result;
            //}
            ////if (UserLockoutEnabledByDefault && SupportsUserLockout)
            //{
            //    await GetUserLockoutStore().SetLockoutEnabledAsync(user, true).ConfigureAwait(false);
            //}
            //await Store.CreateAsync(user).ConfigureAwait(false);
            IdentityResult r1 = IdentityResult.Success;
            return ;
            
            
          //  throw new NotImplementedException();
        }

        private void ThrowIfDisposed()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(CODE.Users user)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task DeleteAsync(CODE.Users user)
        {
            // TODO
            throw new NotImplementedException();
        }

        public async Task<CODE.Users> FindByIdAsync(string userId)
        {
            var v =await this.database.Users.Where(c => c.Id == userId).FirstOrDefaultAsync();
            CODE.Users u = new CODE.Users();
            if (v != null)
            {
                u.Email = v.UserName;
                foreach (var a in v.CustomRoles)
                {
                    u.Roles.Add(new CustomRoles { Id = a.Id, Name = a.Name });
                }
                u.UserName = v.UserName;
                return u;
            }
            //var config = new MapperConfiguration(cfg => {
            //    cfg.CreateMap<ListHell.User, CODE.Users>();
            //    cfg.CreateMap<ListHell.CustomRole, CODE.CustomRoles>();
            //}
            //);
            //var outer = config.CreateMapper();
            //u = outer.Map<CODE.Users>(v);

            // u.Roles = outer.Map<List<CODE.CustomRoles>>(v.CustomRoles);
            return null;
        }

        public async Task<CODE.Users> FindByNameAsync(string userName)
        {
            var v = await database.Users.Select(w=>w).Where(w=>w.UserName==userName).FirstOrDefaultAsync();
            CODE.Users u;
            if (v != null)
            {
               u = new CODE.Users { Id = v.Id, Email = v.UserName, password = v.password, UserName = v.UserName };
                u.Email = userName;
                return u;
            }
            //var config = new MapperConfiguration(cfg => { cfg.CreateMap<ListHell.User, CODE.Users>();
            //    cfg.CreateMap<ListHell.CustomRole, CODE.CustomRoles>();
            //}
            //);
            //var outer = config.CreateMapper();
            //u=   outer.Map<CODE.Users>(v);

            
           // u.Roles = outer.Map<List<CODE.CustomRoles>>(v.CustomRoles);
            return null;
        }

//     public  Task<IdentityResult> ValidateAsync(CODE.Users item
//)
//        {
//            CustomRoles c = new CustomRoles();

//            //or
           
//            var v = await database.Users.Select(w => new CODE.Users
//            { Roles = Mapper.Map<List<CustomRoles>>(w.CustomRoles) ,UserName=w.UserName
//            }).FirstOrDefaultAsync();

//            return v;
//        }
        public Task SetPasswordHashAsync(Users user, string passwordHash)
        {
         
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(Users user)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(user.password);
            return Task.FromResult( System.Convert.ToBase64String(plainTextBytes));
        }

        public Task<bool> HasPasswordAsync(Users user)
        {
            throw new NotImplementedException();
        }
        public async Task<IdentityResult> ValidateAsync(string item)
        {
            IdentityResult r = IdentityResult.Success;

            return r;
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(Users user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(Users user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(Users user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(Users user)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAccessFailedCountAsync(Users user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetLockoutEnabledAsync(Users user)
        {
            return Task.FromResult(false);
        }

        public Task SetLockoutEnabledAsync(Users user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Claim>> GetClaimsAsync(Users user)
        {
            var v = database.Users.Select(w => new {  Type=w.CustomRoles.FirstOrDefault().Name,Value="true"});
            List<Claim> lst=new List<Claim>();
            foreach(var v1 in v){
                lst.Add(new Claim(v1.Type, v1.Value));

            }
            return lst;
        }

        public Task AddClaimAsync(Users user, Claim claim)
        {
       
            throw new NotImplementedException();
        }

        public Task RemoveClaimAsync(Users user, Claim claim)
        {
            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(Users user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(Users user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(Users user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsInRoleAsync(Users user, string roleName)
        {
            return true;
        }
    }
}