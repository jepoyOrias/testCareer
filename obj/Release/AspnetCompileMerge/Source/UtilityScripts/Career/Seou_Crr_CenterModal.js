(function ($) {
    $(document).on('shown.bs.modal', '.modal', centerModal);
    $(document).on('hidden.bs.modal', '.modal', function () {
        $(this).hide();
    });

    var caResizeTimeout;
    $(window).on("resize", function () {
        if (caResizeTimeout) {
            clearTimeout(caResizeTimeout);
        }
        caResizeTimeout = setTimeout(function () {
            $('.modal:visible').each(centerModal);
        }, 150);
    });

}(jQuery));

function centerModal() {
	var $modal_dialog = $(this).find(".modal-dialog");
	$(this).show();
	resizeModal($modal_dialog);	
}

function resizeModal($modal_dialog) {
    var windowHeight = $(window).height();
    var modalHeight = $modal_dialog.height();

    $modal_dialog.css({ 'margin-top': Math.max(0, (windowHeight - modalHeight) / 2) });

    // prevent the Close button where it's position at the top of modal under the browser invisible area.
    if (windowHeight < modalHeight) {
        $modal_dialog.css("top", "0");
    }
}

