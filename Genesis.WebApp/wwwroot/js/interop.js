window.GenesisWeb = {
    saveToken: function (token,username) {
        window.localStorage.setItem('jwt', token);
        window.localStorage.setItem('username', username);
        console.log("Authentication token/username has been stored.");
        return true;
    },
    getToken: function () {
        var token = window.localStorage.getItem('jwt');
        console.log(token ? "Authentication token read from storage." : "No authentication token found in storage.");
        return token;
    },
    getusername: function () {
        var username = window.localStorage.getItem('username');
        console.log(username ? "Authentication token read from storage." : "No authentication username found in storage.");
        return username;
    },
    destroyToken: function () {
        window.localStorage.removeItem('jwt');
        window.localStorage.removeItem('username');
        console.log("Authentication token has been deleted.");
        return true;
    },
    consoleLog: function (message) {
        console.log(message);
        return true;
    },
    scrollToTop: function () {
        window.scrollTo(0, 0);
        return true;
    }
};