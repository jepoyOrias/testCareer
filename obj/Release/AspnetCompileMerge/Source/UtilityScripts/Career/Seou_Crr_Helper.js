/**
 * Number.prototype.format(n, x, s, c)
 * local project use format(0, 3, ',', '.')
 * @param integer n: length of decimal
 * @param integer x: length of whole part
 * @param mixed   s: sections delimiter
 * @param mixed   c: decimal delimiter
 */

Number.prototype.format = function (n, x, s, c) {
    var re = '\\d(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\D' : '$') + ')',
        num = this.toFixed(Math.max(0, ~~n));

    return (c ? num.replace('.', c) : num).replace(new RegExp(re, 'g'), '$&' + (s || ','));
};


var Helper = (function ($) {
    return {
        isValidEmailAddress: function (emailAddress) {
            var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
            return pattern.test(emailAddress);
        },
        CheckEnter: function (e) {
            e = e || event;
            var txtArea = /textarea/i.test((e.target || e.srcElement).tagName);
            return txtArea || (e.keyCode || e.which || e.charCode || 0) !== 13;
        },
        GetQueryString: function (key, url) {
            url = window.top.location.href;
            var regex = new RegExp("[?&]" + key + "(=([^&#]*)|&|#|$)", "i"), results = regex.exec(url);

            if (!results) {
                if (console) {
                    console.log('returning null for ' + key + ' and ' + url);
                }
                return null;
            }

            if (!results[2]) {
                if (console) {
                    console.log('returning empty for ' + key + ' and ' + url);
                }
                return '';
            }

            return decodeURIComponent(results[2].replace(/\+/g, " "));
        },
        removeUrlParameter: function (url, parameter) {
            if (!url) url = window.location.href;
            var urlparts = url.split('?');
            if (urlparts.length >= 2) {

                var prefix = encodeURIComponent(parameter) + '=';
                var pars = urlparts[1].split(/[&;]/g);

                //reverse iteration as may be destructive
                for (var i = pars.length; i-- > 0;) {
                    //idiom for string.startsWith
                    if (pars[i].lastIndexOf(prefix, 0) !== -1) {
                        pars.splice(i, 1);
                    }
                }

                return urlparts[0] + (pars.length > 0 ? '?' + pars.join('&') : '');
            }
            return url;
        },

        updateUrlParameter: function (url, key, value) {
            if (!url) url = window.location.href;
            url = url.split('#')[0];
            var re = new RegExp("([?&])" + key + "=.*?(&|#|$)(.*)", "gi"),
                hash;

            if (re.test(url)) {
                if (typeof value !== 'undefined' && value !== null)
                    return url.replace(re, '$1' + key + "=" + value + '$2$3');
                else {
                    hash = url.split('#');
                    url = hash[0].replace(re, '$1$3').replace(/(&|\?)$/, '');
                    if (typeof hash[1] !== 'undefined' && hash[1] !== null)
                        url += '#' + hash[1];
                    return url;
                }
            }
            else {
                if (typeof value !== 'undefined' && value !== null) {
                    var separator = url.indexOf('?') !== -1 ? '&' : '?';
                    hash = url.split('#');
                    url = hash[0] + separator + key + '=' + value;
                    if (typeof hash[1] !== 'undefined' && hash[1] !== null)
                        url += '#' + hash[1];
                    return url;
                }
                else
                    return url;
            }
        },
        addHidden: function (formName, inputName, inputValue) {
            if (typeof inputValue === 'undefined' || inputValue === null || inputValue === "")
                return;

            if (!$("#" + formName + " input[name='" + inputName + "']").length) {
                var input = document.createElement('input');
                input.type = 'hidden';
                input.tagName = inputName;
                input.name = inputName;
                input.value = inputValue;
                document.getElementById(formName).appendChild(input);
            }
        },
        getCookie: function (cname) {
            var name = cname + "=";
            var ca = top.document.cookie.split(";");
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == " ") c = c.substring(1);
                if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
            }
            return "";
        },

        setCookie: function (cname, cvalue, exdays) {
            if (typeof exdays === 'undefined') {
                top.document.cookie = cname + "=" + cvalue + "; path=/";
                return;
            }
            var d = new Date();
            d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
            var expires = "expires=" + d.toUTCString();
            top.document.cookie = cname + "=" + cvalue + "; " + expires + ";path=/";
        }

    };
})(jQuery);

// array of ISO YYYY-MM-DD format dates
var publicHolidays = {
    usa: [
        "2021/01/01",
        "2021/01/18",
        "2021/02/15",
        "2021/05/31",
        "2021/07/05",
        "2021/09/06",
        "2021/10/11",
        "2021/11/11",
        "2021/11/25",
        "2021/12/24",
        "2021/12/31",
    ],
};