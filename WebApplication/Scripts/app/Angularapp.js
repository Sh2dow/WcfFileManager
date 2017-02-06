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

        activate();

        function activate() {
            fsManager.load();
        }

        function remove(fsitem) {
            photoManager.remove(fsitem).then(function () {
                fsManager.load();
            });
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

        function edit(fsitem) {
            return fsManagerClient.edit({ fileName: fsitem.name })
                                        .$promise
                                        .then(function (result) {
                                            //if the fsitem was edited
                                            var i = service.fsitems.indexOf(fsitem);
                                            service.fsitems.splice(i, 1);

                                            appInfo.setInfo({ message: "filename edit" });

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
            return fsManagerClient.remove({ path: fsitem })
                                        .$promise
                                        .then(function (result) {
                                            //if the fsitem was deleted successfully remove it from the fsitems array
                                            var i = service.fsitems.indexOf(fsitem);
                                            service.fsitems.splice(i, 1);


                                            return result.$promise;
                                        },
                                        function (result) {
                                            return $q.reject(result);
                                        })
                                        ['finally'](
                                        function () {
                                            console.log('file ' + fsitem + ' was deleted.');
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
                    'edit': { method: 'POST', url: 'http://localhost:1786/FileService.svc/EditFile/:fileName', params: { name: '@path', newname: '@newname' } },
                    'remove': { method: 'GET', url: 'http://localhost:1786/FileService.svc/DeleteFile/:fileName', params: { path: '@path' } },

                });
    }

    app.run(
        function ($rootScope) {
        });
})(window.angular);