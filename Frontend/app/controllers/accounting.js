wolfBookingApp.controller('accountingController', function ($scope, $location, authentication, tables, rooms) {
    if (!authentication.isAuthenticated()) {
        $location.path('/login');
        return;
    }

    $scope.room1 = false;

    $scope.loadResult = function () {
        $window.alert($scope.room1);
    }
});