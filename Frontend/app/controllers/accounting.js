wolfBookingApp.controller('accountingController', function ($scope, $location, $window, authentication, tables, rooms, accounting) {
    if (!authentication.isAuthenticated()) {
        $location.path('/login');
        return;
    }

    $scope.availableRooms = [];
    $scope.startDate = new Date();
    $scope.endDate = new Date();
    $scope.selectedRoom = {
        Id: -1,
        Name: "None"
    };

    $scope.datePickerOptions = {
        minDate: new Date(),
        showWeeks: true
    };

    $scope.calculateBill = function () {
        if ($scope.selectedRoom.Id < 0)
            $window.alert('Please select a room')
        else
            accounting.calculateBill($scope.selectedRoom.Id, $scope.startDate, $scope.endDate).then(function (data) {
                var a = data.data;
                var b = 0;
            });
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