var authentication = angular.module('authentication', []);
authentication.factory('authentication', function ($http) {
    var authenticationFactory = {};

    var _isAuthenticated = false;
    var _username = '';
    var _token = '';
    var _roles = [];

    var getHttpHeaderWithAuthorization = function () {
        var headers = {};

        if (isAuthenticated())
            headers.Authorization = 'Bearer ' + _token;

        return headers;
    }

    var getRolesForUser = function (username) {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/roles/foruser/' + username,
            headers: getHttpHeaderWithAuthorization()
        });

        return httpRequest.then(function (data) {
            return data.data;
        });
    }

    var login = function (username, password) {
        var data = "grant_type=password&username=" + username + "&password=" + password;
        var httpRequest = $http({
            method: 'POST',
            url: 'token',
            data: data,
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            }
        });

        httpRequest.success(function (response) {
            _token = response.access_token;
            _username = username;
            _isAuthenticated = true;

            getRolesForUser(username).then(function (roles) {
                _roles = roles;
            });
        }).error(function (err, status) {
            logout();
        });

        return httpRequest;
    }

    var logout = function () {
        _isAuthenticated = false;
        _username = '';
        _token = '';
        _roles = [];
    }

    var isAuthenticated = function () {
        return _isAuthenticated;
    }

    var getUsername = function () {
        return _username;
    }

    var isOnlyUser = function () {
        return _roles.length <= 1 && _roles[0] == 'Users';
    }

    var isAdministrator = function () {
        return _roles.indexOf("Administrators") != -1;
    }

    authenticationFactory.login = login;
    authenticationFactory.logout = logout;
    authenticationFactory.getHttpHeaderWithAuthorization = getHttpHeaderWithAuthorization;
    authenticationFactory.isAuthenticated = isAuthenticated;
    authenticationFactory.getUsername = getUsername;
    authenticationFactory.isOnlyUser = isOnlyUser;
    authenticationFactory.isAdministrator = isAdministrator;

    return authenticationFactory;
});