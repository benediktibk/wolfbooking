var Roles = angular.module('Roles', []);
Roles.factory('Roles', function ($http, Authentication) {
    var RolesFactory = {};

    var getAllRoles = function () {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/Roles/all',
            headers: Authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest.then(function (data) {
            return data.data;
        });
    }

    var getRolesDictionary = function () {
        var allRoles = getAllRoles();
        return allRoles.then(function (allRoles) {
            var RolesDictionary = [];

            for (var i = 0; i < allRoles.length; ++i) {
                var currentRole = allRoles[i];
                RolesDictionary[currentRole.Id] = currentRole.Name;
            }

            return RolesDictionary;
        });
    }

    var getRolesDictionaryInvers = function () {
        var allRoles = getAllRoles();
        return allRoles.then(function (allRoles) {
            var RolesDictionary = {};

            for (var i = 0; i < allRoles.length; ++i) {
                var currentRole = allRoles[i];
                RolesDictionary[currentRole.Name] = currentRole.Id;
            }

            return RolesDictionary;
        });
    }

    RolesFactory.getRolesDictionary = getRolesDictionary;
    RolesFactory.getRolesDictionaryInvers = getRolesDictionaryInvers;

    return RolesFactory;
});
