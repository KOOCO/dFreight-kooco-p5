$(function () {
    $("#saveBtn").click(function () {
        $("#createForm").submit();
    })
})

$('#createForm').on('abp-ajax-success', function (result, rs) {
    location.href = 'EditMawb?ShowMsg=true&Id=' + rs.responseText.id

});