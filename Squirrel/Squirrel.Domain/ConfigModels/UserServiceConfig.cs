using System;

namespace Squirrel.Domain.ConfigModels
{
    public class UserServiceConfig
    {
        public int MinimumPasswordLenght { get; set; }
        public int MaximumAccessFailed { get; set; }
        public TimeSpan LockTimeSpan { get; set; }
        public bool AutoActive { get; set; }
    }
}
