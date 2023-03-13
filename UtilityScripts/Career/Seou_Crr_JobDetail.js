$(document).ready(function () {
    fndJobTypeaheadInit();
    locationTypeaheadInit();
    bindClickForJobListing();
    initSwiperOnMobile();
    prevNextJobInit();
    stickyNavigationInit();
    skillsFilterInit();
    if ($(".crr-noresults").length > 0) {
        $(".crr-layout-2a-a").append('<span class="crr-noresultscrolltotop-icon collapse" onclick="document.documentElement.scrollTop = 0;"><img src="/Images/icon-to_top.svg" /></span>');
    } 
});

function initSwiperOnMobile() {
    //Mobile: Career Path slide.
    if ($('.swiper-container').length > 0) {
        var swiper = new Swiper({
            el: '.swiper-container',
            initialSlide: 0,
            spaceBetween: 5,
            slidesPerView: 1,
            centeredSlides: true,
            slideToClickedSlide: true,
            grabCursor: true,
            scrollbar: {
                el: '.swiper-scrollbar',
            },
            mousewheel: {
                enabled: true,
            },
            keyboard: {
                enabled: true,
            },
            pagination: {
                el: '.swiper-pagination',
            },
        });
    }
}

function bindClickForJobListing() {
    document.querySelectorAll('.crr-joblisting-wrapper').forEach(item => {
        item.addEventListener('click', event => {
            var action = event.currentTarget.getAttribute('href');
            event.preventDefault();
            // set keyword
            var params = getPostFilterConditionParams();
            submitDynamicFilterForm(action, params);
        })
    });


    //let loadMoreEleFromHttpGet = document.getElementById('crr-loadmore-httpget');
    //if (loadMoreEleFromHttpGet !== null) {
    //    loadMoreEleFromHttpGet.addEventListener('click', event => {
    //        event.preventDefault();
    //        document.getElementById('hd_p').value = 2;
    //        var params = getPostFilterConditionParams();
    //        var action = event.currentTarget.getAttribute('href');
    //        submitDynamicFilterForm(action, params);
    //    });
    //}

    let loadMoreEleFromHttpPost = document.getElementById('crr-btn-loadmore');
    if (loadMoreEleFromHttpPost !== null) {
        loadMoreEleFromHttpPost.addEventListener('click', event => {
            event.preventDefault();
            let hd_p = document.getElementById('hd_p').value;
            if (hd_p !== null && hd_p !== "") {
                document.getElementById('hd_p').value = parseInt(hd_p) + 1;
            }
            else {
                document.getElementById('hd_p').value = 1;
            }
            var params = getFilterConditionParams();
            var action = event.currentTarget.getAttribute('href');
            submitDynamicFilterForm(action, params, "get");
        });
    }
}

function getPostFilterConditionParams() {
    const params = {};
    let target;

    let inputEles = document.querySelectorAll("input[name^=hd_]");
    for (var i = 0; i < inputEles.length; i++) {
        target = inputEles[i];
        if (target.value.trim().length > 0) {
            params[target.id.substr(3)] = target.value;
        }
    }

    return params;
}

function getFilterConditionParams() {
    const params = {};
    let target;

    let inputEles = document.querySelectorAll("#crr-filter-params input[name^=hd_]");
    for (var i = 0; i < inputEles.length; i++) {
        target = inputEles[i];
        if (target.value.trim().length > 0) {
            params[target.id.substr(3)] = target.value;
        }
    }

    return params;
}

function submitDynamicFilterForm(path, params, method) {
    method = method || "post";

    var form = document.createElement("form");
    form.setAttribute("method", method);
    form.setAttribute("action", path);

    for (var key in params) {
        if (params.hasOwnProperty(key)) {
            var hiddenField = document.createElement("input");
            hiddenField.setAttribute("type", "hidden");
            hiddenField.setAttribute("name", key);
            hiddenField.setAttribute("value", params[key]);

            form.appendChild(hiddenField);
        }
    }

    document.body.appendChild(form);
    form.submit();
}

