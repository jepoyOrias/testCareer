for (var i = 0; i < document.querySelectorAll("div[class^='crr-custom-select-wrapper']").length; i++) {
    var dropdown = document.querySelectorAll("div[class^='crr-custom-select-wrapper']")[i];
    var target = dropdown.querySelector('div[class^="crr-custom-select-target"]');
    var options = dropdown.querySelector('div[class^="crr-custom-options"]');
    if (target.getBoundingClientRect().left + options.getBoundingClientRect().width > $(window).width()) {
        options.style.right = "0";
        if (target.getBoundingClientRect().left + target.getBoundingClientRect().width < options.getBoundingClientRect().width) {
            options.style.left = -target.getBoundingClientRect().left + "px";
        } else {
            options.style.left = "auto";
        }
    } else {
        options.style.left = "0";
        options.style.right = "auto";
    }
    dropdown.addEventListener('click', function (event) {
        target = this.querySelector('div[class^="crr-custom-select-target"]');
        target.classList.toggle('open');
        options = this.querySelector('div[class^="crr-custom-options"]');
        if (target.classList.contains('open')) {
            if (target.getBoundingClientRect().top + target.getBoundingClientRect().height + options.getBoundingClientRect().height > $(window).height()) {
                options.style.bottom = "100%";
                options.style.top = "auto";
            } else {
                options.style.top = "100%";
                options.style.bottom = "auto";
            }
            if (target.getBoundingClientRect().left + options.getBoundingClientRect().width > $(window).width()) {
                options.style.right = "0";
                if (target.getBoundingClientRect().left + target.getBoundingClientRect().width < options.getBoundingClientRect().width) {
                    options.style.left = -target.getBoundingClientRect().left + "px";
                } else {
                    options.style.left = "auto";
                }
            } else {
                options.style.left = "0";
                options.style.right = "auto";
            }
        }
    });

    dropdown.setAttribute("tabindex", -1);
    var $item;
    dropdown.addEventListener('keydown', function (event) {
        if ($(event.target).find(".crr-custom-select-target").hasClass("open")) {
            if (event.keyCode >= 65 && event.keyCode <= 90) {
                var $target = $(event.target).find(".crr-custom-options");
                var selectedIndex = 0;
                var selectedItem = $target.find(".selected")[0];
                var $items = $target.find(".crr-custom-option").filter(function (index, element) {
                    return $(element).text().trim().toUpperCase().startsWith(String.fromCharCode(event.keyCode));
                });
                selectedIndex = $items.toArray().indexOf(selectedItem);
                if (selectedIndex !== $items.length - 1) {
                    $item = $($items[selectedIndex + 1]);
                } else {
                    $item = $items.first();
                }
                if ($item.length > 0) {
                    $item.addClass("selected");
                    $item.siblings().removeClass("selected");
                    $item.closest('div[class^="crr-custom-select-target"]').find('div[class^="crr-custom-select__trigger"] span').text($item.text());
                    if ($item.position().top <= 0 || $item.position().top >= $target.height() - $item.height()) {
                        $target.animate({ scrollTop: $item.position().top + $target.scrollTop() }, 0);
                    }
                }
            }
            if (event.keyCode === 13) {
                $item.trigger("click");
            }
        }
    });
}

for (var i = 0; i < document.querySelectorAll("span[class^='crr-custom-option']").length; i++) {
    var option = document.querySelectorAll("span[class^='crr-custom-option']")[i];
    option.addEventListener('click', function () {
        if (!this.classList.contains('selected')) {
            var selected = this.parentNode.querySelector('span[class^="crr-custom-option"].selected');
            if (selected !== null) {
                selected.classList.remove('selected');
                this.classList.add('selected');
                $(this).closest('div[class^="crr-custom-select-target"]').find('div[class^="crr-custom-select__trigger"] span')[0].textContent = this.textContent;
            } else {
                this.classList.add('selected');
                $(this).closest('div[class^="crr-custom-select-target"]').find('div[class^="crr-custom-select__trigger"] span')[0].textContent = this.textContent;
            }
        }
        $item = $(this);
    });
}

window.addEventListener('click', function (e) {
    for (var i = 0; i < document.querySelectorAll('div[class^="crr-custom-select-target"]').length; i++) {
        var select = document.querySelectorAll('div[class^="crr-custom-select-target"]')[i];
        if (!select.contains(e.target)) {
            if (select.classList.contains('open')) {
                if ($item && $item.length > 0) {
                    if (select.contains($item[0])) {
                        $item.trigger("click");
                    }
                }
            }
            select.classList.remove('open');
        }
    }
});