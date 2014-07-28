using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Pattern.Repositories;
using Repository.Pattern.Service;
using Retail.Entities.Models;
using Retail.Repository.Repositories;
using Service.Pattern;

namespace Retail.Service
{

    /// <summary>
    ///     Add any custom business logic (methods) here
    /// </summary>
    public interface IFittingService : IService<Fitting>
    {
        IEnumerable<Fitting> GetFittingByRoomName(string roomName);
    }

    public class FittingService : Service<Fitting>, IFittingService
    {
        private readonly IRepositoryAsync<Fitting> _repository;

        public FittingService(IRepositoryAsync<Fitting> repository)
            : base(repository)
        {
            _repository = repository;
        }


        public IEnumerable<Fitting> GetFittingByRoomName(string roomName)
        {
            return _repository.FittingsByRoomName(roomName);
        }
    }

}
