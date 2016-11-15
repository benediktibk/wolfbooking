using Backend.Persistence;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Backend.Facade
{
    public class BookingFacade
    {
        #region repositories

        private readonly BreadRepository _breadRepository;
        private readonly UserRepository _userRepository;
        private readonly RoomRepository _roomRepository;
        private readonly BreadBookingsRepository _breadBookingsRepository;

        #endregion

        #region constructor

        public BookingFacade(BreadRepository breadRepository, UserRepository userRepository, RoomRepository roomRepository, BreadBookingsRepository breadBookingsRepository)
        {
            _breadRepository = breadRepository;
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _breadBookingsRepository = breadBookingsRepository;
        }

        #endregion constructor

        #region GET

        public IList<Room> GetCurrentAvailableRooms()
        {
            return _roomRepository.GetCurrentAvailableRooms().Select(x => new Room(x)).ToList();
        }

        public IList<Bread> GetCurrentAvailableBreads()
        {
            return _breadRepository.GetCurrentAvailableBreads().Select(x => new Bread(x)).ToList();
        }

        public Room GetRoom(int id)
        {
            var room = _roomRepository.Get(id);
            return room == null ? null : new Room(room);
        }

        public User GetUser(int id)
        {
            var user = _userRepository.Get(id);
            return user == null ? null : new User(user);
        }

        public User GetUserByUsername(string username)
        {
            var user = _userRepository.GetByLogin(username);
            return user == null ? null : new User(user);
        }

        public IList<Bread> GetAllBreads()
        {
            var breads = _breadRepository.GetAll();
            return breads.Select(x => new Bread(x)).ToList();
        }

        public IList<Bread> GetBreads(IEnumerable<int> ids)
        {
            var breads = _breadRepository.Get(ids);
            return breads.Select(x => new Bread(x)).ToList();
        }

        public Bread GetBread(int id)
        {
            var bread = _breadRepository.Get(id);
            return bread == null ? null : new Bread(bread);
        }

        public IEnumerable<WolfBookingRole> GetAllRoles()
        {
            return _userRepository.GetAllRoles();
        }

        public SignInStatus IsLoginValid(string login, string password)
        {
            return _userRepository.Login(login, password);
            
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            //    case SignInStatus.Failure:
            //    default:
            //        ModelState.AddModelError("", "Invalid login attempt.");
            //        return View(model);
            //}
        }

        public bool IsRoomInUse(int id)
        {
            var room = _roomRepository.Get(id);

            if (room == null)
                return false;

            return _roomRepository.IsRoomInUse(room);
        }

        public bool IsUserAllowedToSeeRoom(string login, int roomId)
        {
            var user = _userRepository.GetByLogin(login);

            if (user.Room == roomId)
                return true;

            var roleNames = _userRepository.GetUserRoleNamesForUserName(login);
            return roleNames.Contains("Admin") || roleNames.Contains("Manager");
        }

        public bool IsUserAllowedToSeeDataOfUser(string currentUser, string username)
        {
            var user = _userRepository.GetByLogin(currentUser);

            if (user.UserName == username)
                return true;

            var roleNames = _userRepository.GetUserRoleNamesForUserName(user.UserName);
            return roleNames.Contains("Administrators") || roleNames.Contains("Managers");
        }

        public BreadBookings GetCurrentBreadBookingsForRoom(int id)
        {
            var breadBookings = _breadBookingsRepository.GetCurrentBreadBookingsForRoom(id);
            return breadBookings == null ? null : new BreadBookings(breadBookings);
        }

        public IList<BreadBookings> GetCurrentBreadBookingsForAllRooms()
        {
            var result = new List<BreadBookings>();
            var rooms = GetCurrentAvailableRooms();

            foreach (var room in rooms)
            {
                var breadBookings = _breadBookingsRepository.GetCurrentBreadBookingsForRoom(room.Id);

                if (breadBookings == null)
                    throw new Exception($"couldn't load bread bookings for room {room.Id}");

                result.Add(new BreadBookings(breadBookings));
            }

            return result;
        }

        public Bill CalculateBill(int room, DateTime startDate, DateTime endDate)
        {
            var bill = _breadBookingsRepository.GetBreadBookingsForRoomBetween(room, startDate, endDate);
            return bill == null ? null : new Bill(bill);
        }

        #endregion

        #region ADD

        public object AddRoom(Room room)
        {
            return _roomRepository.Add(new Business.Room(room));
        }

        public int AddUser(User user, string password)
        {
            return _userRepository.Add(new Business.User(user), password);
        }

        public int AddBread(Bread bread)
        {
            return _breadRepository.Add(new Business.Bread(bread));
        }

        #endregion

        #region UPDATE

        public bool UpdateRoom(Room room)
        {
            var businessRoom = _roomRepository.Get(room.Id);

            if (businessRoom == null)
                return false;

            businessRoom.UpdateWith(room);
            _roomRepository.Update(businessRoom);
            return true;
        }

        public bool UpdateBread(Bread bread)
        {
            var businessBread = _breadRepository.Get(bread.Id);

            if (businessBread == null)
                return false;

            businessBread.UpdateWith(bread);
            _breadRepository.Update(businessBread);
            return true;
        }

        public bool UpdateUser(User user)
        {
            var businessUser = _userRepository.Get(user.Id);

            if (businessUser == null)
                return false;

            businessUser.UpdateWith(user);
            _userRepository.Update(businessUser);
            return true;
        }

        public bool UpdateBreadBookings(BreadBookings breadBookings)
        {
            var businessBreadBookings = _breadBookingsRepository.GetBreadBookingsById(breadBookings.Id);

            if (businessBreadBookings == null || 
                breadBookings.Room != businessBreadBookings.Room || 
                businessBreadBookings.AlreadyOrdered)
                return false;

            businessBreadBookings.UpdateWith(breadBookings);
            _breadBookingsRepository.Update(businessBreadBookings);
            return true;
        }

        public bool UpdateBreadBookingsAndSetAsOrdered(BreadBookings breadBookings)
        {
            var businessBreadBookings = _breadBookingsRepository.GetBreadBookingsById(breadBookings.Id);
            businessBreadBookings.MarkAsOrdered();

            if (businessBreadBookings == null || 
                breadBookings.Room != businessBreadBookings.Room)
                return false;

            businessBreadBookings.UpdateWith(breadBookings);
            _breadBookingsRepository.Update(businessBreadBookings);
            return true;
        }

        #endregion

        #region DELETE

        public bool DeleteRoom(int id)
        {
            var room = _roomRepository.Get(id);

            if (room == null)
                return false;

            if (_roomRepository.IsRoomInUse(room))
                return false;

            room.MarkAsDeleted();
            _roomRepository.Update(room);
            return true;
        }

        public bool DeleteUser(int id)
        {
            var user = _userRepository.Get(id);

            if (user == null)
                return false;

            user.MarkAsDeleted();
            _userRepository.Update(user);
            return true;
        }

        public bool DeleteBread(int id)
        {
            var bread = _breadRepository.Get(id);

            if (bread == null)
                return false;

            bread.MarkAsDeleted();
            _breadRepository.Update(bread);
            return true;
        }

        #endregion

        public IEnumerable<string> GetRoleNamesForUser(string contextUserName)
        {
            return _userRepository.GetUserRoleNamesForUserName(contextUserName);
        }

        public IList<User> GetAllUsers()
        {
            return _userRepository.GetAvailableUsers(DateTime.Now).Select(x => new User(x)).ToList();
        }
    }
}
