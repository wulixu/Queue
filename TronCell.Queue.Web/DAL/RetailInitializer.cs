using System.Collections.Generic;
using System.Data.Entity;
using Queue.Entities.Models;

namespace TronCell.Queue.Web.DAL
{
    public class RetailInitializer:DropCreateDatabaseIfModelChanges<RetailDataContext>
    {
        protected override void Seed(RetailDataContext context)
        {
            base.Seed(context);
            var fittingRooms = new List<FittingRoom>()
            {
                new FittingRoom() {RoomName = "A001", Description = "A001"},
                new FittingRoom() {RoomName = "A002", Description = "A002"}
            };
            fittingRooms.ForEach(room => context.FittingRooms.Add(room));
            context.SaveChanges();

            var fitting = new List<Fitting>()
            {
                new Fitting() {FittingRoomId = 1 },
                new Fitting() {FittingRoomId = 2},
                new Fitting() {FittingRoomId = 1},
                new Fitting() {FittingRoomId = 2}
            };
            fitting.ForEach(fitting01 => context.Fittings.Add(fitting01));


            context.SaveChanges();
        }
    }
}