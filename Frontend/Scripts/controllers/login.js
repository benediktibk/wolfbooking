wolfBookingApp.controller('loginController', function ($scope, $http) {

    $scope.login = function () {
        var username = $scope.username;
        var password = $scope.password;
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
            $window.alert('login succeeded, token ' + response.access_token);
        }).error(function (err, status) {
            $window.alert('login failed');
        });
    };
});