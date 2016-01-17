var wolfBookingApp = angular.module('wolfBooking', ['ngRoute', 'ui.grid', 'ui.grid.autoResize', 'ui.grid.edit', 'ui.grid.rowEdit']);

wolfBookingApp.config(function ($routeProvider, $locationProvider) {
    $routeProvider
        .when('/', {
            templateUrl: 'views/home',
            controller: 'homeController'
        })
        .when('/home', {
            templateUrl: 'views/home',
            controller: 'homeController'
        })
        .when('/breads', {
            templateUrl: 'views/breads',
            controller: 'breadsController'
        });

    $locationProvider.html5Mode(true);
})

wolfBookingApp.controller('homeController', function ($scope) { });
wolfBookingApp.controller('breadsController', function ($scope, $http) {
    $scope.breads = [];

    $scope.loadBreads = function () {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/breads/all'
        }).then(function (data) {
            $scope.breads = data.data;
            $scope.fillBreadsTable();
        })
    };

    $scope.calculateBreadsTableHeight = function () {
        var rowHeight = 30;
        var headerRowHeight = 33;
        return {
            height: ($scope.breads.length * rowHeight + headerRowHeight) + "px"
        };
    };

    $scope.fillBreadsTable = function () {
        $scope.gridOptions.data = $scope.breads
    };

    $scope.saveBreadsRow = function (rowEntity) {
        var httpRequest = $http({
            method: 'POST',
            url: 'api/breads/item/' + rowEntity.Id,
            data: rowEntity
        });
        $scope.gridApi.rowEdit.setSavePromise(rowEntity, httpRequest);
    };

    $scope.saveAllBreads = function () {
        var dirtyRows = $scope.gridApi.rowEdit.getDirtyRows($scope.gridApi.grid);
        var dataDirtyRows = dirtyRows.map(function (gridRow) {
            return gridRow.entity;
        });
        var i;
        for (i = 0; i < dataDirtyRows.length; ++i)
            $scope.saveBreadsRow(dataDirtyRows[i]);
        $scope.gridApi.rowEdit.setRowsClean(dataDirtyRows);
    };

    $scope.loadBreads();

    $scope.gridOptions = {
        data: $scope.breads,
        enableHorizontalScrollbar: 0,
        enableVerticalScrollbar: 2,
        rowEditWaitInterval: -1,
        columnDefs: [
            { name: 'Id', field: 'Id', visible: false },
            { name: 'Name', field: 'Name', enableCellEdit: true, type: 'string', enableCellEditOnFocus: true },
            { name: 'Price', field: 'Price', enableCellEdit: true, type: 'number', enableCellEditOnFocus: true }
        ]
    };

    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
        gridApi.rowEdit.on.saveRow($scope, $scope.saveBreadsRow);
    };
});