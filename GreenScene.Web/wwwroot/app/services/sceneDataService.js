(function () {
   angular.module('app')
      .factory('sceneDataService', ['$q', '$http', sceneDataService]);

   function sceneDataService($q, $http) {
      return {
         getAllScenes: getAllScenes,
         getSceneByID: getSceneByID
      };

      function getAllScenes() {
         return $http.get('/api/scenes')
            .then(sendResponseData)
            .catch(sendGetScenesError);
      }

      function sendResponseData(response) {
         return response.data;
      }

      function sendGetScenesError(response) {
         return $q.reject('Error retrieving scene(s).');
      }

      function getSceneByID(id) {
         return $http.get('/api/scenes/' + id)
            .then(sendResponseData)
            .catch(sendGetScenesError);
      }
   }
})()
