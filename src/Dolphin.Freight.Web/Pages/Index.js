$(function () {
    var l = abp.localization.getResource('Freight');
    $(document).ready(function () {
        const ids = [
            'CustomerPayment_ReleaseDate',
            'OceanExportMbl_PorEtd',
           
        ];

        ids.forEach(function (id) {
            let dateElem = $('#' + id);

            if (dateElem.length === 0) {
                return;
            }

            dateElem.removeAttr('type').datetimepicker({
                format: 'Y-m-d H:i',
                step: 15,
                allowInput: false
            });

            let currentVal = dateElem.val();
            if (currentVal.includes('T')) {
                dateElem.val(currentVal.replace('T', ' '));
            }
        });
    });


    tempusDominus.extend(window.tempusDominus.plugins.customDateFormat)
    $(".cdatetime").tempusDominus({
        restrictions: {
            minDate: '2023-02-10 00:00',
            maxDate: '2023-04-10 00:00'
        },
        useCurrent: true,

        localization: {
            format: 'yyyy-MM-dd HH:mm',
        }
    });


    $(".cdatetime").change(function () {
        checkDateTime($(this).attr("id"), $(this).val())
    })
})