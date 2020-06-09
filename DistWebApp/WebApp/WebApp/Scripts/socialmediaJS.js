window.fbAsyncInit = function () {
    FB.init({
        appId: '631932424335085',
        cookie: true,
        xfbml: true,
        version: 'v7.0'
    });

    FB.AppEvents.logPageView();

};

(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) { return; }
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
        sessionStorage.setItem("fbToken", facebookToken);
        console.log(facebookToken);
    }
}

function logOut() {
    FB.logout(function (response) {
        setElements(false);
    });
}

$(document).ready(function () {


    $('#linkClose').click(function () {
        $('#divError').hide('fade');
    });

    $('#btnLogin').click(function () {
        $.ajax({
            url: 'https://localhost:44377/token',
            method: 'POST',
            contentType: 'application/json',
            data: {
                username: $('#txtUsername').val(),
                password: $('#txtPassword').val(),
                grant_type: 'password'
            },
            success: function (response) {

                sessionStorage.setItem('accessToken', response.access_token);
                sessionStorage.setItem('username', $('#txtUsername').val());
                window.location.href = "Wall";
                //alert("OK");
            },
            error: function (jqXHR) {
                $('#divErrorText').text(jqXHR.responseText);
                $('#divError').show('fade');
            }
        });
    });

    $(document).ready(function () {
        console.log(sessionStorage.getItem('accessToken'));

        $('#linkClose').click(function () {
            $('#divError').hide('fade');
        });

        $('#btnRegister').click(function () {
            $.ajax({
                url: 'https://localhost:44377/api/Account/Register',
                method: 'POST',
                data: {
                    email: $('#txtEmail').val(),
                    password: $('#txtPassword').val(),
                    confirmPassword: $('#txtConfirmPassword').val()
                },
                success: function () {
                    $('#successModal').modal('show');
                },
                error: function (jqXHR) {
                    $('#divErrorText').text(jqXHR.responseText);
                    $('#divError').show('fade');
                }
            });
        });
    });



    $(document).on("click", "#prefEmail", function () {
        var checkBox = document.getElementById("prefEmail");
        var text = document.getElementById("profileEmail");

        if (checkBox.checked == true) {
            text.style.display = "block";
        } else {
            text.style.display = "none";
        }
    });

    $(document).on("click", "#prefBirthday", function () {
        var checkBox = document.getElementById("prefBirthday");
        var text = document.getElementById("profileBirthday");

        if (checkBox.checked == true) {
            text.style.display = "block";
        } else {
            text.style.display = "none";
        }
    });

    $(document).on("click", "#prefSet", function () {
        var checkBoxBirthday = document.getElementById("prefBirthday");
        var checkBoxEmail = document.getElementById("prefEmail");


        $.ajax({
            url: 'https://localhost:44377/api/facebook/setPref?',
            method: 'POST',
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken'),
            },
            data: {
                Username: sessionStorage.getItem('username'),
                Birthday: checkBoxBirthday.checked,
                Email: checkBoxEmail.checked
            },
            success: function (data) {
                alert("Saved!");
            },
            error: function (e) {
                alert("Error!" + e);
            }
        });


    });


    $(document).on("click", "#btnnn", function () {
        $.ajax({
            url: 'https://localhost:44377/api/facebook/getProfile?accesstoken=' + sessionStorage.getItem("fbToken"),
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken'),
            },
            data: sessionStorage.getItem("fbToken"),
            success: function (data) {
                //console.log(data);
                $("#profileName").append(data.name);
                $("#profileEmail").append(data.email);
                $("#profileBirthday").append(data.birthday);
            },
            error: function (e) {
                console.log("error " + e);
            }
        });

        $.ajax({
            url: 'https://localhost:44377/api/facebook/getUserLikes?accesstoken=' + sessionStorage.getItem("fbToken"),
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken'),
            },
            data: sessionStorage.getItem("fbToken"),
            success: function (data) {
                $.each(data, function (index, likes) {
                    //console.log(likes.name);
                    $("#feed").append('<div class="card bg-light">');
                    $("#likes").append('<div class="card-body">');
                    $("#likes").append('<h5 class="card-title">' + likes.name + '</h5>');
                    $("#likes").append('</div>');
                    $("#likes").append('</div>');
                })
            }
        })

        $.ajax({
            url: 'https://localhost:44377/api/facebook/getFeed?accesstoken=' + sessionStorage.getItem("fbToken"),
            method: 'GET',
            data: sessionStorage.getItem("fbToken"),
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken'),
            },
            dataType: 'json',
            success: function (data) {
                //console.log(data);
                $.each(data, function (index, feed) {
                    //console.log(feed);
                    //console.log(feed.created_time);

                    $("#feed").append('<div class="card bg-light">');
                    $("#feed").append('<div class="card-body">');
                    $("#feed").append('<h5 class="card-title">' + feed.message + '</h5>');
                    $("#feed").append('</div>');
                    $("#feed").append('</div>');
                })
            },
            error: function (e) {
                console.log("error " + e);
            }
        });

        $.ajax({
            url: 'https://localhost:44377/api/facebook/getPagePosts?accesstoken=' + sessionStorage.getItem("fbToken"),
            method: 'GET',
            data: sessionStorage.getItem("fbToken"),
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken'),
            },
            dataType: 'json',
            success: function (data) {
                //console.log(JSON.parse(data));
                var postsObject = JSON.parse(data);
                var count = 1;
                $.each(postsObject, function (index, data) {
                    $.each(data, function (index, d) {
                        console.log(d.message);
                        if (d.message != null) {
                            $("#page").append('<div class="card bg-light">');
                            $("#page").append('<div class="card-body">');
                            $("#page").append('<h5 class="card-title">' + d.message + '</h5>');
                            $("#page").append('<input type="text" class="form-control" id="cmtBox' + count + '" placeholder="Enter comment here...">');
                            $("#page").append('<button type="button" id="cmtBtn'+ count +'" class="btn btn-primary">Submit</button>');
                            $("#page").append('</div>');
                            $("#page").append('</div>');
                        }
                        count = count + 1;
                        
                        
                    });
                });
            },
            error: function (e) {
                console.log("error " + e);
            }
        });
    });
});