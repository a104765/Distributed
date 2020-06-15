//Facebook Connection config

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
    }
}

function logOut() {
    FB.logout(function (response) {
        setElements(false);
    });
}

//-----------------------------------------------------------------------------------------------------------------------------------------


//Facebook API calls

function getProfile() {
    $.ajax({
        url: 'http://a104765-001-site1.ctempurl.com/api/facebook/getProfile?accesstoken=' + sessionStorage.getItem("fbToken"),
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken'),
        },
        data: sessionStorage.getItem("fbToken"),
        success: function (response) {
            console.log(response);
            let output = '<h3>Profile</h3>';
            output += `
                            <input id="prefSet" type="button" value="Set preferences" />
                            <div class="form-group form-check">
                                <input type="checkbox" class="form-check-input" id="prefEmail" checked>
                                <label class="form-check-label" for="prefEmail">Email</label>
                            </div>
                            <div class="form-group form-check">
                                <input type="checkbox" class="form-check-input" id="prefBirthday" checked>
                                <label class="form-check-label" for="prefBirthday">Birthday</label>
                            </div>

                            <div class="card card-body">
                                <h5 class="card-title" id="profileName">${response.name}</h5>
                                <p class="card-text" id="profileEmail">${response.email}</p>
                                <p class="card-text" id="profileBirthday">${response.birthday}</p>
                            </div>`
                ;
            document.getElementById('ProfileSpace').innerHTML = output;
            getUserPref();
        },
        error: function (e) {
            console.log("error " + e);
        }
    });
}

function getUserPref() {
    $.ajax({
        url: 'http://a104765-001-site1.ctempurl.com/api/facebook/getPref?',
        method: 'GET',
        contentType: 'application/json',
        data: {
            username: sessionStorage.getItem("username")
        },
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken')
        },
        success: function (response) {


            if (response.Email == true) {
                $("#prefEmail").prop("checked", true);
            }
            else {
                $("#prefEmail").prop("checked", false);
                $("#profileEmail").hide();
            }

            if (response.Birthday == true) {
                $("#prefBirthday").prop("checked", true);
            }
            else {
                $("#prefBirthday").prop("checked", false);
                $("#profileBirthday").hide();
            }


        },
        error: function (e) {
            alert(e);
        }
    });
}

function getUserLikes() {
    $.ajax({
        url: 'http://a104765-001-site1.ctempurl.com/api/facebook/getUserLikes?accesstoken=' + sessionStorage.getItem("fbToken"),
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken'),
        },
        data: sessionStorage.getItem("fbToken"),
        success: function (response) {

            let output = '<h3>Likes</h3>';
            for (let i in response) {
                if (response[i]) {
                    output += `
                            <ul class="list-group">
                                <li class="list-group-item">${response[i].name}</li>
                            </ul>`
                        ;
                }
            }
            document.getElementById('LikesSpace').innerHTML = output;
        }
    })
}

function getPictures() {
    $.ajax({
        url: 'http://a104765-001-site1.ctempurl.com/api/facebook/getPictures?accesstoken=' + sessionStorage.getItem("fbToken"),
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken'),
        },
        data: sessionStorage.getItem("fbToken"),
        success: function (data) {
            let output = '<h3>Photos</h3>';
            for (x in data) {
                if (data[x]) {
                    output += `<div class="card" style="width: 18rem;">
                                <img src="${data[x].picture}" class="card-img-top" alt="...">
                                <div class="card-body">
                                <p class="card-text">${data[x].name}</p >
                                </div>
                                </div>
                                        `;
                }
            }
            document.getElementById('PhotosSpace').innerHTML = output;
        }
    })
}

function getFeed() {
    $.ajax({
        url: 'http://a104765-001-site1.ctempurl.com/api/facebook/getFeed?accesstoken=' + sessionStorage.getItem("fbToken"),
        method: 'GET',
        data: sessionStorage.getItem("fbToken"),
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken'),
        },
        dataType: 'json',
        success: function (data) {
            let output = '<h3>Feed</h3>';
            for (x in data) {
                if (data[x]) {
                    output += `<ul class="list-group">
                                <li class="list-group-item">${data[x].message}</li>
                            </ul>
                                        `;
                }
            }
            document.getElementById('FeedSpace').innerHTML = output;
        },
        error: function (e) {
            console.log("error " + e);
        }
    });
}


