using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Squirrel.Domain.Enititis;
using Squirrel.Service;

namespace Squirrel.Web.Models
{

    public interface ISqPrincipal : IPrincipal
    {
    }

    public class SqPrincipal : ISqPrincipal
    {
        private readonly IPrincipal _principal;

        public IIdentity Identity
        {
            get
            {
                return _principal.Identity;
            }
            private set { }
        }

        public SqPrincipal(IPrincipal principal)
        {
            _principal = principal;
        }

        public SqPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
        }

        public bool IsInRole(string role)
        {
            return false;
        }
    }


    public static class PrincipalExtention
    {
        private static IUserService _userServicePrincipal;
        private static IUserService UserServicePrincipal
        {
            get { return _userServicePrincipal ?? (_userServicePrincipal = ServiceIOC.Get<IUserService>()); }
        }

        public static Guid? UserId(this IIdentity identity)
        {
            if (identity == null || string.IsNullOrEmpty(identity.Name))
                return null;

            var userTask = UserServicePrincipal.FindByUsernameAsync(identity.Name);
            userTask.Wait();
            var user = userTask.Result;
            if (user == null)
                return null;

            return user.Id;
        }

        public static string Email(this IIdentity identity)
        {
            if (identity == null || string.IsNullOrEmpty(identity.Name))
                return null;

            var userTask = UserServicePrincipal.FindByUsernameAsync(identity.Name);
            userTask.Wait();
            var user = userTask.Result;
            if (user == null)
                return null;

            return user.Email;
        }

        public static bool? IdAdmin(this IIdentity identity)
        {
            if (identity == null || string.IsNullOrEmpty(identity.Name))
                return null;

            var userTask = UserServicePrincipal.FindByUsernameAsync(identity.Name);
            userTask.Wait();
            var user = userTask.Result;
            if (user == null)
                return null;

            return user.IsAdmin;
        }

        public static Profile Profile(this IIdentity identity)
        {
            if (identity == null || string.IsNullOrEmpty(identity.Name))
                return null;

            var userTask = UserServicePrincipal.FindByUsernameAsync(identity.Name);
            userTask.Wait();
            var user = userTask.Result;
            if (user == null)
                return null;

            return user.Profile;
        }
    }
}