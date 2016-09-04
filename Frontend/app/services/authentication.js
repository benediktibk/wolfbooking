var Authentication = angular.module('Authentication', []);
Authentication.factory('Authentication', function ($http) {
    var AuthenticationFactory = {};

    var _isAuthenticated = false;
    var _username = '';
    var _token = '';
    var _Roles = [];

    var getHttpHeaderWithAuthorization = function () {
        var headers = {};

        if (isAuthenticated())
            headers.Authorization = 'Bearer ' + _token;

        return headers;
    }

    var getRolesForUser = function (username) {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/Roles/foruser/' + username,
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

            getRolesForUser(username).then(function (Roles) {
                _Roles = Roles;
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
        _Roles = [];
    }

    var isAuthenticated = function () {
        return _isAuthenticated;
    }

    var getUsername = function () {
        return _username;
    }

    var isOnlyUser = function () {
        return _Roles.length <= 1 && _Roles[0] == 'Users';
    }

    var isAdministrator = function () {
        return _Roles.indexOf("Administrators") != -1;
    }

    AuthenticationFactory.login = login;
    AuthenticationFactory.logout = logout;
    AuthenticationFactory.getHttpHeaderWithAuthorization = getHttpHeaderWithAuthorization;
    AuthenticationFactory.isAuthenticated = isAuthenticated;
    AuthenticationFactory.getUsername = getUsername;
    AuthenticationFactory.isOnlyUser = isOnlyUser;
    AuthenticationFactory.isAdministrator = isAdministrator;

    return AuthenticationFactory;
});