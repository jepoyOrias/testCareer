const salDestinationHost =  (window.location.protocol + '//' + window.location.host);
const GLOBAL_REQUESTDATA_API = {
    GetB2BGlobalSearchResultByKeyword: salDestinationHost + '/business/search/BusinessSearch/SalAjxGetB2BGlobalSearchResult',
    GetB2BGlobalBannerNews: salDestinationHost + '/business/search/BusinessSearch/SalAjxGetGlobalBannerNews',
};
const mobileWidth = window.matchMedia("(hover: none)").matches && screen.width >= 1100 ? screen.width : 1100;
const isMobileMenu = (window.matchMedia("(hover: none)").matches && screen.width >= 1100) || screen.width < 1100;

$(document).ready(function () {
    //bindEventForGlobalSearch();
    //CrrGlobalMenuInit();
    //CrrToggleSearchBox();

    initial_HeaderStickyEvent();

    if (window.matchMedia("(hover: none)").matches && screen.width >= 1100) {
        document.getElementsByClassName("crr-emp-global-gradient")[0].classList.add('i-medium');
    }

});

function applyHeaderSticky() {
    let _$stickies = [].slice.call(document.querySelectorAll('.crr-header--sticky'));
    _$stickies.forEach(function (_$sticky) {
        if (CSS.supports && (CSS.supports('position', 'sticky') || CSS.supports('position', '-webkit-sticky'))) {
            applyHeaderStickyClass(_$sticky);
        }
    });
}

function applyHeaderStickyClass(_$sticky) {
    let currentOffset = _$sticky.getBoundingClientRect().top;
    let stickyOffset = parseInt(getComputedStyle(_$sticky).top.replace('px', ''));
    let isStuck = currentOffset <= stickyOffset;
    if ($('#ca_callout_container').length > 0) {
        _$sticky.classList.toggle('crr-header--scrolling', isStuck);
    }
    else {
        let scrollPos = window.scrollY || window.scrollTop || document.getElementsByTagName("html")[0].scrollTop;
        _$sticky.classList.toggle('crr-header--scrolling', scrollPos > 0);
    }
}

let $globalNav;
function initial_HeaderStickyEvent() {
     //$globalNav = $('.crr-emp-global-gradient:first'); // if debug code locally, please uncomment it and comment below one line code
     $globalNav = $('.crr-emp-global-gradient:first').closest('.fl-builder-content'); // First we get the viewport height and we multiple it by 1% to get a value for a vh unit

    let vh = window.innerHeight * 0.01; // Then we set the value in the --vh custom property to the root of the document

    document.documentElement.style.setProperty('--vh', vh + "px"); // We listen to the resize event

    window.addEventListener('resize', function () {
        // We execute the same script as before
        let vh = window.innerHeight * 0.01;
        document.documentElement.style.setProperty('--vh', vh + "px");
    });
    $globalNav.addClass('crr-header--sticky');
    window.addEventListener('scroll', function () {
        applyHeaderSticky();
    });
}

function popupSearchResult() {
    $("#global-search-result").show();
    if (isMobileMenu) {
        $('.crr-global-searchresult-wrapper').addClass('margin--bottom');
    }
    else {
        $('#global-search-result').addClass('search-result-max-height--vh');
    }
}

