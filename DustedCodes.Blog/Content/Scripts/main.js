﻿if (!window.dustedcodes) {
    dustedcodes = {};
}

(function ($) {
    window.dustedcodes = {
        window: {
            open: function (url, width, height) {
                width = (typeof width === "undefined") ? 700 : width;
                height = (typeof height === "undefined") ? 300 : height;
                window.open(url, "_blank", "width=" + width + ", height=" + height);
            }
        },
        menu: {
            toggle: function () {
                $("nav").slideToggle("fast", function () {
                    if ($(this).css("display") === "none") {
                        $(this).removeClass("nav-expanded").addClass("nav-collapsed").removeAttr("style");
                    } else {
                        $(this).removeClass("nav-collapsed").addClass("nav-expanded").removeAttr("style");
                    }
                });
            }
        }
    };
    $(document).on("click", "#nav-toggle-button", function () {
        dustedcodes.menu.toggle();
    });
})(jQuery);