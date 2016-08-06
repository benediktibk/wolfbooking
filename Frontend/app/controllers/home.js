wolfBookingApp.controller('homeController', function ($scope, pagehistory, authentication) {
    $scope.logout = function () {
        authentication.logout();
    }

    $scope.isLoggedIn = function () {
        return authentication.isAuthenticated();
    }

    $scope.isOnlyUser = function () {
        return authentication.isOnlyUser();
    }

    $scope.isAdministrator = function () {
        return authentication.isAdministrator();
    }
});