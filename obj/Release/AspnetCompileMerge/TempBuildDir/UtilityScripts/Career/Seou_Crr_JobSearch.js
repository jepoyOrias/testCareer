$(document).ready(function () {
    toggleFiltersTabs();
    toggleFiltersMobile();
    onHoverFilterExpand();
    toggleSubMenu();
    toggleStickySearchResults();
    toggleFixedFilterMobile();

    //For Desktop//
    selectFilters();
    filtersSelectedCount();
    filtersSelectedCount(true);
    $("#crr-collpase-search").on("hidden.bs.collapse", function () {
        $(".crr-back-search-arrow").addClass("hidden");
    });

    $(".crr-job-listing-filters-tabs").on("shown.bs.tab", function (e) {
        var target = e.target;
        selectFilters();
        $(target).trigger("click");
       
    });

    $(".crr-global-filters").on("shown.bs.collapse", function (e) {
       
        selectFiltersMobile(e.target);
    });

    //Initializing Badge For Mobile to hide;
    $(".crr-job-filter").find(".badge").hide();
});

function toggleFiltersTabs() {
    $('.crr-job-listing-filters-tabs a').click(function (e) {
        e.stopPropagation();
        var tabContentID = "#" + $(this).attr("aria-controls");
        //$('.crr-job-listing-filters-tabs li').removeClass("active");
        //$(this).parent().addClass("active");
        //$(".crr-job-listing-filters-tabs .tab-pane").removeClass("active");
        $(this).tab('show')
        $(tabContentID).addClass("active");
        //When user click tab we will shrink the expandable
        var wrapper = $(this).parents().find(".crr-layout");
        var height = $(tabContentID).height() + 110 - 30;
        $(wrapper).css('height', height);
    }); 
}

function toggleFiltersMobile() {
    $("#crr-filter-absolute  li").click(function (e) {
        e.stopPropagation();
        $(this).toggleClass("open");
    });
}

function onHoverFilterExpand() {
    var wrapper;
    $(".crr-joblisting").on('mouseenter', function (e) {
        wrapper = $(this).find(".crr-layout");
        var height = $(".tab-content").height() + 110 - 30;
        $(wrapper).css('height', height);
    }).on('mouseleave', function () {
        $(wrapper).css('height', '112');
    });
}

function toggleSubMenu() {
    $(".sa-btn-collapse").on("click", function () {
        var collapsemenu = $(this).data("target");
        $(collapsemenu).on("shown.bs.collapse", function () {
            $(this).prev().find("img").attr("src", "/Images/icon-arrow-up-circle.svg");

        });

        $(collapsemenu).on("hide.bs.collapse", function () {
            $(this).prev().find("img").attr("src", "/Images/icon-arrow-down-circle.svg");
        });

        if ($(this).hasClass("crr-sr-collapse")) {
            $(this).toggleClass("crr-sr-collapse-close");
            $(this).parent().toggleClass("border-bottom");
            $(this).parent().toggleClass("margin-bottom20");
        }
    });

    $(".crr-sub-menu .sa-btn-collapse").on("click", function () {
        $(this).next().toggleClass("in");
    }); 
}

function toggleSearchOnMobile() {
    $(".crr-back-search-arrow").toggleClass("hidden");
}

function toggleStickySearchResults() {
    $(".crr-sr-content").on("scroll", function () {
        var scroll = $(this);
        if ($(scroll).scrollTop() != 0) {
            $(".crr-sr-main-header-wrapper").addClass("crr-sr-scroll");
            $(".collapsable-on-scroll").addClass("hide-on-scroll");
        }
        else {
            $(".crr-sr-main-header-wrapper").removeClass("crr-sr-scroll");
            $(".collapsable-on-scroll").removeClass("hide-on-scroll");
        }
    });
}

function toggleFixedFilterMobile() {  
    $("#crr-filter-absolute").on("hidden.bs.collapse", function () {
        $(".crr-filter-search-sticky").css("z-index", 100);
    });

    $("#crr-filter-absolute").on("show.bs.collapse", function () {
        $(".crr-filter-search-sticky").css("z-index", 1003);    
    });
}

function selectFilters() {
    var activeID = $(".tab-pane.active").attr("id");
    var btns = $(".tab-pane.active").find(".btn");
    var aLinks = $(".crr-job-listing-filters").find("a[aria-controls=" + activeID + "]");

    $(btns).each(function () {
        //Need to unbind the click to make it clickable again//
        $(this).unbind().click(function (e) {

            e.preventDefault();
            $(this).toggleClass("active");

            //Check all active btns
            var activeBtns = $(".tab-pane.active").find(".btn.active");
            //Check if active btns is greater than 0
            if ($(activeBtns).length >= 1) {
                $(aLinks).addClass("hasFilter");
            } else {
                $(aLinks).removeClass("hasFilter");
            }
            //Call the filter Counts function
            filtersSelectedCount();
        })
     });
}

function selectFiltersMobile(activeCollapse) {
    var btns = $(activeCollapse).find(".btn");
   
    $(btns).each(function () {
        //Need to unbind the click to make it clickable again//
        $(this).unbind().click(function (e) {
            var isMobile = true;

            e.preventDefault();
            $(this).toggleClass("active");
            if ($(this).hasClass("crr-remove-hover")) {
                $(this).removeClass("crr-remove-hover");
            } else {
                $(this).addClass("crr-remove-hover");
            }
          
            //In Mobile need remove the hover Effect

            //Check all active btns
            var activeBtns = $(activeCollapse).find(".btn.active");
            //Check if active btns is greater than 0
            var activeBtnCounts = $(activeBtns).length
            var badge = $(this).parents().closest(".crr-job-filter").find(".badge");
            if (activeBtnCounts >= 1) {
                $(badge).show().text(activeBtnCounts);
            } else {
                $(badge).hide().text(activeBtnCounts);
            }
            //Call the filter Counts function
            filtersSelectedCount(isMobile);
            $(this).blur();
        })
    });
}

function filtersSelectedCount(isMobile = false) {
    var selectedFilterCounts = $(".tab-pane").find(".btn.active").length;
    var selectedFilterCountsMobile = $(".crr-job-filter").find(".btn.active").length;
    
    if (!isMobile) {      
        if (selectedFilterCounts >= 1) {
            $(".crr-filter-count")[0].firstChild.data = selectedFilterCounts;
            $(".crr-filter-count").show();
        } else {
            $(".crr-filter-count").hide();
        } 
    } else {      
        if (selectedFilterCountsMobile >= 1) {
            console.log($(".crr-filter-header > .crr-filter-count")[0].firstChild.data)
            $(".crr-filter-header > .crr-filter-count")[0].firstChild.data = selectedFilterCountsMobile;
            $(".crr-filter-header > .crr-filter-count").show();
        } else {
            $(".crr-filter-header > .crr-filter-count").hide();
        }
    }

}

function removeFilters() {
    var btns = $(".tab-pane").find(".btn");
    var btnsMobile = $(".crr-job-filter").find(".btn");
    var tabLinks = $(".crr-job-listing-filters").find("a");
    $(btns).removeClass("active");
    $(btnsMobile).removeClass("active");
    $(tabLinks).removeClass("hasFilter");
    filtersSelectedCount();
}