function getPagePosts() {
    $.ajax({
        url: 'http://a104765-001-site1.ctempurl.com/api/facebook/getPagePosts?accesstoken=' + sessionStorage.getItem("fbToken"),
        method: 'GET',
        method: 'GET',
        data: sessionStorage.getItem("fbToken"),
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken'),
        },
        dataType: 'json',
        success: function (data) {
            var postsObject = JSON.parse(data);
            let output = '<h3>Page Feed</h3>';
            for (x in postsObject.data) {
                if (postsObject.data[x].message != null) {

                    output += `
                                <div class="card bg-light">
                                <div class="card-body">
                                <h5 class="card-title"> ${postsObject.data[x].message} </h5>
                                <input type="text">
                                <button class="postbtn" id="${postsObject.data[x].id}"> Comment </button>
                                </div>
                                </div>
                                `;
                }
            }
            document.getElementById('PageFeedSpace').innerHTML = output;
        },
        error: function (e) {
            console.log("error " + e);
        }
    });
}

$(document).on("click", ".postbtn", function () {

    var id = $(this).attr("id");
    var msg = $(this).siblings("input").val();

    $.ajax({
        url: 'http://a104765-001-site1.ctempurl.com/api/facebook/getToken?accesstoken=' + sessionStorage.getItem("fbToken"),
        method: 'GET',
        contentType: 'application/json',
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken')
        },
        success: function (response) {

            sessionStorage.setItem("token", response["data"][0].access_token);

            $.ajax({
                url: 'http://a104765-001-site1.ctempurl.com/api/facebook/PostComment?id=' + id + '&message=' + msg + '&token=' + sessionStorage.getItem("token"),
                method: 'POST',
                headers: {
                    'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken')
                },
                contentType: 'application/json',
                success: function (response) {
                    alert("Comment Posted");
                },
                error: function (jqXHR) {
                    $('#divErrorText').text(jqXHR.responseText);
                    $('#divError').show('fade');
                }
            });
        },
        error: function (e) {
            alert(e);
        }
    });

    console.log($(this).attr("id"));

    
});

