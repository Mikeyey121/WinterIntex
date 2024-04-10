// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Check if the user has provided consent
var hasConsent = localStorage.getItem('cookieConsent');
if (!hasConsent) {
    // Display the cookie consent notification
    showCookieConsent();
}

function showCookieConsent() {
    var consentDiv = document.createElement('div');
    consentDiv.id = 'cookieConsent';
    consentDiv.innerHTML = `
                    <div class="cookie-notification">
                        This website uses cookies to ensure you get the best experience on our website.
                        <button onclick="acceptCookies()">Accept</button>
                    </div>`;

    // Get a reference to the first child node of the body
    var firstChild = document.body.firstChild;


    // Insert the consentDiv before the firstChild
    document.body.insertBefore(consentDiv, firstChild);
}

function acceptCookies() {
    // Set the consent flag in localStorage
    localStorage.setItem('cookieConsent', 'true');
    // Hide the cookie consent notification
    document.getElementById('cookieConsent').style.display = 'none';
}