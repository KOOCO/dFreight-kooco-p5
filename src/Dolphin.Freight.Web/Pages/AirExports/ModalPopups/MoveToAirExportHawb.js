$('#moveForm').on('abp-ajax-success', function (result, rs) {
    var url = new URL(window.location.href);

    location.href = url + '&ResultedMawbId=' + rs.responseText.MawbId + '&MovedHawbMessage=True';
});