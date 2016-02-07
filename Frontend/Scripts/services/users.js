var users = angular.module('users', []);
users.factory('users', function ($http, authentication) {
    var usersFactory = {};

    var getAll = function () {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/users/all',
            headers: authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest;
    }

    var updateItem = function (user) {
        var httpRequest = $http({
            method: 'PUT',
            url: 'api/users/item/' + user.Id,
            headers: authentication.getHttpHeaderWithAuthorization(),
            data: user
        });

        return httpRequest;
    }

    var createItem = function (user) {
        var httpRequest = $http({
            method: 'POST',
            url: 'api/users',
            headers: authentication.getHttpHeaderWithAuthorization(),
            data: user
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
