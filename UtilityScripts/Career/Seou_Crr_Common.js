let _lazyloadThrottleTimeout;
function lazyload() {
    let last_known_scroll_position = window.scrollY;
    if (last_known_scroll_position < 100) return;

    if (_lazyloadThrottleTimeout) {
        clearTimeout(_lazyloadThrottleTimeout);
    }

    let lazyloadImages = document.querySelectorAll('.logo-lazy');
    _lazyloadThrottleTimeout = setTimeout(function () {
        lazyloadImages.forEach(function (img) {
            img.src = img.dataset.src;
            img.classList.remove('logo-lazy');
        });
    }, 20);

    if (lazyloadImages.length === 0) {
        document.removeEventListener("scroll", lazyload);
    }
}

document.addEventListener("DOMContentLoaded", function () {
    var lazyloadImages;

    if ("IntersectionObserver" in window) {
        lazyloadImages = document.querySelectorAll(".lazy");
        if (lazyloadImages.length === 0) {
            return;
        }
        var imageObserver = new IntersectionObserver(function (entries, observer) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    var image = entry.target;
                    image.src = image.dataset.src;
                    image.classList.remove("lazy");
                    imageObserver.unobserve(image);
                }
            });
        });
        lazyloadImages.forEach(function (image) {
            imageObserver.observe(image);
        });

    }
    else {
        lazyloadImages = document.querySelectorAll(".lazy");
        if (lazyloadImages.length === 0) {
            return;
        }
        let layload_img = function lazyload() {
            if (lazyloadThrottleTimeout) {
                clearTimeout(lazyloadThrottleTimeout);
            }

            lazyloadThrottleTimeout = setTimeout(function () {
                var scrollTop = window.pageYOffset;
                lazyloadImages.forEach(function (img) {
                    if (img.offsetTop < (window.innerHeight + scrollTop)) {
                        img.src = img.dataset.src;
                        img.classList.remove('lazy');
                    }
                });
                if (lazyloadImages.length === 0) {
                    document.removeEventListener("scroll", layload_img);
                    window.removeEventListener("resize", layload_img);
                    window.removeEventListener("orientationChange", layload_img);
                }
            }, 20);
        }

        document.addEventListener("scroll", lazyload);
        window.addEventListener("resize", lazyload);
        window.addEventListener("orientationChange", lazyload);
    }
})


function fndJobTypeaheadInit() {
    let xhrFindJob;
    $('#txt_typeahead_findjob').siblings(".icon-remove").hide();
    $(document).on("mouseup", function (e) {
        if (e.target !== $('#txt_typeahead_findjob')[0] && $.inArray(e.target, $(".crr-findjob-typeahead-menu").find("a")) === -1) {
            $(".crr-findjob-typeahead-menu").hide();
        }
    });
    $('#txt_typeahead_findjob').on("click", function () {
        if ($(".crr-findjob-typeahead-menu .crr-findjob-item a").length > 0 && $('#txt_typeahead_findjob').val().trim() !== "") {
            $(".crr-findjob-typeahead-menu").show();
        } else {
            if ($(this).val().trim().length >= 3) {
                xhrFindJob = getJobsByKeyword();
            }

        }
    });

    $('#txt_typeahead_findjob').keyup(delay(function (e) {
        var searchKeyowrdLen = $(this).val().trim().length;
        if (searchKeyowrdLen >= 3) {
            xhrFindJob = getJobsByKeyword();
        }
        else {
            $('#txt_typeahead_findjob').siblings(".crr-findjob-typeahead-spinner").hide();
            $('#txt_typeahead_findjob').siblings(".icon-remove").hide();
            $(".crr-findjob-typeahead-menu").html("");
            $(".crr-findjob-typeahead-menu").hide();
            if (xhrFindJob !== null) {
                xhrFindJob.abort();
            }
        }
    }, 500));
}

function getJobsByKeyword() {
    if ($('#txt_typeahead_findjob').val().trim() === "") {
        return;
    }
    let searchtype = $(".crr-input-searchtype .filters-selected label").data("searchtype");
    return $.ajax({
        url: CRR_REQUESTDATA_API.GetJobsByKeyword,
        data: {
            strKeyWords: $('#txt_typeahead_findjob').val().trim(),
            searchType: searchtype
        },
        type: "post",
        cache: false,
        beforeSend: function () {
            $('#txt_typeahead_findjob').siblings(".crr-findjob-typeahead-spinner").show();
            $('#txt_typeahead_findjob').siblings(".icon-remove").hide();
        },
        success: function (data) {
            $('#txt_typeahead_findjob').siblings(".crr-findjob-typeahead-spinner").hide();
            $('#txt_typeahead_findjob').siblings(".icon-remove").show();
            $(".crr-findjob-typeahead-menu").html(data);
            $(".crr-findjob-typeahead-menu").show();
        }
    });
}

