using Squirrel.Domain.Enititis;

namespace Squirrel.Domain.ExtensionMethods
{
    public static class UserExtensions
    {
        public static string ShowName(this User user)
        {
            if (user.Profile == null)
            {
                return user.Username;
            }

            if (string.IsNullOrEmpty(user.Profile.Firstname) && string.IsNullOrEmpty(user.Profile.Lastname))
            {
                return user.Username;
            }

            return string.Format("{0} {1}", user.Profile.Firstname, user.Profile.Lastname);
        }

        public static string AvatarAddress(this User user)
        {
            if (user.Profile == null || !user.Profile.AvatarId.HasValue)
            {
                return null;
            }

            return user.Profile.Avatar.Address;
        }
    }
}
