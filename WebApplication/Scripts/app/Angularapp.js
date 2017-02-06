(function (angular) {

    var app = angular.module('app', ['ngResource'])
    app.controller('fsitems', fsitems)
    app.value('path', {
        load: function (path) {
            this.path = path;
        }
    });

    fsitems.$inject = ['fsManager'];

    function fsitems(fsManager) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'FileManager';
        vm.fsitems = fsManager.fsitems;
        vm.load = fsManager.load;
        vm.remove = fsManager.remove;

        //get('');

        activate();

        function activate() {
            fsManager.load();
        }
    }

    function edit(fsitem) {
        appInfo.setInfo({ busy: true, message: "editing fsitem " + fsitem.name });
        return fsitemManagerClient.edit({ fileName: fsitem.name })
                                    .$promise
                                    .then(function (result) {
                                        //if the fsitem was edited
                                        var i = service.fsitems.indexOf(fsitem);
                                        service.fsitems.splice(i, 1);

                                        appInfo.setInfo({ message: "fsitems edit" });

                                        return result.$promise;
                                    },
                                    function (result) {
                                        appInfo.setInfo({ message: "something went wrong: " + result.data.message });
                                        return $q.reject(result);
                                    })
                                    ['finally'](
                                    function () {
                                        appInfo.setInfo({ busy: false });
                                    });
    }

    function remove(fsitem) {
        appInfo.setInfo({ busy: true, message: "deleting fsitem " + fsitem.name });
        return fsitemManagerClient.remove({ fileName: fsitem.name })
                                    .$promise
                                    .then(function (result) {
                                        //if the fsitem was deleted successfully remove it from the fsitems array
                                        var i = service.fsitems.indexOf(fsitem);
                                        service.fsitems.splice(i, 1);

                                        appInfo.setInfo({ message: "fsitems deleted" });

                                        return result.$promise;
                                    },
                                    function (result) {
                                        appInfo.setInfo({ message: "something went wrong: " + result.data.message });
                                        return $q.reject(result);
                                    })
                                    ['finally'](
                                    function () {
                                        appInfo.setInfo({ busy: false });
                                    });
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
            return fsManagerClient.get({ path: path })
            .$promise
            .then(function (result) {
                result
                        .forEach(function (path) {
                            service.fsitems.push(path);
                        });

                return result.$promise;
            });
        }
    }

    angular
        .module('app')
        .factory('fsManagerClient', fsManagerClient);

    fsManagerClient.$inject = ['$resource'];

    function fsManagerClient($resource) {
        return $resource("http://localhost:1786/FileService.svc/GetAllFiles?:path", { path: "@path" },
                {
                    //'get': { method: 'GET', isArray: true },
                    'get': { method: 'GET', url: 'http://localhost:1786/FileService.svc/GetAllFiles/:fileName', isArray: true, params: { path: '@path' } },
                    'save': { method: 'POST', url: 'http://localhost:1786/FileService.svc/Delete/:fileName', transformRequest: angular.identity, headers: { 'Content-Type': undefined }, params: { name: '@fileName' } },
                    'edit': { method: 'PUT', url: 'http://localhost:1786/FileService.svc/PUT/:fileName', params: { name: '@fileName' } },
                    'remove': { method: 'DELETE', url: 'http://localhost:1786/FileService.svc/Delete/:fileName', params: { name: '@fileName' } }
                });
    }

    app.run(
        function ($rootScope) {
        });
})(window.angular);