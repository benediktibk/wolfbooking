var authentication = angular.module('authentication', []);
authentication.factory('authentication', function ($http) {
    var authenticationFactory = {};

    var _isAuthenticated = false;
    var _username = '';
    var _token = '';

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
        }).error(function (err, status) {
            logout();
        });

        return httpRequest;
    }

    var logout = function ()
    {
        _isAuthenticated = false;
        _username = '';
        _token = '';
    }

    authenticationFactory.login = login;
    authenticationFactory.logout = logout;

    return authenticationFactory;
});