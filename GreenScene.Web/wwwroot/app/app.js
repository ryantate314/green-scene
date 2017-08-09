(function(){
   
    var app = angular.module('app', ['ngRoute']);

    app.config(['$routeProvider', '$logProvider', function($routeProvider, $logProvider){
        $routeProvider.caseInsensitiveMatch = true;
        $routeProvider
            .when('/', {
                templateUrl: 'homeTemplate.html',//In main html to reduce load times
                controller: 'homeController',
                resolve: function()
            })
            .when('/scene/:id', {
                templateUrl: '/app/templates/scene.html',
                controller: 'sceneController'
            });

        $logProvider.debugEnabled(true);
    }]);

    app.run(['$rootScope', '$log', function($rootScope, $log) {
        $rootScope.$on('$routeChangeStart', function(event, current, previous){
            $log.log('route change start');
        });
    }]);

})()

