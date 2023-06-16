
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

function initializeDropdownSearch(id) {
    var l = abp.localization.getResource('Freight');
    $('#' + id).select2({
        placeholder: '- Select -',
        templateSelection: getFormat(),
        templateResult: getFormat()
    });
}