class EditExportBookings {
    static convertUnit(value, module) {
        switch (module) {
            case "NetWeight":
                if (value === 'LB') {
                    for (var i = 0; i <= $(".netWeight").length - 1; i++) {
                        $('input[name="Commodities[' + i + '].NetWeight"]').val(parseFloat($('input[name="Commodities[' + i + '].NetWeight"]').val() * 2.20462).toFixed(2));
                    }
                }
                else if (value === 'KG') {
                    for (var i = 0; i <= $(".netWeight").length - 1; i++) {
                        $('input[name="Commodities[' + i + '].NetWeight"]').val(parseFloat($('input[name="Commodities[' + i + '].NetWeight"]').val() / 2.20462).toFixed(2));
                    }
                }
                break;
            case "GrossWeight":
                if (value === 'LB') {
                    for (var i = 0; i <= $('.grossWeight').length - 1; i++) {
                        $('input[name="Commodities[' + i + '].GrossWeight"]').val(parseFloat($('input[name="Commodities[' + i + '].GrossWeight"]').val() * 2.20462).toFixed(2));
                    }
                }
                else if (value === 'KG') {
                    for (var i = 0; i <= $('.grossWeight').length - 1; i++) {
                        $('input[name="Commodities[' + i + '].GrossWeight"]').val(parseFloat($('input[name="Commodities[' + i + '].GrossWeight"]').val() / 2.20462).toFixed(2));
                    }
                }
                break;
        }
    }
}