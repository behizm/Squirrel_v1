using Squirrel.Data;
using Squirrel.Domain.ResultModels;

namespace Squirrel.Service.Services
{
    class BaseService
    {
        private IRepositoryContext _repositoryContext;
        protected IRepositoryContext RepositoryContext
        {
            get { return _repositoryContext ?? (_repositoryContext = DataIOC.Get<IRepositoryContext>()); }
        }

        private IWarehouseContext _warehouseContext;
        protected IWarehouseContext WarehouseContext
        {
            get { return _warehouseContext ?? (_warehouseContext = DataIOC.Get<IWarehouseContext>()); }
        }

        public OperationResult Result { get; protected set; }
    }
}
