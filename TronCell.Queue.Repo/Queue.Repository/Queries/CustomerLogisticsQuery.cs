using Queue.Entities.Models;
using Repository.Pattern.Ef6;

namespace Queue.Repository.Queries
{
    public class CustomerLogisticsQuery : QueryObject<Fitting>
    {
        public CustomerLogisticsQuery FromFittingRoom(int fittingRoomId)
        {
            And(x => x.FittingRoomId == fittingRoomId);
            return this;
        }
    }
}