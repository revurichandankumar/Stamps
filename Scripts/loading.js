$.loading = {
    start: function (loadingTips='') {
        let _LoadingHtml = '<div class="spin spin-lg spin-spinning">' +
                                '<span class="tips">'+loadingTips+'</span>'+
                           '</div>'

        $('body').append(_LoadingHtml);
    },
    end: function () {
        $(".spin").remove();
    }
}