var typeaheadDatums_Location = [];
function locationTypeaheadInit() {
    var remoteHost = CRR_REQUESTDATA_API.GetLocationsByKeyword;
    var engine = new Bloodhound({
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        datumTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: remoteHost + "?strLocationKeyword=%QUERY&bolIsSearchCountry=true",
            wildcard: '%QUERY',
            cache: true,
            transform: function (jsonData) {
                typeaheadDatums_Location.length = 0;
                var tempLoc = "";
                $.each(jsonData, function (i, el) {
                    if (el.Country !== null)
                        tempLoc = el.Country;
                    else if (el.City === null && el.StateCode !== null) {
                        tempLoc = el.State;
                    }
                    else if (el.ZipCode !== null) {
                        tempLoc = [el.ZipCode, el.City, el.StateCode].join(", ");
                    }
                    else if (el.City !== null)
                        tempLoc = [el.City, el.StateCode].join(", ");
                    if (-1 === typeaheadDatums_Location.findIndex(function (obj) { return obj.desc === tempLoc; }))
                        typeaheadDatums_Location.push({
                            desc: tempLoc, loc: el
                        });
                });

                return typeaheadDatums_Location;
            }
        }
    });
    var $hint = $("#txt_typeahead_location").siblings(".crr-location-typeahead-hint");
    var $menu = $("#txt_typeahead_location").siblings(".crr-location-typeahead-menu");
    $("#txt_typeahead_location").typeahead({
        hint: $hint,
        menu: $menu,
        minLength: 3,
        highlight: true,
        classNames: {
            open: 'is-open',
            cursor: 'is-active',
            suggestion: 'crr-location-typeahead-suggestion',
            selectable: 'crr-location-typeahead-selectable'
        }
    }, {
            limit: 10,
            source: engine,
            displayKey: 'desc',
            templates: {
                suggestion: function (data) { return "<div>" + data.desc + "</div>"; },
                empty: ['<div class="crr-location-typeahead-empty-message">', 'Your search turned up 0 results.', '</div>'].join('\n')
            }
        }).on('typeahead:asyncrequest', function () {
            $(this).siblings(".crr-location-typeahead-spinner").show();
            $(this).siblings(".icon-remove").hide();
        }).on('typeahead:asynccancel typeahead:asyncreceive', function () {
            $(this).siblings(".crr-location-typeahead-spinner").hide();
            $(this).siblings(".icon-remove").show();
        }).on('click', function () {
            if ($("#txt_typeahead_location").val().trim().length > 0 && !$(this).siblings(".crr-location-typeahead-spinner").is(":visible")) {
                $(".crr-location-typeahead-menu").attr("aria-expanded", true);
                $(this).siblings(".icon-remove").show();
            } else {
                $(this).siblings(".icon-remove").hide();
            }
        }).on('typeahead:select', function (obj, datum, name) {
            document.findjob_form.submit();
        }).on('keyup', delay(function (e) {
            if ($("#txt_typeahead_location").val().trim().length === 0) {
                $(this).siblings(".icon-remove").hide();
            }
        }, 500));
}

function SalSelectJobFromTypeahead(obj) {
    $("#txt_typeahead_findjob").val($(obj).text());
    $(".crr-findjob-typeahead-menu").html("");
    $(".crr-findjob-typeahead-menu").hide();
    $("#txt_typeahead_findjob").focus();
    document.findjob_form.submit();
}

function emptyTypeaheadInputText(obj) {
    $(obj).siblings("input[type='search']").typeahead('val', '');
    $(obj).siblings("input[type='search']").val('');
    $(obj).hide();
}

function delay(callback, ms) {
    var timer = 0;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 0);
    };
}

function addHidden(formName, inputName, inputValue) {
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
}

function scrollToSection(id) {
    const yOffset = -10;
    const element = document.getElementById(id);
    const y = element.getBoundingClientRect().top + window.pageYOffset + yOffset;
    window.scrollTo({ top: y, behavior: 'smooth' });
}
$(window).scroll(function () {
    if ($(this).scrollTop() < 500) {
        $(".crr-scrolltotop-icon,.crr-noresultscrolltotop-icon").css("display", "none");
    } else {
        $(".crr-scrolltotop-icon,.crr-noresultscrolltotop-icon").css("display", "block");
    }
})

function changeSearchType(obj) {
    if ($(obj).closest("li").hasClass("filters-selected")) {
        return;
    }
    //document.findjob_form.submit();
    const searchtype = $(obj).data("searchtype");
    $(obj).closest("li").addClass("filters-selected");
    $(obj).closest("li").siblings("li").removeClass("filters-selected");
    getJobsByKeyword();
    $("#txt_typeahead_findjob").focus();
    $("#txt_typeahead_findjob").attr("placeholder", SEARCH_KEYWORD_PlACEHolder[Number(searchtype)]);
}
$('.crr-scrolltotop-icon').on("click", function () {
    $(".carr-detail-info").removeClass("is-pinned");
}); 