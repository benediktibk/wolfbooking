var users = angular.module('users', []);
users.factory('users', function ($http, authentication, roles, rooms) {
    var usersFactory = {};

    var addRoleVariablesToData = function (data) {
        var rolesDictonaryRequest = roles.getRolesDictionary();

        return rolesDictonaryRequest.then(function (rolesDictonary) {
            for (var i = 0; i < data.data.length; ++i) {
                var user = data.data[i];
                var roles = user.Roles;
                user.isAdministrator = false;
                user.isManager = false;
                user.isUser = false;

                for (var j = 0; j < roles.length; ++j) {
                    var role = roles[j];
                    var roleTranslated = rolesDictonary[role];
                    switch (roleTranslated) {
                        case 'Administrators':
                            user.isAdministrator = true;
                            break;
                        case 'Managers':
                            user.isManager = true;
                            break;
                        case 'Users':
                            user.isUser = true;
                            break;
                    }
                }
            }

            return data;
        });
    }

    var addRolesToUser = function (user) {
        var rolesDictonaryRequest = roles.getRolesDictionaryInvers();

        return rolesDictonaryRequest.then(function (rolesDictionary) {
            user.Roles = [];

            if (user.isUser) {
                var roleId = rolesDictionary["Users"];
                user.Roles.push(roleId);
            }

            if (user.isManager) {
                var roleId = rolesDictionary["Managers"];
                user.Roles.push(roleId);
            }

            if (user.isAdministrator) {
                var roleId = rolesDictionary["Administrators"];
                user.Roles.push(roleId);
            }

            return user;
        })
    }

    var addAvailableRoomsToData = function (data) {
        var roomsRequest = rooms.getAll();

        return roomsRequest.then(function (availableRooms) {
            for (var i = 0; i < data.data.length; ++i) {
                var user = data.data[i];
                user.availableRooms = availableRooms.data;
            }

            return data;
        });
    }

    var addSelectedRoomToData = function (data) {        
        for (var i = 0; i < data.data.length; ++i) {
            var user = data.data[i];

            var roomFound = false;

            for (var j = 0; j < user.availableRooms.length && !roomFound; ++j) {
                var currentAvailableRoom = user.availableRooms[j];

                if (user.Room == currentAvailableRoom.Id) {
                    user.selectedRoom = currentAvailableRoom;
                    roomFound = true;
                }
            }
        }

        return data;
    }

    var setIdOfSelectedRoom = function (user) {
        user.Room = user.selectedRoom.Id;
        return user;
    }

    var getAll = function () {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/users/all',
            headers: authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest.then(function (data) {
            return addRoleVariablesToData(data).then(function (data) {
                 return addAvailableRoomsToData(data).then(function (data) {
                    return addSelectedRoomToData(data);
                });
            });
        });
    }

    var updateItem = function (user) {
        return addRolesToUser(user).then(function (user) {
            var userWithRoomSet = setIdOfSelectedRoom(user);
            var httpRequest = $http({
                method: 'PUT',
                url: 'api/users/item/' + user.Id,
                headers: authentication.getHttpHeaderWithAuthorization(),
                data: userWithRoomSet
            });

            return httpRequest;
        });
    }

    var createItem = function (user) {
        return addRolesToUser(user).then(function (user) {
            var httpRequest = $http({
                method: 'POST',
                url: 'api/users',
                headers: authentication.getHttpHeaderWithAuthorization(),
                data: user
            });

            return httpRequest;
        });

        return httpRequest;
    }

    var deleteItem = function (user) {
        var httpRequest = $http({
            method: 'DELETE',
            url: 'api/users/item/' + user.Id,
            headers: authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest;
    }

    usersFactory.getAll = getAll;
    usersFactory.updateItem = updateItem;
    usersFactory.createItem = createItem;
    usersFactory.deleteItem = deleteItem;

    return usersFactory;
});
