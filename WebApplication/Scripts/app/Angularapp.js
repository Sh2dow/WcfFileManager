var app = angular.module('app', ['ngResource'])

app.controller('fsitems', fsitems)

app.value('path', {
    load: function (path) {
        this.path = path;
    }
});

fsitems.$inject = ['fsManager'];

angular
    .module('app');

function fsitems(fsManager) {
    /* jshint validthis:true */
    var vm = this;
    vm.title = 'FileManager';
    vm.fsitems = fsManager.fsitems;
    vm.load = fsManager.load;
    vm.edit = fsManager.edit;
    vm.remove = fsManager.remove;

    activate();

    function activate() {
        console.log('loading...');
        fsManager.load();
    }
}

angular
    .module('app')
    .factory('fsManager', fsManager);

fsManager.$inject = ['$q', 'fsManagerClient'];

function fsManager($q, fsManagerClient) {
    var service = {
        fsitems: [],
        load: load,
        edit: edit,
        remove: remove
    };

    return service;

    function load(path) {
        console.log(path);
        service.fsitems.length = 0;
        return fsManagerClient.get({ path })
        .$promise
        .then(function (result) {
            result
                    .forEach(function (path) {
                        service.fsitems.push(path);
                    });

            return result.$promise;
        });
    }

    function edit(path, name) {
        return fsManagerClient.edit({path, name})
                                    .$promise
                                    .then(function (result) {
                                        //if the fsitem was edited

                                        var i = service.fsitems.indexOf(path);
                                        service.fsitems.splice(i, 1);

                                        return result.$promise;
                                    },
                                    function (result) {
                                        return $q.reject(result);
                                    })
                                    ['finally'](
                                    function () {
                                        console.log('file ' + name + ' was renamed.');
                                    });
    }


    function remove(path) {
        return fsManagerClient.remove({ path })
                                    .$promise
                                    .then(function (result) {
                                        //if the fsitem was deleted successfully remove it from the fsitems array
                                        var i = service.fsitems.indexOf(path);
                                        service.fsitems.splice(i, 1);

                                        return result.$promise;
                                    },
                                    function (result) {
                                        return $q.reject(result);
                                    })
                                    ['finally'](
                                    function () {
                                        console.log('file ' + path + ' was deleted.');
                                    });
    }
}

angular
    .module('app')
    .factory('fsManagerClient', fsManagerClient);

fsManagerClient.$inject = ['$resource'];

function fsManagerClient($resource) {
    return $resource("http://localhost:1786/FileService.svc/:path", { path: "@path" },
            {
                //'get': { method: 'GET', isArray: true },
                'get': { method: 'GET', url: 'http://localhost:1786/FileService.svc/GetAllFiles/:fileName', isArray: true, params: { path: '@path' } },
                'save': { method: 'POST', url: 'http://localhost:1786/FileService.svc/AddFile/:fileName', transformRequest: angular.identity, headers: { 'Content-Type': undefined }, params: { name: '@fileName' } },
                'edit': { method: 'GET', url: 'http://localhost:1786/FileService.svc/EditFile/:fileName', params: { path: '@path', name: '@name' } },
                'remove': { method: 'GET', url: 'http://localhost:1786/FileService.svc/DeleteFile/:fileName', params: { path: '@path' } },

            });
}


angular
        .module('app.b', ['app'])
        .controller('AppKeysCtrl', AppKeysCtrl);

function AppKeysCtrl($scope, $http, $location) {
    $scope.oldField = {};
    $scope.newField = {};
    $scope.editing = false;

    $scope.appkeys = [];

    $scope.editAppKey = function (field) {
        $scope.editing = $scope.appkeys.indexOf(field);
        $scope.oldField = field;
        $scope.newField = angular.copy(field);
        console.log('editAppKey');
    }

    $scope.saveField = function (index) {
        if ($scope.editing !== false) {
            $scope.appkeys[$scope.editing] = $scope.newField;
            $scope.editing = false;
        }
        console.log('saveField');
    };

    $scope.cancel = function (index) {
        if ($scope.editing !== false) {
            $scope.appkeys[$scope.editing] = $scope.newField;
            $scope.editing = false;
        }
        console.log(index + ' cancel');
    };
}

//app.run(function ($rootScope) {
//});

angular.element(document).ready(function () {
    //angular.bootstrap(document, ["app"]);
});