$(document).ready(function () {

    

    $('#linkClose').click(function () {
        $('#divError').hide('fade');
    });

    $('#btnLogin').click(function () {
        $.ajax({
            url: 'http://a104765-001-site1.ctempurl.com/token',
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

            },
            error: function (jqXHR) {
                $('#divErrorText').text(jqXHR.responseText);
                $('#divError').show('fade');
            }
        });

    });

    


    $(document).ready(function () {

        $('#linkClose').click(function () {
            $('#divError').hide('fade');
        });

        $('#btnRegister').click(function () {
            $.ajax({
                url: 'http://a104765-001-site1.ctempurl.com/api/Account/Register',
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


    $(document).on("change", "#prefEmail", function () {
        var checkBox = document.getElementById("prefEmail");
        var text = document.getElementById("profileEmail");

        if (checkBox.checked == true) {
            text.style.display = "block";
        } else {
            text.style.display = "none";
        }
    });

    $(document).on("change", "#prefBirthday", function () {
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
            url: 'http://a104765-001-site1.ctempurl.com/api/facebook/setPref?',
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


    $(document).on("click", "#logoutBtn", function () {
        sessionStorage.clear();
    });

    if (sessionStorage.getItem("accessToken") == null) {
        $("#logoutBtn").hide();
    }
    else {
        $("#logoutBtn").show();
    }





});


















  // Your web app's Firebase configuration
        var firebaseConfig = {
            apiKey: "AIzaSyBGRmFUoOk4txccNOpkLUi4LrNY8z2hFqM",
    authDomain: "distributed-fe5c5.firebaseapp.com",
    databaseURL: "https://distributed-fe5c5.firebaseio.com",
    projectId: "distributed-fe5c5",
    storageBucket: "distributed-fe5c5.appspot.com",
    messagingSenderId: "362555904502",
    appId: "1:362555904502:web:f0dd7706acec7a2b30b7d7",
    measurementId: "G-DZM818RT7B"
  };
  // Initialize Firebase
firebase.initializeApp(firebaseConfig);



function signin() {
    var provider = new firebase.auth.TwitterAuthProvider();
    firebase.auth().signInWithPopup(provider).then(function (result) {
        
        console.log(result);
        var token = result.credential.accessToken;
        sessionStorage.setItem("TwitterToken", token);
        console.log(token);
        var secret = result.credential.secret;
        sessionStorage.setItem("TwitterSecret", secret);
        console.log(secret);
        // The signed-in user info.
        var user = result.user;
        console.log(user);
        // ...
    }).catch(function (error) {
        // Handle Errors here.
        var errorCode = error.code;
        var errorMessage = error.message;
        // The email of the user's account used.
        var email = error.email;
        // The firebase.auth.AuthCredential type that was used.
        var credential = error.credential;
        // ...
    });


    
}

function getHomeTweets() {
    $.ajax({
        url: 'http://a104765-001-site1.ctempurl.com/api/twitter/HomeTweets',
        method: 'GET',
        data:
        {
            access_token: sessionStorage.getItem("TwitterToken"),
            accessSecret_token: sessionStorage.getItem("TwitterSecret")
        },
        headers:
        {
            'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken')
        },
        success: function (response) {
            let output = '<h3>Home Tweets</h3>';
            for (let i in response) {
                if (response[i]) {
                    output += `
                            <ul class="list-group">
                                <li class="list-group-item">${response[i]}</li>
                            </ul>`
                        ;
                }
            }
            document.getElementById('HomeTweetsSpace').innerHTML = output;
        },
        error: function (jqXHR) {
            $('#divErrorText').text(jqXHR.responseText);
            $('#divError').show('fade');
        }
    });
}

function getSearchHistory() {
    $.ajax({
        url: 'http://a104765-001-site1.ctempurl.com/api/twitter/SearchHistory?username=' + sessionStorage.getItem("username"),
        method: 'POST',
        headers:
        {
            'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken')
        },
        success: function (response) {
            console.log(response);
        },
        error: function (jqXHR) {
            $('#divErrorText').text(jqXHR.responseText);
            $('#divError').show('fade');
        }
    });
}


function getFavTweets() {
    $.ajax({
        url: 'http://a104765-001-site1.ctempurl.com/api/twitter/FavTweets',
        method: 'GET',
        data:
        {
            access_token: sessionStorage.getItem("TwitterToken"),
            accessSecret_token: sessionStorage.getItem("TwitterSecret")
        },
        headers:
        {
            'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken')
        },
        success: function (response) {
            let output = '<h3>Favourite Tweets</h3>';
            for (let i in response) {
                if (response[i]) {
                    output += `
                            <ul class="list-group">
                                <li class="list-group-item">${response[i]}</li>
                            </ul>`
                        ;
                }
            }
            document.getElementById('FavTweetsSpace').innerHTML = output;
        },
        error: function (jqXHR) {
            $('#divErrorText').text(jqXHR.responseText);
            $('#divError').show('fade');
        }
    });
}

function getFriends() {
    $.ajax({
        url: 'http://a104765-001-site1.ctempurl.com/api/twitter/TwitterFriends',
        method: 'GET',
        data:
        {
            access_token: sessionStorage.getItem("TwitterToken"),
            accessSecret_token: sessionStorage.getItem("TwitterSecret")
        },
        headers:
        {
            'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken')
        },
        success: function (response) {
            let output = '<h3>Friends</h3>';
            for (let i in response) {
                if (response[i]) {
                    output += `
                            <ul class="list-group">
                                <li class="list-group-item">${response[i].name}</li>
                            </ul>`
                        ;
                }
            }
            document.getElementById('GetFriendsSpace').innerHTML = output;
        },
        error: function (jqXHR) {
            $('#divErrorText').text(jqXHR.responseText);
            $('#divError').show('fade');
        }
    });
}

function getFollowers() {
    $.ajax({
        url: 'http://a104765-001-site1.ctempurl.com/api/twitter/TwitterFollowers',
        method: 'GET',
        data:
        {
            access_token: sessionStorage.getItem("TwitterToken"),
            accessSecret_token: sessionStorage.getItem("TwitterSecret")
        },
        headers:
        {
            'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken')
        },
        success: function (response) {
            let output = '<h3>Followers</h3>';
            for (let i in response) {
                if (response[i]) {
                    output += `
                            <ul class="list-group">
                                <li class="list-group-item">${response[i].name}</li>
                            </ul>`
                        ;
                }
            }
            document.getElementById('FollowersSpace').innerHTML = output;
        },
        error: function (jqXHR) {
            $('#divErrorText').text(jqXHR.responseText);
            $('#divError').show('fade');
        }
    });
}

function postTweet() {
    var accessToken = sessionStorage.getItem("TwitterToken");
    var accessSecretToken = sessionStorage.getItem("TwitterSecret");
    var tweet = document.getElementById('tweet').value;
    $.ajax({
        url: 'http://a104765-001-site1.ctempurl.com/api/twitter/PostTweet?access_token=' + accessToken + '&accessSecret_token=' + accessSecretToken + '&tweet=' + tweet,
        method: 'POST',
        headers:
        {
            'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken')
        },
        success: function (response) {
            alert("Tweet posted!");
        },
        error: function (jqXHR) {
            alert("A problem occured");
        }
    });
}


function getSearchHistory() {

    $.ajax({
        url: 'http://a104765-001-site1.ctempurl.com/api/twitter/SearchHistory?username=' + sessionStorage.getItem('username'),
        method: 'GET',
        data:
        {
            access_token: sessionStorage.getItem("TwitterToken"),
            accessSecret_token: sessionStorage.getItem("TwitterSecret"),
        },
        headers:
        {
            'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken')
        },
        success: function (response) {
            console.log(response);
            document.getElementById('dropdown').innerHTML = `<select class="custom-select" id="historyDropDown"></select>`;
            for (let i in response) {
                if (response[i]) {
                    $('#historyDropDown').append($('<option>', {
                        value: response[i].searchQuery,
                        text: response[i].searchQuery
                    }));
                }
            }
        },
        error: function (jqXHR) {
            $('#divErrorText').text(jqXHR.responseText);
            $('#divError').show('fade');
        }
    });
}



