// Read https://docs.angularjs.org/api/ng/service/$http#interceptors
app.factory('authService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

    //var serviceBase = 'https://www.url.co.th/';
    var serviceBase = 'localhost:/';
    var authServiceFactory = {};

    var _authentication = {
        isAuth: false,
        userName: "",
        useRefreshTokens: true
    };

    var _saveRegistration = function (registration) {

        _logOut();

        return $http.post(serviceBase + 'api/account/register', registration).then(function (response) {
            return response;
        });

    };

    var _login = function (loginData, params) {
        _logOut();
        var deferred = $q.defer();
        var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;
        var data1 = null;
        var data2 = null;
        if (params != undefined) {
            //ถ้ามาจากที่อื่น
            //-ลอคอินคร้ังแรกส่งของคนอื่นไปเอาค่า
            //-ลอคอินครั้งที่สองเก็บเข้าสโตเรจ
            data1 = data + "&client_id=" + params.client_id;
            data2 = data + "&client_id=" + "3ae54600cd964454b4caf6f91ffe0c48";
            $http.post(serviceBase + 'oauth2/token', data1, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response1) {
                $http.post(serviceBase + 'oauth2/token', data2, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response2) {
                    if (loginData.useRefreshTokens) {
                        localStorageService.set('authorizationData', { token: response2.access_token, userName: loginData.userName, refreshToken: response2.refresh_token, useRefreshTokens: true });
                    }
                    else {
                        localStorageService.set('authorizationData', { token: response2.access_token, userName: loginData.userName, refreshToken: "", useRefreshTokens: false });
                    }
                    _authentication.isAuth = true;
                    _authentication.userName = loginData.userName;
                    _authentication.useRefreshTokens = loginData.useRefreshTokens;
                    deferred.resolve(response1);
                }).error(function (err2, status2) {
                    _logOut();
                    deferred.reject(err2);
                });
            }).error(function (err1, status1) {
                _logOut();
                deferred.reject(err1);
            });
        } else {
            //ถ้ามาจากตัวเอง
            //-ลอคอินครั้งเดียวเก็บเข้าสโตเรจ
            data = data + "&client_id=" + "3ae54600cd964454b4caf6f91ffe0c48";
            $http.post(serviceBase + 'oauth2/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {
                //console.log(response)
                if (loginData.useRefreshTokens) {
                    localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName, refreshToken: response.refresh_token, useRefreshTokens: true });
                }
                else {
                    localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName, refreshToken: "", useRefreshTokens: false });
                }
                _authentication.isAuth = true;
                _authentication.userName = loginData.userName;
                _authentication.useRefreshTokens = loginData.useRefreshTokens;
                deferred.resolve(response);

            }).error(function (err, status) {
                _logOut();
                deferred.reject(err);
            });
        }
        return deferred.promise;
    };

    var _logOut = function () {

        localStorageService.remove('authorizationData');

        _authentication.isAuth = false;
        _authentication.userName = "";
        _authentication.useRefreshTokens = true;


    };

    var _fillAuthData = function () {

        var authData = localStorageService.get('authorizationData');
        if (authData) {
            _authentication.isAuth = true;
            _authentication.userName = authData.userName;
            _authentication.useRefreshTokens = authData.useRefreshTokens;
        }

    }
    var _refreshToken = function () {
        var deferred = $q.defer();

        var authData = localStorageService.get('authorizationData');

        if (authData) {

            if (authData.useRefreshTokens) {

                var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + ngAuthSettings.clientId;

                localStorageService.remove('authorizationData');

                $http.post(serviceBase + 'oauth2/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

                    localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: response.refresh_token, useRefreshTokens: true });

                    deferred.resolve(response);

                }).error(function (err, status) {
                    _logOut();
                    deferred.reject(err);
                });
            }
        }

        return deferred.promise;
    };


    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;
    authServiceFactory.refreshToken = _refreshToken;

    return authServiceFactory;
}]);
app.factory('ordersService', ['$http', function ($http) {

    var serviceBase = 'https://lkresserver.on.lk/';
    var ordersServiceFactory = {};

    var _getOrders = function () {

        return $http.get(serviceBase + 'api/protected').then(function (results) {
            return results;
        });
    };

    ordersServiceFactory.getOrders = _getOrders;

    return ordersServiceFactory;

}]);
app.factory('authInterceptorService', ['$q', '$location', 'localStorageService', '$injector', function ($q, $location, localStorageService, $injector) {
    var authInterceptorServiceFactory = {};
    var _request = function (config) {
        config.headers = config.headers || {};
        var authData = localStorageService.get('authorizationData');
        if (authData) {
            //console.log(config.url.split('/'))
            if (config.url.split('/')[2] === "lkauthserver.on.lk") {
            } else {
                config.headers.Authorization = 'Bearer ' + authData.token;
            }
        }
        return config;
    }
    var _responseError = function (rejection) {
        var authData = localStorageService.get('authorizationData');
        if (rejection.status === 401) {
            if (authData !== null) {
                _requestAccessToken({
                    refresh_token: authData.refreshToken,
                    grant_type: 'refresh_token',
                    client_id: "3ae54600cd964454b4caf6f91ffe0c48"
                }).then(function (res) {
                    location.reload(true);
                });
            }
        }
        return $q.reject(rejection);
    }
    var _requestAccessToken = function (obj) {
        var authData = localStorageService.get('authorizationData');
        var deferred = $q.defer();
        var $http = $injector.get('$http');
        $http({
            method: 'POST',
            url: 'https://lkauthserver.on.lk/oauth2/token',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            data: obj,
            transformRequest: function (obj2) {
                var str = [];
                for (var p in obj2)
                    str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj2[p]));
                return str.join("&");
            }
        }).success(function (response) {
            localStorageService.set('authorizationData', {
                token: response.access_token,
                userName: authData.userName,
                refreshToken: response.refresh_token,
                useRefreshTokens: true
            });
            deferred.resolve(response)
        }).error(function (err) {
            deferred.reject(err)
        });
        return deferred.promise;
    }
    authInterceptorServiceFactory.request = _request;
    authInterceptorServiceFactory.responseError = _responseError;
    authInterceptorServiceFactory.requestAccessToken = _requestAccessToken;
    return authInterceptorServiceFactory;
}]);