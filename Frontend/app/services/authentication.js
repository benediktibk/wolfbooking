var Authentication = angular.module('Authentication', ['ngCookies']);
Authentication.factory('Authentication', function ($http, $cookies) {
    var AuthenticationFactory = {};

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

    var copyLoginValuesToCookies = function () {
        $cookies.put('loginToken', _token);
        $cookies.put('loginUsername', _username);
        $cookies.put('loginRoles', _roles);
        $cookies.put('loginIsAuthenticated', _isAuthenticated);
    }

    var getRolesForUser = function (username) {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/roles/' + username,
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
                _roles = Roles;
                copyLoginValuesToCookies();
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
        copyLoginValuesToCookies();
    }

    var isAuthenticated = function () {
        if (_isAuthenticated)
            return true;

        isAuthenticatedCookie = $cookies.get('loginIsAuthenticated');
        _isAuthenticated = isAuthenticatedCookie == 'true';

        if (!_isAuthenticated)
            return false;

        _token = $cookies.get('loginToken');
        _username = $cookies.get('loginUsername');
        _roles = $cookies.get('loginRoles');

        return true;
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

    AuthenticationFactory.login = login;
    AuthenticationFactory.logout = logout;
    AuthenticationFactory.getHttpHeaderWithAuthorization = getHttpHeaderWithAuthorization;
    AuthenticationFactory.isAuthenticated = isAuthenticated;
    AuthenticationFactory.getUsername = getUsername;
    AuthenticationFactory.isOnlyUser = isOnlyUser;
    AuthenticationFactory.isAdministrator = isAdministrator;

    return AuthenticationFactory;
});