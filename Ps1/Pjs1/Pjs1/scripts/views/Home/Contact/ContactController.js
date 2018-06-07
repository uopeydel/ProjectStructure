var app = angular.module("projectStructure", []);
app.controller("ContactController", ['$scope', '$http', function ($scope, $http) {

    $scope.depts = "depts test 1234 ";

    $scope.GetUser = function () {
        $http({
            method: 'GET',
            url: '/api/Values/GetUser'
        }).then(function successCallback(response) {
            console.log("successCallback,", response)
        }, function errorCallback(response) {
            console.log("successCallback,", response)
        });
    }

    $scope.ShowModal = function () {
        console.log("xxx");
        $('#myModal').modal('show');
        console.log("wxxx");
    }

}]);