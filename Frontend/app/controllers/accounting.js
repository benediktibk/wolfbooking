wolfBookingApp.controller('accountingController', function ($scope, $location, $window, authentication, tables, rooms, accounting) {
    if (!authentication.isAuthenticated()) {
        $location.path('/login');
        return;
    }

    $scope.availableRooms = [];
    $scope.startDate = new Date();
    $scope.endDate = new Date();

    $scope.datePickerOptions = {
        minDate: new Date(),
        showWeeks: true
    };

    $scope.calculateBill = function () {
        var rooms = [];

        for (var i = 0; i < $scope.availableRooms.length; ++i)
            if ($scope.availableRooms[i].Selected)
                rooms.push($scope.availableRooms[i]);

        if (rooms.length <= 0)
            $window.alert('Please select at least one room')
        else
            accounting.calculateBill(rooms[0].Id, $scope.startDate, $scope.endDate).then(function (data) {
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