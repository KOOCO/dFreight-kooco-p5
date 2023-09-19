$(function () {
    var l = abp.localization.getResource('Freight');

    var _changeInterval = null;
    var queryListFilter = function () {
        return {
            search: $("#Keyword").val(),


            freightLocationId: $("#VesselSchedule_FreightLocationId").val() == '' ? null : $("#VesselSchedule_FreightLocationId").val(),

            consigneeId: $("#VesselSchedule_ConsigneeId").val() == '' ? null : $("#VesselSchedule_ConsigneeId").val(),
            carrierId: $("#VesselSchedule_CarrierId").val() == '' ? null : $("#VesselSchedule_CarrierId").val(),
            depatureId: $("#VesselSchedule_DepatureId").val() == '' ? null : $("#VesselSchedule_DepatureId").val(),
            destinationId: $("#VesselSchedule_DestinationId").val() == '' ? null : $("#VesselSchedule_DestinationId").val(),
           
            directMaster: $("#DirectMaster").val() == '' ? null : $("#DirectMaster").val(),
            flightNo: $("#FlightNo").val(),
            officeId: $("#VesselSchedule_OfficeId").val() == '' ? null : $("#VesselSchedule_OfficeId").val(),
            awbType: $("#AwbType").val() == '' ? null : $("#AwbType").val(),


            depatureDate: $("#DepatureDate").val() == '' || $("#DepatureDate").val() == null ? null : new Date($("#DepatureDate").val()),
            arrivalDate: $("#ArrivalDate").val() == '' || $("#ArrivalDate").val() == null ? null : new Date($("#ArrivalDate").val()),
            postDate: $("#PostDate").val() == '' || $("#PostDate").val() == null ? null : new Date($("#PostDate").val()),
            creationDate: $("#CreationDate").val() == '' || $("#CreationDate").val() == null ? null : new Date($("#CreationDate").val()),

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
            ajax: abp.libs.datatables.createAjax(dolphin.freight.importExport.airImports.airImportMawb.getList, queryListFilter),
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

   
    $('#Search').click(function (e) {
        dataTable.ajax.reload();
    });
    

    $('#AddMawbButton').click(function (e) {
        window.location = "\CreateMawb";
    });

});