import jQuery from 'jquery';

(function ($) {
    $.fn.extend({
        "upload": function (url, callback) {
            let that = this;
            if (this.val()) {
                let formData = new FormData(this.parents("form")[0]);
                $.ajax({
                    async: true,
                    cache: false,
                    contentType: false,
                    processData: false,
                    type: "POST",
                    dataType: "JSON",
                    url: url,
                    data: formData,
                    error: function (e) {
                        alert(e);
                        that.val();
                    },
                    success: function (data) {
                        that.val("");
                        if (jQuery.isFunction(callback)) {
                            callback.call(null, data);
                        }
                    }
                });
            }
        }
    });
})(jQuery);