function showMoreCompanyDesc() {
    var dots = document.getElementById("companydescdots");
    var moreText = document.getElementById("companydescmore");
    var btnText = document.getElementById("btnMoreCompanyDesc");
    dots.style.display = "none";
    btnText.innerHTML = "";
    moreText.style.display = "inline";
}
$(".carr-company-overview-head a").on('click', function () {
    //transform: rotate(180deg);
    if ($(".carr-company-overview-body").hasClass('in')) {
        $(this).children().css("transform", "rotate(180deg)");
    } else {
        $(this).children().css("transform", "");
    }
})
$(".carr-skill-head a").on('click', function () {
    //transform: rotate(180deg);
    if ($(".carr-skill-body").hasClass('in')) {
        $(this).children().css("transform", "rotate(180deg)");
    } else {
        $(this).children().css("transform", "");
    }
})
$(".carr-career-path-head a").on('click', function () {
    //transform: rotate(180deg);
    if ($(".carr-career-path-body").hasClass('in')) {
        $(this).children().css("transform", "rotate(180deg)");
    } else {
        $(this).children().css("transform", "");
    }
})
$(".carr-howtobecome-head a").on('click', function () {
    //transform: rotate(180deg);
    if ($(".carr-howtobecome-body").hasClass('in')) {
        $(this).children().css("transform", "rotate(180deg)");
    } else {
        $(this).children().css("transform", "");
    }
})

function resetSelectedSkills() {
    $('input[name="skills"]').prop("checked", false);
    changeSelectedSkill();;
    event.stopPropagation();
}

function changeSelectedSkill() {
    let selectedskill = '';
    if ($('input[name="skills"]:checked').length > 0) {
        $(".crr-allskills-filter").removeClass("filters-selected");
        selectedskill = $('input[name="skills"]:checked').map((index, element) => { return element.value; }).toArray().join('|||');
    } else {
        $(".crr-allskills-filter").addClass("filters-selected");
    }
    $("#hd_sk").val(selectedskill);
}

function searchJobBySkills() {
    let params = getFilterConditionParams();
    let url = settings.baseUrl + 'jobs';
    for (var key in params) {
        if (params.hasOwnProperty(key)) {
            url = Helper.updateUrlParameter(url, key, params[key]);
        }
    }
    window.location.href = url;
}

function applyJob(url) {
    open(url);
}

function seeMoreResults(obj) {
    $(obj).hide();
    $(".crr-column-right").hide();
    $(".crr-joblisting,.crr-find-job-wrapper,.crr-column-left,.crr-filter-title").show();
    $(".crr-filter-title").css("display", "flex");
    document.documentElement.scrollTop = 0;
}

function prevNextJobInit() {
    if ($("#hd_pj").val() === "") {
        $(".crr-updatejob-arrow .left").remove();
    }
    if ($("#hd_nj").val() === "") {
        $(".crr-updatejob-arrow .right").remove();
    }
    $(".crr-updatejob-arrow .left").on("click", function () {
        var params = getPostFilterConditionParams();
        submitDynamicFilterForm($("#hd_pj").val(), params);
    });
    $(".crr-updatejob-arrow .right").on("click", function () {
        var params = getPostFilterConditionParams();
        submitDynamicFilterForm($("#hd_nj").val(), params);
    });
}
function getBodyScrollTop() { const el = document.scrollingElement || document.documentElement; return el.scrollTop }
var touchmoveEventOn = false;
function stickyNavigationInit() {
    document.addEventListener('touchmove', function (e) {
        touchmoveEventOn = true;
        if ($(".carr-detail-info").is(":visible")) {
            const targetBoundingClientRect = $(".carr-detail-info")[0].getBoundingClientRect();
            const headerBoundingClientRect = $(".crr-emp-global-gradient")[0].getBoundingClientRect();
            var test = getBodyScrollTop();
            if (targetBoundingClientRect.top <= 0 && test >= headerBoundingClientRect.height) {
                $(".carr-detail-info").addClass("is-pinned");
            } else {
                $(".carr-detail-info").removeClass("is-pinned");
            }
        }
        let section;
        let sectionBoundingClientRect;
        $(".crr-sticky-nav a").each((index, element) => {
            section = document.querySelector($(element).attr("href"));
            if (!!section) {
                sectionBoundingClientRect = section.getBoundingClientRect();
                if (sectionBoundingClientRect.top <= 10) {
                    $(element).addClass("selected");
                    $(element).siblings().removeClass("selected");
                }
            }
        });

    }, { passive: true });
    window.addEventListener('scroll', function (e) {
        if (!touchmoveEventOn) {
            if ($(".carr-detail-info").is(":visible")) {
                const targetBoundingClientRect = $(".carr-detail-info")[0].getBoundingClientRect();
                const headerBoundingClientRect = $(".crr-emp-global-gradient")[0].getBoundingClientRect();
                if (targetBoundingClientRect.top <= 0 && document.documentElement.scrollTop >= headerBoundingClientRect.height) {
                    $(".carr-detail-info").addClass("is-pinned");
                } else {
                    $(".carr-detail-info").removeClass("is-pinned");
                }
            }
            let section;
            let sectionBoundingClientRect;
            $(".crr-sticky-nav a").each((index, element) => {
                section = document.querySelector($(element).attr("href"));
                if (!!section) {
                    sectionBoundingClientRect = section.getBoundingClientRect();
                    if (sectionBoundingClientRect.top <= 10) {
                        $(element).addClass("selected");
                        $(element).siblings().removeClass("selected");
                    }
                }
            });
        }
    });
    $(".crr-sticky-nav a").each((index, element) => {
        if (!document.querySelector($(element).attr("href"))) {
            $(element).remove();
        }
    });
    $(".crr-sticky-nav a").on("click", function (event) {
        $(event.target).closest("a").addClass("selected");
        $(event.target).closest("a").siblings().removeClass("selected");
    });
}

