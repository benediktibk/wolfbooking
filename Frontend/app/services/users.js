var Users = angular.module('Users', []);
Users.factory('Users', function ($http, Authentication, Roles, Rooms) {
    var UsersFactory = {};

    var addRoleVariablesToUser = function (user, RolesDictonary) {
        var roles = user.Roles;
        user.isAdministrator = false;
        user.isManager = false;
        user.isUser = false;

        for (var j = 0; j < roles.length; ++j) {
            var role = roles[j];
            switch (role.Name) {
                case 'Admin':
                    user.isAdministrator = true;
                    break;
                case 'Manager':
                    user.isManager = true;
                    break;
                case 'User':
                    user.isUser = true;
                    break;
            }
        }

        return user;
    }

    var addRoleVariablesToData = function (data) {
        var RolesDictonaryRequest = Roles.getRolesDictionary();

        return RolesDictonaryRequest.then(function (RolesDictonary) {
            for (var i = 0; i < data.data.length; ++i) {
                var user = data.data[i];
                user = addRoleVariablesToUser(user, RolesDictonary);
            }

            return data;
        });
    }

    var addRolesToUser = function (user) {
        var RolesDictonaryRequest = Roles.getRolesDictionaryInvers();

        return RolesDictonaryRequest.then(function (RolesDictionary) {
            user.Roles = [];

            if (user.isUser) {
                var roleId = RolesDictionary["User"];
                user.Roles.push(roleId);
            }

            if (user.isManager) {
                var roleId = RolesDictionary["Manager"];
                user.Roles.push(roleId);
            }

            if (user.isAdministrator) {
                var roleId = RolesDictionary["Administrator"];
                user.Roles.push(roleId);
            }

            return user;
        })
    }

    var addParentUserToRooms = function (Rooms, user) {
        var RoomsWithParentUsers = [];

        for (var i = 0; i < Rooms.length; ++i) {
            var oldRoom = Rooms[i];
            var newRoom = {
                Id: oldRoom.Id,
                Name: oldRoom.Name,
                ParentUser: user
            };
            RoomsWithParentUsers.splice(i, 0, newRoom);
        }

        return RoomsWithParentUsers;
    }

    var addAvailableRoomsToData = function (data) {
        var RoomsRequest = Rooms.getAll();

        return RoomsRequest.then(function (availableRooms) {
            availableRooms.data.splice(0, 0, { Id: -1, Name: 'None' });
            for (var i = 0; i < data.data.length; ++i) {
                var user = data.data[i];
                user.availableRooms = addParentUserToRooms(availableRooms.data, user);
            }

            return data;
        });
    }

    var addSelectedRoomToUser = function (user) {
        var roomFound = false;

        for (var j = 0; j < user.availableRooms.length && !roomFound; ++j) {
            var currentAvailableRoom = user.availableRooms[j];

            if (user.Room == currentAvailableRoom.Id) {
                user.selectedRoom = currentAvailableRoom;
                roomFound = true;
            }
        }

        return user;
    }

    var addSelectedRoomToData = function (data) {        
        for (var i = 0; i < data.data.length; ++i) {
            var user = data.data[i];
            user = addSelectedRoomToUser(user);
        }

        return data;
    }

    var setIdOfSelectedRoom = function (user) {
        user.Room = user.selectedRoom.Id;
        return user;
    }

    var fillNewUserWithAvailableRooms = function (user) {
        var RoomsRequest = Rooms.getAll();

        return RoomsRequest.then(function (availableRooms) {
            availableRooms.data.splice(0, 0, { Id: -1, Name: 'None' });
            user.availableRooms = addParentUserToRooms(availableRooms.data, user);
            return addSelectedRoomToUser(user);
        });
    }

    var getAll = function () {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/Users/all',
            headers: Authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest.then(function (data) {
            return addRoleVariablesToData(data).then(function (data) {
                 return addAvailableRoomsToData(data).then(function (data) {
                    return addSelectedRoomToData(data);
                });
            });
        });
    }

    var getItemByUserName = function (username) {
        var httpRequest = $http({
            metho: 'GET',
            url: 'api/Users/username/' + username,
            headers: Authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest;
    }

    var updateItem = function (user) {
        return addRolesToUser(user).then(function (user) {
            var userWithRoomSet = setIdOfSelectedRoom(user);
            delete userWithRoomSet.selectedRoom;
            delete userWithRoomSet.availableRooms;
            var httpRequest = $http({
                method: 'PUT',
                url: 'api/Users/item/' + user.Id,
                headers: Authentication.getHttpHeaderWithAuthorization(),
                data: userWithRoomSet
            });

            return httpRequest;
        });
    }

    var createItem = function(user) {
        return addRolesToUser(user)
            .then(function(user) {
                var userWithRoomSet = setIdOfSelectedRoom(user);
                delete userWithRoomSet.selectedRoom;
                delete userWithRoomSet.availableRooms;
                var httpRequest = $http({
                    method: 'POST',
                    url: 'api/Users',
                    headers: Authentication.getHttpHeaderWithAuthorization(),
                    data: userWithRoomSet
                });

                return httpRequest;
            });
    }

    var deleteItem = function (user) {
        var httpRequest = $http({
            method: 'DELETE',
            url: 'api/Users/item/' + user.Id,
            headers: Authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest;
    }

    UsersFactory.getAll = getAll;
    UsersFactory.updateItem = updateItem;
    UsersFactory.createItem = createItem;
    UsersFactory.deleteItem = deleteItem;
    UsersFactory.fillNewUserWithAvailableRooms = fillNewUserWithAvailableRooms;
    UsersFactory.getItemByUserName = getItemByUserName;

    return UsersFactory;
});
