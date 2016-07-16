var rooms = angular.module('rooms', []);
rooms.factory('rooms', function ($http, authentication) {
    var roomsFactory = {};

    var getAll = function () {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/rooms/all',
            headers: authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest;
    }

    var updateItem = function (room) {
        var httpRequest = $http({
            method: 'PUT',
            url: 'api/rooms/item/' + room.Id,
            headers: authentication.getHttpHeaderWithAuthorization(),
            data: room
        });

        return httpRequest;
    }

    var createItem = function (room) {
        var httpRequest = $http({
            method: 'POST',
            url: 'api/rooms',
            headers: authentication.getHttpHeaderWithAuthorization(),
            data: room
        });

        return httpRequest;
    }

    var deleteItem = function (room) {
        var httpRequest = $http({
            method: 'DELETE',
            url: 'api/rooms/item/' + room.Id,
            headers: authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest;
    }

    var isInUse = function (room) {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/rooms/inuse/' + room.Id,
            headers: authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest;
    }

    roomsFactory.getAll = getAll;
    roomsFactory.updateItem = updateItem;
    roomsFactory.createItem = createItem;
    roomsFactory.deleteItem = deleteItem;
    roomsFactory.isInUse = isInUse;

    return roomsFactory;
});
