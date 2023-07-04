$(function () {
    var l = abp.localization.getResource('Freight');

});

function getFormat() {
    const optionFormat = (item) => {
        if (!item.id) {
            return item.text;
        }

        var span = document.createElement('span');
        var template = '';

        template += '<div class="d-flex align-items-center">';

        template += '<div class="d-flex flex-column">'
        template += '<span class="fs-6 lh-1">' + item.text + '</span>';
        template += '</div>';
        template += '</div>';

        span.innerHTML = template;

        return $(span);
    }

    return optionFormat;
}

function initializeDropdownSearch(id, selectType) {

    var l = abp.localization.getResource('Freight');
    $('#' + id).select2({
        placeholder: l("Dropdown:Select"),
        allowClear: true
    });

    setWidth(id, selectType);
}

function setWidth(id, selectType) {
    console.log(id + ' ' + selectType);
    if (selectType == 1) {
        $('#' + id).parent().find('.select2-container').attr('style', 'width:calc(100% - 50px) !important')
    }
    else {
        $('#' + id).parent().find('.select2-container').attr('style', 'width:calc(100%) !important')
    }
}