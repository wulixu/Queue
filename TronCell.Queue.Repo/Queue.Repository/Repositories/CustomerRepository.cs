using System.Collections.Generic;
using System.Linq;
using Queue.Entities.Models;
using Repository.Pattern.Repositories;

namespace Queue.Repository.Repositories
{
    // Exmaple: How to add custom methods to a repository.
    public static class FittingRepository
    {
        public static IEnumerable<Fitting> GetFittingByRoomName(this IRepository<Fitting> repository, string roomName)
        {
            return repository
                .Queryable()
                .Where(c => c.FittingRoom.RoomName == roomName)
                .AsEnumerable();
        }

        public static IEnumerable<Fitting> FittingsByRoomName(this IRepositoryAsync<Fitting> repository, string roomName)
        {
            return repository
                .Queryable()
                .Where(x => x.FittingRoom.RoomName == roomName)
                .AsEnumerable();
        }
    }

    
}