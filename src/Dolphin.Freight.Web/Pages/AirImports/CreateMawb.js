$(function () {
    $("#saveBtn").click(function () {
        debugger;
        $("#createForm").submit();
    })
})

$('#createForm').on('abp-ajax-success', function (result, rs) {
    location.href = 'EditMawb?ShowMsg=true&Id=' + rs.responseText.id

});