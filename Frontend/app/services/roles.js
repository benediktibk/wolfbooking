var roles = angular.module('roles', []);
roles.factory('roles', function ($http, authentication) {
    var rolesFactory = {};

    var getAllRoles = function () {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/roles/all',
            headers: authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest.then(function (data) {
            return data.data;
        });
    }

    var getRolesDictionary = function () {
        var allRoles = getAllRoles();
        return allRoles.then(function (allRoles) {
            var rolesDictionary = [];

            for (var i = 0; i < allRoles.length; ++i) {
                var currentRole = allRoles[i];
                rolesDictionary[currentRole.Id] = currentRole.Name;
            }

            return rolesDictionary;
        });
    }

    var getRolesDictionaryInvers = function () {
        var allRoles = getAllRoles();
        return allRoles.then(function (allRoles) {
            var rolesDictionary = {};

            for (var i = 0; i < allRoles.length; ++i) {
                var currentRole = allRoles[i];
                rolesDictionary[currentRole.Name] = currentRole.Id;
            }

            return rolesDictionary;
        });
    }

    rolesFactory.getRolesDictionary = getRolesDictionary;
    rolesFactory.getRolesDictionaryInvers = getRolesDictionaryInvers;

    return rolesFactory;
});
