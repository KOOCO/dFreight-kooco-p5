﻿$(function () {
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

    if ($('#' + id).closest('.modal').length) {
        $('#' + id).select2({
            placeholder: l("Dropdown:Select"),
            allowClear: true,
            dropdownParent: $('#' + id).closest('.modal')
        });
    }
    else {
        $('#' + id).select2({
            placeholder: l("Dropdown:Select"),
            allowClear: true,
        });
    }

    $(id).on('select2:open', () => {
        document.querySelector('.select2-search__field').focus();
    });

    setWidth(id, selectType);
}

function setWidth(id, selectType) {
    if (selectType == 1) {
        $('#' + id).parent().find('.select2-container').attr('style', 'max-width:calc(100%-40px) !important');
    }
}