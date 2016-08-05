wolfBookingApp.controller('breadBookingsController', function ($scope, $q, $location, breadbookings, authentication, tables, users, breads, rooms) {
    tables.initialize($scope, [
            { name: 'Id', field: 'Id', visible: false },
            { name: 'Name', field: 'Name', enableCellEdit: false, type: 'string', enableCellEditOnFocus: true },
            { name: 'Price', field: 'Price', enableCellEdit: false, type: 'number', enableCellEditOnFocus: false },
            { name: 'Amount', field: 'Amount', enableCellEdit: true, type: 'number', enableCellEditOnFocus: false }
    ]);

    if (!authentication.isAuthenticated()) {
        $location.path('/login');
        return;
    }

    var currentBookingMetaData = {
        Id: -1,
        Room: -1,
        Date: 0
    };

    $scope.availableRooms = [];
    $scope.selectedRoom = null;

    rooms.getAll().then(function (data) {
        $scope.availableRooms = data.data;
    });

    $scope.loadAll = function () {
        var username = authentication.getUsername();
        
        users.getItemByUserName(username).then(function (data) {
            var user = data.data;
            breadbookings.getCurrentByRoom(user.Room).then(function (data) {
                var bookings = data.data.Bookings;
                currentBookingMetaData.Id = data.data.Id;
                currentBookingMetaData.Room = data.data.Room;
                currentBookingMetaData.Date = data.data.Date;
                var ids = [];

                for (var i = 0; i < bookings.length; ++i)
                    ids.push(bookings[i].Bread);

                breads.getByIds(ids).then(function (data) {
                    var breads = data.data;
                    var breadsById = {};

                    for (var i = 0; i < breads.length; ++i)
                        breadsById[breads[i].Id] = breads[i];

                    for (var i = 0; i < bookings.length; ++i) {
                        var bread = breadsById[bookings[i].Bread];
                        bookings[i].Name = bread.Name;
                        bookings[i].Price = bread.Price;
                    }

                    tables.setAllRowsClean($scope, bookings);
                });
            })
        });
    };

    $scope.calculateTableHeight = function () {
        return tables.calculateTableHeight($scope);
    };

    $scope.persistAllChanges = function () {
        var data = $scope.gridOptions.data;
        var bookings = {
            Bookings: [],
            Id: currentBookingMetaData.Id,
            Room: currentBookingMetaData.Room,
            Date: currentBookingMetaData.Date
        };

        for (var i = 0; i < data.length; ++i) {
            var current = data[i];
            bookings.Bookings.push({
                Id: current.Id,
                Bread: current.Bread,
                Amount: current.Amount
            });
        }

        breadbookings.updateItem(bookings).then(function () {
            $scope.loadAll();
        });
    };

    $scope.cancelAllChanges = function () {
        $scope.loadAll();
    };

    $scope.loadAll();
});