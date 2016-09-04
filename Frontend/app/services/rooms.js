var Rooms = angular.module('Rooms', []);
Rooms.factory('Rooms', function ($http, Authentication) {
    var RoomsFactory = {};

    var getAll = function () {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/Rooms/all',
            headers: Authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest;
    }

    var updateItem = function (room) {
        var httpRequest = $http({
            method: 'PUT',
            url: 'api/Rooms/item/' + room.Id,
            headers: Authentication.getHttpHeaderWithAuthorization(),
            data: room
        });

        return httpRequest;
    }

    var createItem = function (room) {
        var httpRequest = $http({
            method: 'POST',
            url: 'api/Rooms',
            headers: Authentication.getHttpHeaderWithAuthorization(),
            data: room
        });

        return httpRequest;
    }

    var deleteItem = function (room) {
        var httpRequest = $http({
            method: 'DELETE',
            url: 'api/Rooms/item/' + room.Id,
            headers: Authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest;
    }

    var isInUse = function (room) {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/Rooms/inuse/' + room.Id,
            headers: Authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest;
    }

    RoomsFactory.getAll = getAll;
    RoomsFactory.updateItem = updateItem;
    RoomsFactory.createItem = createItem;
    RoomsFactory.deleteItem = deleteItem;
    RoomsFactory.isInUse = isInUse;

    return RoomsFactory;
});