function skillsFilterInit() {
    $(".crr-skill-searchinput input").on("input", function (event) {
        const keyword = event.target.value.toLocaleLowerCase();
        $(".crr-skill-items li").removeClass("hide");
        $(".crr-skill-items li").css("display", "");
        $(".crr-skill-items")[0].scrollTop = 0;
        if (keyword.length > 0) {
            const excludeItems = $(".crr-skill-items li:not(.crr-no-skills)").filter((index, element) => { return element.innerText.toLocaleLowerCase().indexOf(keyword) == -1; });
            excludeItems.addClass("hide");
            $(".crr-skill-items li:not(:visible):not(.hide):not(.crr-no-skills)").slice(0, 20).show();
            $(event.target).siblings(".icon-crr-remove").show();
            if (excludeItems.length >= $(".crr-skill-items li:not(.crr-no-skills)").length) {
                $(".crr-no-skills").show();
            } else {
                $(".crr-no-skills").hide();
            }
        } else {
            $(".crr-skill-items li").removeClass("hide");
            $(event.target).siblings(".icon-crr-remove").hide();
            $(".crr-no-skills").hide();
        }
    });
    $(".crr-skill-searchinput .icon-crr-remove").on("click", function (event) {
        $(".crr-skill-items")[0].scrollTop = 0;
        $(".crr-skill-searchinput input").val("");
        $(".crr-skill-items li").css("display", "");
        $(".crr-skill-items li").removeClass("hide");
        $(".crr-no-skills").hide();
        $(event.target).hide();
    });
    $(".crr-skill-items").on("scroll", function (event) {
        event.target.scrollHeight - event.target.clientHeight;
        if ($(".crr-skill-items")[0].scrollHeight - $(".crr-skill-items")[0].clientHeight - event.target.scrollTop <= 50) {
            $(".crr-skill-items li:not(:visible):not(.hide):not(.crr-no-skills)").slice(0, 20).show();
        };
    });

    $("#crr-skill-dropdown").on("hide.bs.dropdown", function () {
        $(".crr-skill-searchinput .icon-crr-remove").trigger("click");
    });

    $("#crr-skill-dropdown").on("show.bs.dropdown", function () {
        if ($(".crr-skill-items").html().trim() === "") {
            getSkillsFIlter();
        }
    });
}

function getSkillsFIlter() {
    $.ajax({
        url: CRR_REQUESTDATA_API.GetSkillsFilter,
        data: {
            strSelectedSkills: $("#hd_sk").val()
        },
        type: "post",
        cache: false,
        beforeSend: function () {
            $(".crr-skill-items").html("<li class='text-center'><img src='" + settings.baseUrl + "Styles/Career/img/Loading.gif'" + " /></li>");
        },
        success: function (data) {
            $(".crr-skill-items").html(data);
            $(".crr-skill-items input[name='skills']").on("click", changeSelectedSkill);
        }
    });
}