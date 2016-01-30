using Backend;
using Backend.Facade;

namespace Frontend.Controllers
{
    public class AccountsController : Controller
    {
        private BookingFacade _bookingFacade;

        public AccountsController()
        {
            _bookingFacade = Factory.BookingFacade;
        }
    }
}