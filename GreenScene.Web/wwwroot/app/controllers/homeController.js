(function () {
   angular.module('app')
      .controller('homeController', ['$scope', 'scenes', homeController]);

   function homeController($scope, scenes) {
      var vm = $scope;

      vm.scenes = scenes;
   }
})()
