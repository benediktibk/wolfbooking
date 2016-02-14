wolfBookingApp.controller('homeController', function ($scope, pagehistory, authentication) {
    $scope.logout = function () {
        authentication.logout();
    }

    $scope.isLoggedIn = function () {
        return authentication.isAuthenticated();
    }
});