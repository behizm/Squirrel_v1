using System;
using Squirrel.Domain.Enititis;

namespace Squirrel.Domain.ViewModels
{
    public class ProfileSearchModel
    {
        public Guid? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
    }
}
