(function(){
    angular.module('app')
        .controller('sceneController', ['$scope', '$rootScope', 'scene', sceneController]);

    function sceneController($scope, $rootScope, scene){
        var vm = $scope;
        vm.scene = scene;

        $rootScope.setTitle(scene.title);
    }
})()
