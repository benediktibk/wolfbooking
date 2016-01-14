using System;
using System.Data.Entity.Infrastructure;

namespace Backend.Persistence
{
    class WolfBookingContextFactory : IDbContextFactory<WolfBookingContext>
    {
        public WolfBookingContext Create()
        {
            return Factory.CreateWolfBookingContext();
        }
    }
}
