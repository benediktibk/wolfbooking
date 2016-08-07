wolfBookingApp.controller('accountingController', function ($scope, $q, $location, $window, $translate, authentication, tables, rooms, accounting) {
    tables.initialize($scope, [
        { name: 'Accounting.Room', field: 'Room', type: 'string', headerCellFilter: 'translate' },
        { name: 'Accounting.Bread', field: 'Bread', type: 'string', headerCellFilter: 'translate' },
        { name: 'Accounting.Date', field: 'Date', type: 'string', headerCellFilter: 'translate' },
        { name: 'Accounting.Price', field: 'Price', type: 'number', headerCellFilter: 'translate' },
        { name: 'Accounting.Amount', field: 'Amount', type: 'number', headerCellFilter: 'translate' },
        { name: 'Accounting.Totalascolumn', field: 'Total', type: 'number', headerCellFilter: 'translate' }
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
            $translate('Accounting.Selectionerror').then(function (errorMessage) {
                $window.alert(errorMessage);
            });
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