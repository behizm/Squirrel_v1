using System.Security.Principal;
using Squirrel.Domain.Enititis;
using Squirrel.Service;
using Squirrel.Utility.Async;

namespace Squirrel.Web.Models
{

    public interface ISqPrincipal : IPrincipal
    {
        new SqIdentity Identity { get; }
    }

    public class SqPrincipal : ISqPrincipal
    {
        private readonly SqIdentity _identity;

        public SqPrincipal(SqIdentity identity)
        {
            _identity = identity;
        }

        public SqPrincipal(string username)
        {
            _identity = new SqIdentity(username);
        }

        IIdentity IPrincipal.Identity { get { return _identity; } }

        public SqIdentity Identity
        {
            get { return _identity; }
        }

        public bool IsInRole(string role)
        {
            return false;
        }
    }

    public class SqIdentity : IIdentity
    {
        public SqIdentity(string name)
        {
            Name = name;
            _filled = false;
        }

        private IUserService _userServicePrincipal;
        private IUserService UserServicePrincipal
        {
            get { return _userServicePrincipal ?? (_userServicePrincipal = ServiceIOC.Get<IUserService>()); }
        }

        private bool _filled;

        private void FillProps()
        {
            var user = AsyncTools.ConvertToSync(() => UserServicePrincipal.FindByUsernameAsync(Name));
            if (user == null)
                return;

            _email = user.Email;
            _isAdmin = user.IsAdmin;
            _profile = user.Profile;
            _filled = true;
        }

        public string Name { get; private set; }

        public string AuthenticationType
        {
            get { return "SQAUTH"; }
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(Name); }
        }

        private string _email;
        public string Email
        {
            get
            {
                if (!_filled)
                    FillProps();
                return _email;
            }
        }

        private bool? _isAdmin;
        public bool? IsAdmin
        {
            get
            {
                if (!_filled)
                    FillProps();
                return _isAdmin;
            }
        }

        private Profile _profile;
        public Profile Profile
        {
            get
            {
                if (!_filled)
                    FillProps();
                return _profile;
            }
        }
    }
}