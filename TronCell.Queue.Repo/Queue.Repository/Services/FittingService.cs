using System.Collections.Generic;
using Queue.Entities.Models;
using Queue.Repository.Repositories;
using Repository.Pattern.Repositories;
using Repository.Pattern.Service;

namespace Queue.Repository.Services
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
