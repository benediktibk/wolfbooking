var users = angular.module('users', []);
users.factory('users', function ($http, authentication) {
    var usersFactory = {};

    var addRoleVariablesToData = function (data) {
        for (var i = 0; i < data.data.length; ++i) {
            var user = data.data[i];
            var roles = user.Roles;
            user.isAdministrator = false;
            user.isManager = false;
            user.isUser = false;

            for (var j = 0; j < roles.length; ++j) {
                switch (roles[j]) {
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
    }

    var addRolesToUser = function (user) {
        user.Roles = [];

        if (user.isUser)
            user.Roles.push('Users');

        if (user.isManager)
            user.Roles.push('Managers');

        if (user.isAdministrator)
            user.Roles.push('Administrators');

        return user;
    }

    var getAll = function () {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/users/all',
            headers: authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest.then(function (data) {
            return addRoleVariablesToData(data);
        });
    }

    var updateItem = function (user) {
        user.Roles = [];
        var httpRequest = $http({
            method: 'PUT',
            url: 'api/users/item/' + user.Id,
            headers: authentication.getHttpHeaderWithAuthorization(),
            data: addRolesToUser(user)
        });

        return httpRequest;
    }

    var createItem = function (user) {
        var httpRequest = $http({
            method: 'POST',
            url: 'api/users',
            headers: authentication.getHttpHeaderWithAuthorization(),
            data: addRolesToUser(user)
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
