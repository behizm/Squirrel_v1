using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Squirrel.Data.Services;
using Squirrel.Domain.ResultModels;

namespace Squirrel.Data
{
    // ReSharper disable once InconsistentNaming
    public static class DataIOC
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
            _core.RegisterType<IRepositoryContext, RepositoryContext<SquirrelContext>>();
            _core.RegisterType<IWarehouseContext, WarehouseContext<SquirrelContext>>();
        }
    }
}