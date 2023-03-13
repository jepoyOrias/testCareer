
$(document).ready(function () {

    google.accounts.id.initialize({
        client_id: "1001587784972-i6v4ef9ddn8v3om3rphen1iu3sjo4br6.apps.googleusercontent.com",
        callback: handleCredentialResponse
    });

    google.accounts.id.renderButton(
        document.getElementById("buttonDiv"),
        { theme: "outline", size: "large" }  // customization attributes
    );



});




function handleCredentialResponse(response) {
 
    const accessToken = parseJwt(response.credential);
    console.log("ID: " + accessToken.sub);
    console.log('Full Name: ' + accessToken.name);
    console.log('Given Name: ' + accessToken.given_name);
    console.log('Family Name: ' + accessToken.family_name);
    console.log("Image URL: " + accessToken.picture);
    console.log("Email: " + accessToken.email);
}



function parseJwt(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
};