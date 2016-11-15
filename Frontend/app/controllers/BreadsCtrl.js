wolfBookingApp.controller('BreadsCtrl', function ($scope, $q, $location, Breads, Authentication, Tables) {
    Tables.initialize($scope, [
            { name: 'Id', field: 'Id', visible: false },
            { name: ' ', enableCellEdit: false, cellTemplate: '<div id="BreadsDeleteButton"><i class="fa fa-trash-o fa-lg" ng-click="grid.appScope.deleteBread(row)"></i></div>', width: 30 },
            { name: 'Breads.Name', field: 'Name', enableCellEdit: true, type: 'string', enableCellEditOnFocus: true, headerCellFilter: 'translate' },
            { name: 'Breads.Price', field: 'Price', enableCellEdit: true, type: 'number', enableCellEditOnFocus: true, headerCellFilter: 'translate' }
    ]);

    if (!Authentication.isAuthenticated()) {
        $location.path('/Login');
        return;
    }

    $scope.loadAll = function () {
        Breads.getAll().then(function (data) {
            Tables.setAllRowsClean($scope, data.data);
        })
    };

    $scope.calculateTableHeight = function () {
        return Tables.calculateTableHeight($scope);
    };

    $scope.persistUpdate = function (rowEntity) {
        return Breads.updateItem(rowEntity);
    };

    $scope.persistCreate = function (rowEntity) {
        return Breads.createItem(rowEntity);
    };

    $scope.persistDelete = function (rowEntity) {
        return Breads.deleteItem(rowEntity);
    }

    $scope.persistAllChanges = function () {
        Tables.persistAllChanges($scope);
    };

    $scope.cancelAllChanges = function () {
        $scope.loadAll();
    }

    $scope.addBread = function () {
        Tables.addRow($scope, {
            Id: 0,
            Name: '',
            Price: ''
        });
    };

    $scope.deleteBread = function (row) {
        Tables.deleteRow($scope, row);
    };

    $scope.loadAll();
});