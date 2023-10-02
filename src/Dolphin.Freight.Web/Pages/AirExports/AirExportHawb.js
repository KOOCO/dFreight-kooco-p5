class AirExportHawb {
    static KgLbConversion(Elem, Unit) {
        debugger;
        switch (Unit) {
            case 'KG':
                var LB = parseFloat(Elem.currentTarget.value * 2.2).toFixed(2);
                $($(Elem.currentTarget).parent().parent().next().children().children()[0]).val(LB);
                break;
            case 'LB':
                var KG = parseFloat(Elem.currentTarget.value / 2.2).toFixed(2);
                $($(Elem.currentTarget).parent().parent().prev().children().children()[0]).val(KG);
                break;
        }
    }
}