wolfBookingApp.controller('LoginCtrl', function ($scope, $window, $location, Authentication, PageHistory) {

    $scope.login = function () {
        var username = $scope.username;
        var password = $scope.password;
        Authentication.login(username, password).
            success(function () {
                if (PageHistory.doesPreviousViewExist())
                    PageHistory.goToPreviousView();
                else
                    $location.path('/Breadbookings');
            }).
            error(function () {
                $window.alert('failed');
            });
    };
});