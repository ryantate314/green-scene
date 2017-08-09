(function(){
    angular.module('app')
        .controller('sceneController', ['$scope', '$routeParams', 'sceneDataService', sceneController]);

    function sceneController($scope, $routeParams, sceneDataService){
        var vm = $scope;
        var sceneId = $routeParams.id;
        
        sceneDataService.getSceneByID(sceneId)
        .then(function(scene){
            vm.scene = scene;
        })
        .catch(function(error){
            console.log(error);
        });

        console.log('ran scene controller');
    }
})()
