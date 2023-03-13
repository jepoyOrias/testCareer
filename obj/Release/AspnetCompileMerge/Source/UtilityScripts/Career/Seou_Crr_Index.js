$(document).ready(function () {
    fndJobTypeaheadInit();
    locationTypeaheadInit();
    $(".crr-input-searchtype input[type='radio']:eq(0)").prop("checked", true);
    $(".crr-input-searchtype input[type='radio']:eq(0)").siblings().prop("checked", false);

   toggleUserTabs();
    
});

function toggleUserTabs() {
    $('.crr-user-tabs a').click(function (e) {
        e.preventDefault()
        $('.crr-user-tabs li').removeClass("active");
        $(this).parent().addClass("active");

        $(".crr-user-tabs .tab-pane").removeClass("active");
        var tabContentID = "#" + $(this).attr("aria-controls");
        $(tabContentID).addClass("active");
    });
}

function toggleSeeMoreMobile(el) {
    $(".crr-adv-card").removeClass("active");
    $(".crr-adv-header").removeClass("active");
    $(".crr-jobs-list-wrapper").addClass("collapse-on-mobile")
    var parent = $(el).closest(".crr-adv-card").addClass("active");
    $(el).prev(".crr-adv-header").addClass("active")
    $(parent).find(".crr-jobs-list-wrapper").removeClass("collapse-on-mobile");
    $(".crr-seemore-mobile").show();
    $(el).hide();
}

function bookMark(el) {
    $(el).toggleClass("isBookmarked");
    $(el).empty();

    if ($(el).hasClass("isBookmarked")){
        $(el).append('<img  src="Career/Images/icon-bookmark-solid.svg"/>');
    } else {
        $(el).append('<span class="icon-crr-bookmark text-black text-size18" ></span>');
    }
    return false;
}