
$(document).ready(function () {
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

function previewImage() {
    var file = document.getElementById("file").files;
    if (file.length > 0) {
        var fileReader = new FileReader();
        fileReader.onload = function (event) {
            document.getElementById("preview").setAttribute("src", event.target.result);
        };
        fileReader.readAsDataURL(file[0]);
    }
}

$(function () {
    $(".crr-my-settings-container").find(".crr-my-settings-container-item").each(function () {
        let upIcon = $(this).find('.crr-up-icon');
        let downIcon = $(this).find('.crr-down-icon');
        let containerMain = $(this).find('.crr-container-main');
        let boderStyle = $(this).find('.crr-change-boder-style');

        upIcon.click(function () {
            upIcon.css("display", "none");
            downIcon.show();
            containerMain.css("display", "none");
            boderStyle.removeClass("boder-style");
        });

        downIcon.click(function () {
            $('.crr-up-icon').css("display", "none");
            $('.crr-down-icon').show();
            $('.crr-container-main').css("display", "none");
            $('.crr-change-boder-style').removeClass("boder-style");

            upIcon.show();
            downIcon.css("display", "none");
            containerMain.show();
            boderStyle.addClass("boder-style");
        });
    });

    $(".crr-job-match-item").find("div").each(function () {
        let upCircleIcon = $(this).find('.crr-circle-up-icon');
        let downCircleIcon = $(this).find('.crr-circle-down-icon');
        let containerItem = $(this).find('.crr-container-item');

        upCircleIcon.click(function () {
            upCircleIcon.css("display", "none");
            downCircleIcon.show();
            containerItem.css("display", "none");
        });

        downCircleIcon.click(function () {
            downCircleIcon.css("display", "none");
            upCircleIcon.show();
            containerItem.show();
        });
    });

    $(".crr-job-match-item").find("div").each(function () {
        let onButton = $(this).find('.crr-on-button');
        let offButton = $(this).find('.crr-off-button');

        onButton.click(function () {
            offButton.show();
            onButton.css("display", "none");
        });

        offButton.click(function () {
            offButton.css("display", "none");
            onButton.show();
        });
    });
});

function showJobDetails() {
    $('#crr-saved-jobs').removeClass("active");
    $('#crr-saved-jobs-detail').addClass("active");
};

function showJobList() {
    $('#crr-saved-jobs-detail').removeClass("active");
    $('#crr-saved-jobs').addClass("active");
};