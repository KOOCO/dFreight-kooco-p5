$(function () {
    $("#saveBtn").click(function () {
        $('#createForm').valid();
        var validate = $('#createForm').validate();
        var mawbErrors = validate.errorList.filter((error) => error.element.name.includes('MawbModel')); 
        if (mawbErrors.length && $('#btnMawbCollapse').hasClass('collapsed')) {
            $('#btnMawbCollapse').click();
            setTimeout(() => {
                $('#createForm').submit();
            }, 500)
        }
        else {
            $('#createForm').submit();
        }
    })
});


$('#createForm').on('abp-ajax-success', function (result, rs) {
    location.href = 'EditModal?ShowMsg=true&Id=' + rs.responseText.id
});
