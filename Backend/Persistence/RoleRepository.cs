namespace Backend.Persistence
{
    public class RoleRepository
    {
        private WolfBookingContextFactory _contextFactory;

        public RoleRepository(WolfBookingContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
    }
}
