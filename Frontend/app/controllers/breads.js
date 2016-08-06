wolfBookingApp.controller('breadsController', function ($scope, $q, $location, breads, authentication, tables) {
    tables.initialize($scope, [
            { name: 'Id', field: 'Id', visible: false },
            { name: ' ', enableCellEdit: false, cellTemplate: '<div id="breadsDeleteButton"><i class="fa fa-times fa-lg" ng-click="grid.appScope.deleteBread(row)"></i></div>', width: 30 },
            { name: 'Name', field: 'Name', enableCellEdit: true, type: 'string', enableCellEditOnFocus: true },
            { name: 'Price [€]', field: 'Price', enableCellEdit: true, type: 'number', enableCellEditOnFocus: true }
    ]);

    if (!authentication.isAuthenticated()) {
        $location.path('/login');
        return;
    }

    $scope.loadAll = function () {
        breads.getAll().then(function (data) {
            tables.setAllRowsClean($scope, data.data);
        })
    };

    $scope.calculateTableHeight = function () {
        return tables.calculateTableHeight($scope);
    };

    $scope.persistUpdate = function (rowEntity) {
        return breads.updateItem(rowEntity);
    };

    $scope.persistCreate = function (rowEntity) {
        return breads.createItem(rowEntity);
    };

    $scope.persistDelete = function (rowEntity) {
        return breads.deleteItem(rowEntity);
    }

    $scope.persistAllChanges = function () {
        tables.persistAllChanges($scope);
    };

    $scope.cancelAllChanges = function () {
        $scope.loadAll();
    }

    $scope.addBread = function () {
        tables.addRow($scope, {
            Id: 0,
            Name: '',
            Price: ''
        });
    };

    $scope.deleteBread = function (row) {
        tables.deleteRow($scope, row);
    };

    $scope.loadAll();
});