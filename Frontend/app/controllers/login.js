wolfBookingApp.controller('loginController', function ($scope, $window, $location, authentication, pagehistory) {

    $scope.login = function () {
        var username = $scope.username;
        var password = $scope.password;
        authentication.login(username, password).
            success(function () {
                if (pagehistory.doesPreviousViewExist())
                    pagehistory.goToPreviousView();
                else
                    $location.path('/breadbookings');
            }).
            error(function () {
                $window.alert('failed');
            });
    };
});