$(document).ready(function () {
    if (localStorage.getItem('accessToken') == null) {
        window.location.href = "Login.html";
    }

    $('#btnLogOut').click(function () {
        localStorage.removeItem('accessToken');
        window.location.href = "Login.html";
    });
});

window.fbAsyncInit = function() {
    FB.init({
        appId: '631932424335085',
        cookie: true,
        xfbml: true,
        version: 'v7.0'
    });

FB.AppEvents.logPageView();

};

(function(d, s, id){
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) {return;}
    js = d.createElement(s); js.id = id;
    js.src = "https://connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

function checkLoginState() {
    FB.getLoginStatus(function (response) {
        statusChangeCallback(response);
    });
}

function statusChangeCallback(response) {
    if (response.status === 'connected') {

        var facebookToken = response.authResponse.accessToken;
        console.log(facebookToken);

        $.ajax({
            url: '/api/facebook/getFullName?accesstoken=' + facebookToken,
            method: 'GET',
            data: facebookToken,
            success: function (data) {
                console.log(data);
            },
            error: function (e) {
                console.log("error " + e);
            }
        });

        $.ajax({
            url: '/api/facebook/getFeed?accesstoken=' + facebookToken,
            method: 'GET',
            data: facebookToken,
            success: function (data) {
                console.log(data);
            },
            error: function (e) {
                console.log("error " + e);
            }
        });
    }
}

function logOut() {
    FB.logout(function (response) {
        setElements(false);
    });
}
