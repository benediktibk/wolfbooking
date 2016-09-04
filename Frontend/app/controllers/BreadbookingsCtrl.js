wolfBookingApp.controller('BreadbookingsCtrl', function ($scope, $q, $location, Breadbookings, Authentication, Tables, Users, Breads, Rooms) {
    Tables.initialize($scope, [
            { name: 'Id', field: 'Id', visible: false },
            { name: 'Breadbookings.Name', field: 'Name', enableCellEdit: false, type: 'string', enableCellEditOnFocus: true, headerCellFilter: 'translate' },
            { name: 'Breadbookings.Price', field: 'Price', enableCellEdit: false, type: 'number', enableCellEditOnFocus: false, headerCellFilter: 'translate' },
            { name: 'Breadbookings.Amount', field: 'Amount', enableCellEdit: true, type: 'number', enableCellEditOnFocus: false, headerCellFilter: 'translate' }
    ]);

    if (!Authentication.isAuthenticated()) {
        $location.path('/Login');
        return;
    }

   $scope.currentBookingMetaData = {
        Id: -1,
        Room: -1,
        Date: 0,
        AlreadyOrdered: false
    };

    $scope.availableRooms = [];
    $scope.selectedRoom = null;


    $scope.isOnlyUser = function () {
        return Authentication.isOnlyUser();
    };

    $scope.loadAllForRoom = function (room) {
        Breadbookings.getCurrentByRoom(room).then(function (data) {
            var bookings = data.data.Bookings;
            $scope.currentBookingMetaData.Id = data.data.Id;
            $scope.currentBookingMetaData.Room = data.data.Room;
            $scope.currentBookingMetaData.Date = data.data.Date;
            $scope.currentBookingMetaData.AlreadyOrdered = data.data.AlreadyOrdered;
            var ids = [];

            for (var i = 0; i < bookings.length; ++i)
                ids.push(bookings[i].Bread);

            Breads.getByIds(ids).then(function (data) {
                var Breads = data.data;
                var BreadsById = {};

                for (var i = 0; i < Breads.length; ++i)
                    BreadsById[Breads[i].Id] = Breads[i];

                for (var i = 0; i < bookings.length; ++i) {
                    var bread = BreadsById[bookings[i].Bread];
                    bookings[i].Name = bread.Name;
                    bookings[i].Price = bread.Price;
                }

                Tables.setAllRowsClean($scope, bookings);
            });
        });
    };

    $scope.loadAll = function () {
        if ($scope.isOnlyUser()) {
            var username = Authentication.getUsername();
            Users.getItemByUserName(username).then(function (data) {
                var user = data.data;
                $scope.loadAllForRoom(user.Room);
            });
        }
        else {
            if ($scope.selectedRoom == null)
                return;

            $scope.loadAllForRoom($scope.selectedRoom.Id);
        }
    };

    $scope.calculateTableHeight = function () {
        return Tables.calculateTableHeight($scope);
    };

    $scope.persistAllChanges = function () {
        var data = $scope.gridOptions.data;
        var bookings = {
            Bookings: [],
            Id: $scope.currentBookingMetaData.Id,
            Room: $scope.currentBookingMetaData.Room,
            Date: $scope.currentBookingMetaData.Date
        };

        for (var i = 0; i < data.length; ++i) {
            var current = data[i];
            bookings.Bookings.push({
                Id: current.Id,
                Bread: current.Bread,
                Amount: current.Amount
            });
        }

        Breadbookings.updateItem(bookings).then(function () {
            $scope.loadAll();
        });
    };

    $scope.cancelAllChanges = function () {
        $scope.loadAll();
    };

    $scope.selectedRoomChanged = function (selectedRoom) {
        $scope.loadAll();
    }


    if (!$scope.isOnlyUser()) {
        Rooms.getAll().then(function (data) {
            $scope.availableRooms = data.data;
        });
    }

    $scope.loadAll();
});