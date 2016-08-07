wolfBookingApp.controller('accountingController', function ($scope, $q, $location, $window, authentication, tables, rooms, accounting) {
    tables.initialize($scope, [
        { name: 'Room', field: 'Room', type: 'string' },
        { name: 'Bread', field: 'Bread', type: 'string' },
        { name: 'Date', field: 'Date', type: 'string' },
        { name: 'Price [€]', field: 'Price', type: 'number' },
        { name: 'Amount', field: 'Amount', type: 'number' },
        { name: 'Total [€]', field: 'Total', type: 'number' }
    ]);

    if (!authentication.isAuthenticated()) {
        $location.path('/login');
        return;
    }

    $scope.availableRooms = [];
    $scope.startDate = new Date();
    $scope.endDate = new Date();
    $scope.billTotal = 0;
    $scope.billLoaded = false;

    $scope.datePickerOptions = {
        minDate: new Date(),
        showWeeks: true
    };

    $scope.resultData;

    $scope.calculateBill = function () {
        var rooms = [];

        for (var i = 0; i < $scope.availableRooms.length; ++i)
            if ($scope.availableRooms[i].Selected)
                rooms.push($scope.availableRooms[i]);

        if (rooms.length <= 0) {
            $window.alert('Please select at least one room')
            return;
        }

        var requests = [];
        
        for (var i = 0; i < rooms.length; ++i) {
            var request = accounting.calculateBill(rooms[0].Id, $scope.startDate, $scope.endDate);
            requests.push(request);
        }

        $q.all(requests).then(function (data) {
            var allBills = [];

            for (var i = 0; i < data.length; ++i)
                allBills.push(data[i].data);

            var tableEntries = [];
            var total = 0;

            for (var i = 0; i < allBills.length; ++i) {
                var bill = allBills[i];
                total = total + bill.Total;

                for (var j = 0; j < bill.Entries.length; ++j) {
                    var entry = bill.Entries[j];
                    entry.Room = rooms[i].Name;
                    var date = new Date(entry.Date);
                    entry.Date = date.toDateString();
                    tableEntries.push(entry);
                }
            }

            tables.setAllRowsClean($scope, tableEntries);
            $scope.billTotal = total;
            $scope.billLoaded = true;
        });
    }

    $scope.calculateTableHeight = function () {
        return tables.calculateTableHeight($scope);
    };


    if ($scope.isOnlyUser()) 
        return;

    rooms.getAll().then(function (data) {
        var availableRooms = data.data;

        for (var i = 0; i < availableRooms.length; ++i) {
            var room = availableRooms[i];
            room.Selected = true;
            availableRooms[i] = room;
        }

        $scope.availableRooms = availableRooms;
    });
});