var xhrGlobalSearch;
var headerHeight;
function bindEventForGlobalSearch() {
    headerHeight = $(".crr-emp-header-global-menu-content").height();

    $(document).on("mouseup", function (e) {
        if (e.target !== $('#header_globalsearch-input')[0] && $.inArray(e.target, $("#global-search-result").find("a")) === -1) {
            $("#global-search-result").hide();
            if ($(window).width() <= mobileWidth) {
                if ($("#global-search-result").html().trim() !== "") {
                    $(".crr-emp-header-global-menu-content").height("auto");
                }
            }
        }
    });

    $('#header_globalsearch-input').on("click", function () {
        if ($(window).width() <= mobileWidth) {
            $(".global_header_btn").height("auto");
        }
        if ($("#global-search-result").html().trim() !== "") {
            if ($(window).width() <= mobileWidth) {
                if ($("#global-search-result").html().trim() !== "") {
                    $(".crr-emp-header-global-menu-content").height(screen.height);
                }
            }
            popupSearchResult();
        } else {
            if ($(this).val().trim().length >= 1) {
                xhrGlobalSearch = $.ajax({
                    url: GLOBAL_REQUESTDATA_API.GetB2BGlobalSearchResultByKeyword,
                    data: {
                        strKeyword: $('#header_globalsearch-input').val().trim()
                    },
                    type: "get",
                    cache: false,
                    beforeSend: function () {
                        $('#header_globalsearch-input').siblings(".icon-search").hide();
                        $('#header_globalsearch-input').siblings(".header_globalsearch-trafficdrivertad-spinner").show();
                        if ($("#global-search-result").html() === "") {
                            headerHeight = $(".crr-emp-header-global-menu-content").height();
                        }
                    },
                    success: function (data) {
                        $('#header_globalsearch-input').siblings(".icon-search").show();
                        $('#header_globalsearch-input').siblings(".header_globalsearch-trafficdrivertad-spinner").hide();
                        $("#global-search-result").html(data);
                        if ($(window).width() <= mobileWidth) {
                            if ($("#global-search-result").html().trim() !== "") {
                                $(".crr-emp-header-global-menu-content").height(screen.height);
                            }
                        }
                        popupSearchResult();
                    }
                });
            }
        }
    });

    $('#header_globalsearch-input').keyup(delay(function (e) {
        var searchKeyowrdLen = $(this).val().trim().length;
        if (e.keyCode === 13) {
            if (searchKeyowrdLen === 0) {
                addRedBordForGlobalSearchBox();
                return;
            } else {
                window.location.href = salDestinationHost + "/business/search?keyword=" + encodeURIComponent($("#header_globalsearch-input").val()) + "&r=g";
            }
        }

        resetBordForGlobalSearchBox();
        if (searchKeyowrdLen >= 1) {
            xhrGlobalSearch = $.ajax({
                url: GLOBAL_REQUESTDATA_API.GetB2BGlobalSearchResultByKeyword,
                data: {
                    strKeyword: $('#header_globalsearch-input').val().trim()
                },
                type: "get",
                cache: false,
                beforeSend: function () {
                    $('#header_globalsearch-input').siblings(".icon-search").hide();
                    $('#header_globalsearch-input').siblings(".header_globalsearch-trafficdrivertad-spinner").show();
                    if ($("#global-search-result").html() === "") {
                        headerHeight = $(".crr-emp-header-global-menu-content").height();
                    }
                },
                success: function (data) {
                    $('#header_globalsearch-input').siblings(".icon-search").show();
                    $('#header_globalsearch-input').siblings(".header_globalsearch-trafficdrivertad-spinner").hide();

                    $("#global-search-result").html(data);
                    if ($(window).width() <= mobileWidth) {
                        if ($("#global-search-result").html().trim() !== "") {
                            $(".crr-emp-header-global-menu-content").height(screen.height);
                        }
                    }
                    popupSearchResult();

                }
            });
        }
        else {
            $('#header_globalsearch-input').siblings(".header_globalsearch-trafficdrivertad-spinner").hide();
            $("#global-search-result").html("");
            if (searchKeyowrdLen === 0) {
                $('#header_globalsearch-input').siblings(".icon-search").show();
            }

            if ($(window).width() <= mobileWidth) {
                if ($("#global-search-result").html().trim() === "") {
                    $(".crr-emp-header-global-menu-content").height("auto");
                }
            }
            if (xhrGlobalSearch !== null) {
                xhrGlobalSearch.abort();
            }
        }
    }, 500));

    $('#form_globalheadersearch .icon-search').on('click', function () {
        if ($("#header_globalsearch-input").val().trim().length === 0) {
            addRedBordForGlobalSearchBox();
            return;
        }
        else {
            window.location.href = salDestinationHost + "/business/search?keyword=" + encodeURIComponent($("#header_globalsearch-input").val()) + "&r=g";
        }
    });

}

function addRedBordForGlobalSearchBox() {
    $("#header_globalsearch-input").css({ "border": "1px solid #c32026" });
    $("#header_globalsearch-input").addClass("header_globalsearch-input-error");
    $("#header_globalsearch-input").focus();
}

