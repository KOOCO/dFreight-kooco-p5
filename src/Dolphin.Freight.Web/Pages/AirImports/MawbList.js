$(function () {
    var l = abp.localization.getResource('Freight');

    var dataTable = $('#MawbListTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[2, "asc"]],
            searching: false,
            scrollX: true,
            responsive: {
                details: {
                    type: 'column'
                }
            },
            ajax: abp.libs.datatables.createAjax(dolphin.freight.importExport.airImports.airImportMawb.getList),
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
                                        window.location = "\EditMawb?ShowMsg=true&Id=" + data.record.id;
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
                    title: l('OverseaAgent'),
                    data: "overseaAgentTPName"
                },
                {
                    title: l('Carrier'),
                    data: "carrierTPName"
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

   

    

    $('#AddMawbButton').click(function (e) {
        window.location = "\CreateMawb";
    });

});