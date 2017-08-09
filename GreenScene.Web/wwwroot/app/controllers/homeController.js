(function(){
    angular.module('app')
        .controller('homeController', ['$scope', 'sceneDataService', homeController]);

    function homeController($scope, sceneDataService){
        var vm = $scope;
        
        sceneDataService.getAllScenes()
        .then(function(scenes){
            console.log(scenes);
            vm.scenes = scenes;
        })
        .catch(function(error){
            console.log('Error getting list of scenes.');
        });
    }
})()