function resetBordForGlobalSearchBox() {
    $("#header_globalsearch-input").css({ "border": "none" });
    $("#header_globalsearch-input").removeClass("header_globalsearch-input-error");
}

function CrrToggleSearchBox() {
    $("#form_globalheadersearch").hover(
        function () {
            if ($(window).width() <= mobileWidth) {
                return;
            }
            $('.global_header_btn').collapse('hide');
            resetBordForGlobalSearchBox();
        }, function () {
            if ($(window).width() <= mobileWidth) {
                return;
            }
            $('.global_header_btn').collapse('show');
        }
    ).on("click", function (event) {
        $('.global_header_btn').addClass('hide');
        $("#form_globalheadersearch input").addClass("show");
    });

    $('a.crr-emp-btn-modal-close').on('click', function (e) {
        $(".crr-header-global-navcontainer").collapse("hide");
        $('.global_header_btn').removeClass('hide');
        $("#form_globalheadersearch input").removeClass("show");
        return false;
    });
    $(document).on("click", function (event) {
        if (!document.getElementById("form_globalheadersearch").contains(event.target)) {
            $('.global_header_btn').removeClass('hide');
            $("#form_globalheadersearch input").removeClass("show");
            //if ($(event.target).parent('a.crr-emp-btn-modal-close').length > 0) {
            //    event.preventDefault();
            //   /* event.stopPropagation();*/
            //}
        }
    });
}

function CrrGlobalMenuInit() {
    $(".crr-header-nav-link").on("mouseover", function () {
        $(this).closest("li").find(".dropdown").addClass("open");
        $(this).closest("li").addClass("open");
    });
    $(".crr-header-global-nav .dropdown").on("mouseover", function () {
        $(this).addClass("open");
        $(this).closest("li").addClass("open");
    });
    $(".icon-dropdown-img").on("click", function () {
        $(this).closest("li").siblings().removeClass("open");
        $(this).closest("li").siblings().find(".dropdown").removeClass("open");
        $(this).closest("li").toggleClass("open");
        $(this).closest("li").find(".dropdown").toggleClass("open");
        headerHeight = $(".crr-emp-header-global-menu-content").height();
    });
    $(document).on("mouseover", function (event) {
        if ($(window).width() <= mobileWidth) {
            return;
        }
        $(".crr-header-global-nav .dropdown").each(function (index, element) {
            if (!element.contains(event.target) && !$(element).closest("li").find(".crr-header-nav-link")[0].contains(event.target)) {
                $(element).removeClass("open");
                $(element).closest("li").removeClass("open");
                if ($(element).find('[data-toggle="collapse"]').length > 0 && $(element).find('[data-toggle="collapse"]').attr("aria-expanded") === "true") {
                    $(element).find('[data-toggle="collapse"]').trigger("click");
                }
            }
        });
    }).on("mousedown", function (event) {
        if (!$(".crr-header-global-navcontainer")[0].contains(event.target) && event.target != $(".icon-drag-handle")[0]) {
            $(".crr-header-global-navcontainer").collapse("hide")
        }
    });

    $('.crr-header-global-navcontainer').on('shown.bs.collapse', function (event) {
        if (isMobileMenu) { // touch device
            if (!document.getElementById('globalNav')) {
                var c = document.createDocumentFragment();
                var e = document.createElement('div');
                e.className = 'globalNav__menuOverlay';
                e.id = 'globalNav';
                c.appendChild(e);
                document.body.appendChild(c);
            }
            else {
                $('#globalNav').show();
            }
            
            if (typeof ($("#ca_callout_container").visible()) === 'undefined' || $("#ca_callout_container").visible() === false) {
                $('.crr-header-global-menu .crr-header-global-navcontainer').addClass('max-height-news-none--vh');
            }
            else {
                $('.crr-header-global-menu .crr-header-global-navcontainer').addClass('max-height-news--vh');
            }
            window.dispatchEvent(new Event('resize'));
            
            $('body').addClass('overflow--hidden');
        }
    });

    $('.crr-header-global-navcontainer').on('hide.bs.collapse', function (event) {
        if (isMobileMenu) { // touch device
            $('.crr-header-global-menu .crr-header-global-navcontainer').removeClass('max-height--vh');
            $('body').removeClass('overflow--hidden');
            $('.crr-header-global-menu .crr-header-global-navcontainer').removeClass('max-height-news-none--vh');
            $('.crr-header-global-menu .crr-header-global-navcontainer').removeClass('max-height-news--vh');
            if (document.getElementById('globalNav')) {
                $('#globalNav').hide();
            }
        }

        if (!$(event.target).hasClass("crr-header-global-navcontainer")) {
            return;
        }
        let $dropdown = $(".crr-header-global-nav .dropdown.open");
        if ($dropdown.length > 0) {
            $dropdown.removeClass("open");
            $dropdown.closest("li").removeClass("open");
            if ($dropdown.find('[data-toggle="collapse"]').length > 0 && $dropdown.find('[data-toggle="collapse"]').attr("aria-expanded") === "true") {
                $dropdown.find('[data-toggle="collapse"]').trigger("click");
            }
        }
    });
}

