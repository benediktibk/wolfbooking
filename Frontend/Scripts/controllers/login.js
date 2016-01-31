wolfBookingApp.controller('loginController', function ($scope, $window, authentication) {

    $scope.login = function () {
        var username = $scope.username;
        var password = $scope.password;
        authentication.login(username, password).
            success(function () { $window.alert('sucess'); }).
            error(function () { $window.alert('failed') });
    };
});