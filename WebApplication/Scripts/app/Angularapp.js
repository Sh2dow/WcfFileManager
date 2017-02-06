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
        vm.title = 'fs manager';
        vm.fsitems = fsManager.fsitems;
        vm.get = fsManager.get;

        get('');

        function get(path) {
            var f = fsManager.get(path)
        }
    }

    angular
        .module('app')
        .factory('fsManager', fsManager);

    fsManager.$inject = ['$q', 'fsManagerClient'];

    function fsManager($q, fsManagerClient) {
        var service = {
            fsitems: [],
            get: get
        };

        return service;

        function get(path) {
            console.log(path);
            service.fsitems.length = 0;
            return fsManagerClient.get({ path: path })
            .$promise
            .then(function (result) {
                result.fsitems
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
        return $resource("http://localhost:1786/FileService.svc/GetAllFiles/:path",
                { id: "@path" },
                {
                    'get': { method: 'GET', url: 'http://localhost:1786/FileService.svc/GetAllFiles?path=:path', params: { path: '@path' } },
                    //'query': { method: 'GET' },
                    'save': { method: 'POST', transformRequest: angular.identity, headers: { 'Content-Type': undefined } },
                    'remove': { method: 'DELETE', url: 'api/photo/:fileName', params: { name: '@fileName' } }
                });
    }

    app.run(
        function ($rootScope) {
        });
})(window.angular);