using Microsoft.Practices.Unity;
using Squirrel.Service.Services;

namespace Squirrel.Service
{
    // ReSharper disable once InconsistentNaming
    public class ServiceIOC
    {
        public static T Get<T>() where T : class
        {
            if (!_isStarted)
                Start();

            return _core.Resolve<T>();
        }

        static IUnityContainer _core;
        static bool _isStarted;

        static void Start()
        {
            _core = new UnityContainer();
            Register();
            _isStarted = true;
        }

        static void Register()
        {
            _core.RegisterType<IUserService, UserService>();
        }
    }
}