wolfBookingApp.controller('breadsController', function ($scope, $q, $location, breads, authentication, tables) {
    tables.initialize($scope, [
            { name: 'Id', field: 'Id', visible: false },
            { name: ' ', enableCellEdit: false, cellTemplate: '<div id="breadsDeleteButton"><i class="fa fa-times fa-lg" ng-click="grid.appScope.deleteBread(row)"></i></div>', width: 30 },
            { name: 'Name', field: 'Name', enableCellEdit: true, type: 'string', enableCellEditOnFocus: true },
            { name: 'Price', field: 'Price', enableCellEdit: true, type: 'number', enableCellEditOnFocus: true }
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
        var httpRequest = breads.updateItem(rowEntity);
        $scope.gridApi.rowEdit.setSavePromise(rowEntity, httpRequest);
        return httpRequest;
    };

    $scope.persistCreate = function (rowEntity) {
        var httpRequest = breads.createItem(rowEntity);
        $scope.gridApi.rowEdit.setSavePromise(rowEntity, httpRequest);
        return httpRequest;
    };

    $scope.persistDelete = function (rowEntity) {
        var httpRequest = breads.deleteItem(rowEntity);
        return httpRequest;
    }

    $scope.persistAllChanges = function () {
        tables.persistAllChanges($scope);
    };

    $scope.cancelAllChanges = function () {
        $scope.loadAll();
    }

    $scope.addBread = function () {
        $scope.gridOptions.data.push({
            Id: 0,
            Name: '',
            Price: ''
        });
    };

    $scope.deleteBread = function (row) {
        var index = $scope.gridOptions.data.indexOf(row.entity);
        $scope.deleted.push(row.entity);
        $scope.gridOptions.data.splice(index, 1);
    };

    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
    };

    $scope.loadAll();
});