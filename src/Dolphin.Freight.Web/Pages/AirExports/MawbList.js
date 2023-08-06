$(function () {
    var l = abp.localization.getResource('Freight');
    var _changeInterval = null;
    var queryListFilter = function () {
        return {
            search: $("input[name='Search'").val()
        };
    };
    var dataTable = $('#MawbListTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[2, "asc"]],
            searching: false,
            scrollX: true,
            processing: true,
            responsive: {
                details: {
                    type: 'column'
                }
            },
            ajax: abp.libs.datatables.createAjax(dolphin.freight.importExport.airExports.airExportMawb.getList, queryListFilter),
            columnDefs: [
                //{
                //    className: 'dtr-control',
                //    orderable: false,
                //    "defaultContent": ""
                //},
                {
                    title: l('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Edit'),
                                    //visible: abp.auth.isGranted('BookStore.Books.Edit'),
                                    action: function (data) {
                                        window.location = "\EditModal?ShowMsg=true&Id=" + data.record.id;
                                    }
                                }
                            ]
                    }
                },
                {
                    title: l('FileNo'),
                    data: "filingNo",
                    targets: 0
                },
                {
                    title: l('MawbNo'),
                    data: "mawbNo"
                },
                {
                    title: l('DepatureDate'),
                    data: "depatureDate",
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            }).toFormat('yyyy-MM-dd HH:mm');
                    }
                },
                {
                    title: l('ArrivalDate'),
                    data: "arrivalDate",
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            }).toFormat('yyyy-MM-dd HH:mm');
                    }
                },
                {
                    title: l('Depature'),
                    data: "depatureAirportName"
                },
                {
                    title: l('Destination'),
                    data: "destinationAirportName"
                },
                {
                    title: l('FlightNo'),
                    data: "flightNo"
                }
                //{
                //    title: l('AR Balance'),
                //    data: ""
                //},
                //{
                //    title: l('A/P Balance'),
                //    data: ""
                //},
                //{
                //    title: l('D/C Balance'),
                //    data: ""
                //},
                //{
                //    title: l('Sales'),
                //    data: "sales"
                //},
                //{
                //    title: l('OP'),
                //    data: "oP"
                //}
            ]
        })
    );

   
    $('#Search').keyup(function () {
        clearInterval(_changeInterval)
        _changeInterval = setInterval(function () {
            dataTable.ajax.reload();
            clearInterval(_changeInterval)
        }, 1000);
    });
    

    $('#AddMawbButton').click(function (e) {
        window.location = "\CreateMawb";
    });

});