(function () {

   var app = angular.module('app', ['ngRoute']);

   app.config(['$routeProvider', '$logProvider', '$locationProvider', function ($routeProvider, $logProvider, $locationProvider) {
      $routeProvider.caseInsensitiveMatch = true;
      $routeProvider
         .when('/', {
            templateUrl: 'homeTemplate.html',//In main html to reduce load times
            controller: 'homeController',
            resolve: {
               scenes: ['sceneDataService', function (sceneDataService) {
                  return sceneDataService.getAllScenes();
               }]
            },
            title: 'Home'
         })
         .when('/scene/:key', {
            templateUrl: '/app/templates/scene.html',
            controller: 'sceneController',
            resolve: {
               scene: ['$route', 'sceneDataService', function ($route, sceneDataService) {
                  return sceneDataService.getSceneByID($route.current.params.key);
               }]
            }
         })
         .when('/fileselect', {
            templateUrl: '/app/templates/fileSelect.html',
            controller: 'fileSelectController',
            title: 'File Select'
         });

      //Remove ! from hash
      //$locationProvider.hashPrefix('');

      $logProvider.debugEnabled(true);

     
   }]);

   app.directive("filePathAutocomplete", function () {
      return {
         restrict: 'A',//Only allow usage as attributes, not tags or classes
         link: function (scope, elem, attr, ctrl) {
            elem.autocomplete({
               source: '/api/directorysearch',
               minLength: 0
            });
            //Trigger the auto-complete drop-down whenever the box is clicked
            //in addition to when text is changed.
            elem.focus(function (event) {
               elem.autocomplete("search");
            });
         }
      }
   });

   app.run(['$rootScope', '$log', function ($rootScope, $log) {

      $rootScope.setTitle = function (title) {
         $rootScope.title = title + ' | Green Scene';
      }

      $rootScope.$on('$routeChangeSuccess', function (event, current, previous) {
         if (current.$$route.title) {
            $rootScope.setTitle(current.$$route.title);
         }
      });
   }]);

})()

