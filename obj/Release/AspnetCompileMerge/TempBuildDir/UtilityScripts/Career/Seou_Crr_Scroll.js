var sal_is_first_time = 0;
var sal_previous_scroll_top = 0;
var sal_stop_check = 0;

/* mobile  */
function salLoadTouchEvent() {

    document.addEventListener('touchstart', salTouchEvent, false);
    document.addEventListener('touchmove', salTouchEvent, false);
    document.addEventListener('touchend', salTouchEvent, false);

    var sal_previous_pos_x = 0;
    var sal_previous_pos_y = 0;


    function salTouchEvent(e) {
        var event = e || window.event;
        var sal_current_touch_pos_x;
        var sal_current_touch_pos_y;
        var sal_changed_pos_x;
        var sal_changed_pos_y;
        var sal_is_vertical_move;
        var sal_bottom_message;
        var sal_top_message;

        switch (event.type) {
            case "touchstart":
                sal_previous_pos_x = event.touches[0].clientX;
                sal_previous_pos_y = event.touches[0].clientY;

                break;
            case "touchend":
                sal_current_touch_pos_x = event.changedTouches[0].clientX;
                sal_current_touch_pos_y = event.changedTouches[0].clientY;

                sal_changed_pos_x = sal_current_touch_pos_x - sal_previous_pos_x;
                sal_changed_pos_y = sal_current_touch_pos_y - sal_previous_pos_y;

                sal_is_vertical_move = true;
                if (Math.abs(sal_changed_pos_x) >= Math.abs(sal_changed_pos_y)) {
                    sal_is_vertical_move = false;
                }

                if (!sal_is_vertical_move && sal_changed_pos_x > 50) //Move To Right
                {
                    if ($("#hd_pj").val() !== "") {
                        setTimeout(function () {
                            var params = getPostFilterConditionParams();
                            submitDynamicFilterForm($("#hd_pj").val(), params);
                        }, 700);
                    }
                }
                else if (!sal_is_vertical_move && sal_changed_pos_x < - 50) //Move To Left
                {
                    if ($("#hd_nj").val() !== "") {
                        setTimeout(function () {
                            var params = getPostFilterConditionParams();
                            submitDynamicFilterForm($("#hd_nj").val(), params);
                        }, 700);
                    }
                }
                
                break;
            case "touchmove":
               
                sal_current_touch_pos_x = event.touches[0].clientX;
                sal_current_touch_pos_y = event.touches[0].clientY;

                sal_changed_pos_x = sal_current_touch_pos_x - sal_previous_pos_x;
                sal_changed_pos_y = sal_current_touch_pos_y - sal_previous_pos_y;

                sal_is_vertical_move = true;
                if (Math.abs(sal_changed_pos_x) >= Math.abs(sal_changed_pos_y)) {
                    sal_is_vertical_move = false;
                }

                if (!sal_is_vertical_move && sal_changed_pos_x > 50) //Move To Right
                {
                    
                }
                else if (!sal_is_vertical_move && sal_changed_pos_x < - 50) //Move To Left
                {
                    
                }
                break;
        }
    }
}
