window.addEventListener("load", function () {
    let message = 'This site uses cookies and other similar technologies to provide site functionality, analyze traffic and usage, and deliver content tailored to your interests.';
    if (window.innerWidth > 736) {
        message += " | "
    }
    window.cookieconsent.initialise({
        cookie: {
            "name": "isdenysaladsc",
            "domain": settings.cookieDomain
        },
        "palette": {
            "popup": {
                "background": "#f6f6f8",

            },
            "button": {
                "background": "#17bd64",
                "text": "#ffffff"
            }
        },
        "position": "top",
        "content": {
            "message": '<span style="font-size:14px; color:#01374f;">' + message + '</span>',
            "dismiss": "I Accept",
            "link": '<span style="color:#0773c8;font-weight:bold;">Learn More</span>',
            "href": settings.baseUrl + "legal/privacy-controls/"
        }
    })
});