$(document).on('change', '[name=PackagingUnit]', function () {
    $('#totalPackageTypeUnit').text($('#PackagingUnit').find('option:selected').text());
});
