wolfBookingApp.controller('breadsController', function ($scope, $q, $location, breads, authentication) {
    $scope.deleted = [];

    $scope.gridOptions = {
        data: [],
        enableHorizontalScrollbar: 0,
        enableVerticalScrollbar: 0,
        rowEditWaitInterval: -1,
        columnDefs: [
            { name: 'Id', field: 'Id', visible: false },
            { name: ' ', enableCellEdit: false, cellTemplate: '<div id="breadsDeleteButton"><i class="fa fa-times fa-lg" ng-click="grid.appScope.deleteBread(row)"></i></div>', width: 30 },
            { name: 'Name', field: 'Name', enableCellEdit: true, type: 'string', enableCellEditOnFocus: true },
            { name: 'Price', field: 'Price', enableCellEdit: true, type: 'number', enableCellEditOnFocus: true }
        ],
        enableColumnMenus: false
    };

    if (!authentication.isAuthenticated()) {
        $location.path('/login');
        return;
    }

    $scope.loadAll = function () {
        breads.getAll().then(function (data) {
            $scope.gridOptions.data = data.data;
            var dirtyRows = $scope.gridApi.rowEdit.getDirtyRows($scope.gridApi.grid);
            var dataDirtyRows = dirtyRows.map(function (gridRow) {
                return gridRow.entity;
            });
            $scope.gridApi.rowEdit.setRowsClean(dataDirtyRows);
        })
    };

    $scope.calculateTableHeight = function () {
        var rowHeight = 30;
        var headerRowHeight = 33;
        return {
            height: ($scope.gridOptions.data.length * rowHeight + headerRowHeight) + "px"
        };
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
        var dirtyRows = $scope.gridApi.rowEdit.getDirtyRows($scope.gridApi.grid);
        var dataDirtyRows = dirtyRows.map(function (gridRow) {
            return gridRow.entity;
        });

        var promises = [];
        var i;
        for (i = 0; i < dataDirtyRows.length; ++i) {
            var row = dataDirtyRows[i];
            if (row.Id == 0)
                promises.push($scope.persistCreate(row));
            else
                promises.push($scope.persistUpdate(row));
        }

        for (i = 0; i < $scope.deleted.length; ++i)
            promises.push($scope.persistDelete($scope.deleted[i]));

        $q.all(promises).then(function () {
            $scope.loadAll();
            $scope.deleted = [];
        });
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