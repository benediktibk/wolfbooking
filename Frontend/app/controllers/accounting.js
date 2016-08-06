wolfBookingApp.controller('accountingController', function ($scope, $location, authentication, tables, rooms) {
    if (!authentication.isAuthenticated()) {
        $location.path('/login');
        return;
    }

    $scope.availableRooms = [];

    $scope.loadResult = function () {
        $window.alert($scope.room1);
    }


    if (!$scope.isOnlyUser()) {
        rooms.getAll().then(function (data) {
            var availableRooms = data.data;

            for (var i = 0; i < availableRooms.Length; ++i)
                availableRooms[i].Selected = false;

            $scope.availableRooms = availableRooms;
        });
    }
});