(function ($) {
    var $w = $(window);
    $.fn.visible = function (partial, hidden, direction, container) {

        if (this.length < 1)
            return;

        // Set direction default to 'both'.
        direction = direction || 'both';

        var $t = this.length > 1 ? this.eq(0) : this,
            isContained = typeof container !== 'undefined' && container !== null,
            $c = isContained ? $(container) : $w,
            wPosition = isContained ? $c.position() : 0,
            t = $t.get(0),
            vpWidth = $c.outerWidth(),
            vpHeight = $c.outerHeight(),
            clientSize = hidden === true ? t.offsetWidth * t.offsetHeight : true;

        if (typeof t.getBoundingClientRect === 'function') {

            // Use this native browser method, if available.
            var rec = t.getBoundingClientRect(),
                tViz = isContained ?
                    rec.top - wPosition.top >= 0 && rec.top < vpHeight + wPosition.top :
                    rec.top >= 0 && rec.top < vpHeight,
                bViz = isContained ?
                    rec.bottom - wPosition.top > 0 && rec.bottom <= vpHeight + wPosition.top :
                    rec.bottom > 0 && rec.bottom <= vpHeight,
                lViz = isContained ?
                    rec.left - wPosition.left >= 0 && rec.left < vpWidth + wPosition.left :
                    rec.left >= 0 && rec.left < vpWidth,
                rViz = isContained ?
                    rec.right - wPosition.left > 0 && rec.right < vpWidth + wPosition.left :
                    rec.right > 0 && rec.right <= vpWidth,
                vVisible = partial ? tViz || bViz : tViz && bViz,
                hVisible = partial ? lViz || rViz : lViz && rViz,
                vVisible = (rec.top < 0 && rec.bottom > vpHeight) ? true : vVisible,
                hVisible = (rec.left < 0 && rec.right > vpWidth) ? true : hVisible;

            if (direction === 'both')
                return clientSize && vVisible && hVisible;
            else if (direction === 'vertical')
                return clientSize && vVisible;
            else if (direction === 'horizontal')
                return clientSize && hVisible;
        } else {

            var viewTop = isContained ? 0 : wPosition,
                viewBottom = viewTop + vpHeight,
                viewLeft = $c.scrollLeft(),
                viewRight = viewLeft + vpWidth,
                position = $t.position(),
                _top = position.top,
                _bottom = _top + $t.height(),
                _left = position.left,
                _right = _left + $t.width(),
                compareTop = partial === true ? _bottom : _top,
                compareBottom = partial === true ? _top : _bottom,
                compareLeft = partial === true ? _right : _left,
                compareRight = partial === true ? _left : _right;

            if (direction === 'both')
                return !!clientSize && ((compareBottom <= viewBottom) && (compareTop >= viewTop)) && ((compareRight <= viewRight) && (compareLeft >= viewLeft));
            else if (direction === 'vertical')
                return !!clientSize && ((compareBottom <= viewBottom) && (compareTop >= viewTop));
            else if (direction === 'horizontal')
                return !!clientSize && ((compareRight <= viewRight) && (compareLeft >= viewLeft));
        }
    };

})(jQuery);
