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

        private IRepositoryContext _repositoryContext2;
        protected IRepositoryContext RepositoryContext2
        {
            get { return _repositoryContext2 ?? (_repositoryContext2 = DataIOC.Get<IRepositoryContext>()); }
        }

        private IWarehouseContext _warehouseContext;
        protected IWarehouseContext WarehouseContext
        {
            get { return _warehouseContext ?? (_warehouseContext = DataIOC.Get<IWarehouseContext>()); }
        }

        private IWarehouseContext _warehouseContext2;
        protected IWarehouseContext WarehouseContext2
        {
            get { return _warehouseContext2 ?? (_warehouseContext2 = DataIOC.Get<IWarehouseContext>()); }
        }

        public OperationResult Result { get; protected set; }
    }
}
