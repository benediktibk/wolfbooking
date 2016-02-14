wolfBookingApp.controller('loginController', function ($scope, $window, authentication, pagehistory) {

    $scope.login = function () {
        var username = $scope.username;
        var password = $scope.password;
        authentication.login(username, password).
            success(function () {
                pagehistory.goToPreviousView();
            }).
            error(function () {
                $window.alert('failed');
            });
    };
});