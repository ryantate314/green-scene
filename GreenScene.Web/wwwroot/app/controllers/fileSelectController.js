(function () {
   angular.module('app')
      .controller('fileSelectController', ['$scope', homeController]);

   function homeController($scope) {
      var vm = $scope;

      vm.path = "";
      vm.test = "Hello World";
   }
})()
