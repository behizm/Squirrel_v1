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

        private IRepositoryContext _repositoryContext3;
        protected IRepositoryContext RepositoryContext3
        {
            get { return _repositoryContext3 ?? (_repositoryContext3 = DataIOC.Get<IRepositoryContext>()); }
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

        private IWarehouseContext _warehouseContext3;
        protected IWarehouseContext WarehouseContext3
        {
            get { return _warehouseContext3 ?? (_warehouseContext3 = DataIOC.Get<IWarehouseContext>()); }
        }

        public OperationResult Result { get; protected set; }
    }
}
