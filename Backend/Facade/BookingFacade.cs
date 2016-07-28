using Backend.Persistence;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Backend.Facade
{
    public class BookingFacade
    {
        #region repositories

        private readonly BreadRepository _breadRepository;
        private readonly UserRepository _userRepository;
        private readonly RoleRepository _roleRepository;
        private readonly RoomRepository _roomRepository;
        private readonly BreadBookingsRepository _breadBookingsRepository;

        #endregion

        #region constructor

        public BookingFacade(BreadRepository breadRepository, UserRepository userRepository, RoleRepository roleRepository, RoomRepository roomRepository, BreadBookingsRepository breadBookingsRepository)
        {
            _breadRepository = breadRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _roomRepository = roomRepository;
            _breadBookingsRepository = breadBookingsRepository;
        }

        #endregion constructor

        #region GET

        public IList<Room> GetCurrentAvailableRooms()
        {
            return _roomRepository.GetCurrentAvailableRooms().Select(x => new Room(x)).ToList();
        }

        public IList<User> GetCurrentAvailableUsersWithoutPasswords()
        {
            var users = _userRepository.GetCurrentAvailableUsers().Select(x => new User(x)).ToList();

            foreach (var user in users)
                user.Password = "";

            return users;
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

        public Bread GetBread(int id)
        {
            var bread = _breadRepository.Get(id);
            return bread == null ? null : new Bread(bread);
        }

        public List<string> GetRolesForUser(string login)
        {
            var user = _userRepository.GetByLogin(login);

            if (user == null)
                return new List<string>();

            var roles = _roleRepository.GetRolesForUser(user.Id);
            return roles.Select(role => role.Name).ToList();
        }

        public List<Role> GetAllRoles()
        {
            return _roleRepository.GetAllRoles().Select(x => new Role(x)).ToList();
        }

        public bool IsLoginValid(string login, string password)
        {
            var user = _userRepository.GetByLogin(login);

            if (user == null)
                return false;

            return user.Password == password;
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

            var roles = _roleRepository.GetRolesForUser(user.Id);
            var roleNames = roles.Select(x => x.Name).ToList();
            return roleNames.Contains("Administrators") || roleNames.Contains("Managers");
        }

        public BreadBookings GetCurrentBreadBookingsForRoom(int id)
        {
            var breadBookings = _breadBookingsRepository.GetCurrentBreadBookingsForRoom(id);
            return new BreadBookings(breadBookings);
        }

        public List<BreadBookings> GetPreviousBreadBookingsForRoom(int id)
        {
            var breadBookings = _breadBookingsRepository.GetPreviousBreadBookingsForRoom(id);
            return breadBookings.Select(x => new BreadBookings(x)).ToList();
        }

        #endregion

        #region ADD

        public object AddRoom(Room room)
        {
            return _roomRepository.Add(new Business.Room(room));
        }

        public int AddUser(User user)
        {
            return _userRepository.Add(new Business.User(user));
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

            if (businessBreadBookings == null)
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
    }
}