function Search(query) {
    $.ajax({
        url: 'http://a104765-001-site1.ctempurl.com/api/twitter/Search?username=' + sessionStorage.getItem('username'),
        method: 'GET',
        data:
        {
            access_token: sessionStorage.getItem("TwitterToken"),
            accessSecret_token: sessionStorage.getItem("TwitterSecret"),
            query: query
        },
        headers:
        {
            'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken')
        },
        success: function (response) {
            console.log(response);
            let output = '';
            for (let i in response) {
                if (response[i]) {
                    output += `
                            <ul class="list-group">
                                <li class="list-group-item">${response[i].text}</li>
                            </ul>`
                        ;
                }
            }
            document.getElementById('SearchSpace').innerHTML = output;
            
            },
            error: function (jqXHR) {
                                $('#divErrorText').text(jqXHR.responseText);
                $('#divError').show('fade');
            }
        });
    }




function FBopenTab(evt, tabName) {
    // Declare all variables
    var i, tabcontent, tablinks;

    // Get all elements with class="tabcontent" and hide them
    tabcontent = document.getElementsByClassName("FBtabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }

    // Get all elements with class="tablinks" and remove the class "active"
    tablinks = document.getElementsByClassName("FBtablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }

    // Show the current tab, and add an "active" class to the button that opened the tab
    document.getElementById(tabName).style.display = "block";
    evt.currentTarget.className += " active";
}

function TWopenTab(evt, tabName) {
    // Declare all variables
    var i, tabcontent, tablinks;

    // Get all elements with class="tabcontent" and hide them
    tabcontent = document.getElementsByClassName("TWtabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }

    // Get all elements with class="tablinks" and remove the class "active"
    tablinks = document.getElementsByClassName("TWtablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }

    // Show the current tab, and add an "active" class to the button that opened the tab
    document.getElementById(tabName).style.display = "block";
    evt.currentTarget.className += " active";
}

