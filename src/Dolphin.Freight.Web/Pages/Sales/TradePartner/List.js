$(function () {
    var l = abp.localization.getResource('Freight');

    var _changeInterval = null;
    var queryListFilter = function () {
        return {
            search: $("#Keyword").val(),

            tpType: $("#tpType").val() == '' ? null : $("#tpType").val(),
            tpAccountGroupName: $("#tpAccountGroupName").val() == '' ? null : $("#tpAccountGroupName").val(),
            tpCountryCode: $("#tpCountryCode").val() == '' ? null : $("#tpCountryCode").val(),
            status: $("#status").val() == '' ? null : $("#status").val(),
           
           
            creationDate: $("#CreationDate").val() == '' || $("#CreationDate").val() == null ? null : new Date($("#CreationDate").val()),
            name: $("#name").val(),
            address: $("#address").val(),
            city: $("#city").val(),
            city: $("#state").val(),
            taxId: $("#taxid").val(),
            salePerson: $("#salePerson").val(),
            saleOffice: $("#saleOffice").val(),
        };
    };


    var dataTable = $('#TPListTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            processing: true,
            
            ajax: abp.libs.datatables.createAjax(dolphin.freight.tradePartners.tradePartner.getList, queryListFilter ),
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Edit'),
                                    //visible: abp.auth.isGranted('BookStore.Books.Edit'),
                                    action: function (data) {
                                        //editModal.open({ id: data.record.id });
                                        console.log(data);
                                        /*window.location = "\EditTradePartnerInfo?id=" + data.record.id;*/
                                        window.location = "\EditTradePartnerInfo\\" + data.record.id;
                                    }
                                },
                                //{
                                //    text: l('Set Account Group'),
                                //    //visible: abp.auth.isGranted('BookStore.Books.Edit'),
                                //    action: function (data) {
                                        
                                //    }
                                //},
                                {
                                    text: l('Delete'),
                                    // visible: abp.auth.isGranted('BookStore.Authors.Delete'),
                                    confirmMessage: function (data) {
                                        return l(
                                            'AreYouSureToDelete',
                                            data.record.name
                                        );
                                    },
                                    action: function (data) {
                                        dolphin.freight.tradePartners.tradePartner
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.info(
                                                    l('SuccessfullyDeleted')
                                                );
                                                dataTable.ajax.reload();
                                            });
                                    }
                                }
                            ]
                    }
                },
                {
                    title: l('Display:TradePartner:THead:Code'),
                    data: "tpCode",
                    width: '300px', targets: 0
                },
                {
                    title: l('Display:TradePartner:THead:Name'),
                    data: "tpName",
                    width: '300px'
                },
                {
                    title: l('Display:TradePartner:THead:Alias'),
                    data: "tpAliasName",
                    width: '300px'
                },
                {
                    title: l('Display:TradePartner:THead:LocalName'),
                    data: "tpNameLocal",
                    width: '300px'
                },
                {
                    title: l('SCAC'),
                    data: "scacCode",
                    width: '300px'
                },
                {
                    title: l('IATA'),
                    data: "iataCode",
                    width: '300px'
                },
                //{
                //    title: l('Display:TradePartner:THead:SCAC'),
                //    data: {},
                //    render: function (data) {
                //        if ((data.scacCode == null) && (data.iataCode == null)) {
                //            return '';
                //        } else if ((data.scacCode == null) && (data.iataCode != null)) {
                //            return data.iataCode;
                //        } else if ((data.scacCode != null) && (data.iataCode == null)) {
                //            return data.scacCode;
                //        } else if ((data.scacCode) && (data.iataCode)) {
                //            // GoFreight系統中為只顯示scac
                //            return data.scacCode;
                //        }
                //    }
                //},
                {
                    title: l('Display:TradePartner:THead:FirmCode'),
                    data: "firmsCode",
                    width: '300px'
                },
                {
                    title: l('Display:TradePartner:THead:Type'),
                    width: '300px',
                    data: "tpType",
                    render: function (data) {
                        if (data) {
                            console.log(data);
                            return l([
                                "Enum:TPType.AirCarrier",
                                "Enum:TPType.Bank",
                                "Enum:TPType.BookingWindow",
                                "Enum:TPType.CFS",
                                "Enum:TPType.Consignee",
                                "Enum:TPType.Customer",
                                "Enum:TPType.CustomerBroker",
                                "Enum:TPType.Cy",
                                "Enum:TPType.Employee",
                                "Enum:TPType.Forwarder",
                                "Enum:TPType.Government",
                                "Enum:TPType.Manufacturer",
                                "Enum:TPType.OceanCarrier",
                                "Enum:TPType.OfficeExpense",
                                "Enum:TPType.Others",
                                "Enum:TPType.OverseaAgent",
                                "Enum:TPType.RailCompany",
                                "Enum:TPType.RampLocation",
                                "Enum:TPType.ShipperKnown",
                                "Enum:TPType.ShipperUnknown",
                                "Enum:TPType.Terminal",
                                "Enum:TPType.Trucker",
                                "Enum:TPType.Vendor",
                                "Enum:TPType.Warehouse",
                                "Enum:TPType.FBAWarehouse"
                            ][data - 1]);
                        }
                        else
                            return "";
                    }
                },
                {
                    title: l('Display:TradePartner:THead:Address'),
                    data: "tpPrintAddress",
                    width: '300px',
                },
                {
                    title: l('Display:TradePartner:THead:AccountingAddress'),
                    data: "accountAddress",
                    width: '300px',
                },
                {
                    title: l('Display:TradePartner:THead:City'),
                    data: "cityCode",
                    width: '300px'
                },
                {
                    title: l('Display:TradePartner:THead:State'),
                    data: "stateCode",
                    width: '300px'
                },
                {
                    title: l('Display:TradePartner:THead:Tax'),
                    data: "taxId",
                    width: '300px'
                },
                {
                    title: l('Display:TradePartner:THead:TrackPayment'),
                    data: "trackPayment",
                    dataFormat: Boolean,
                    width: '300px'
                },
                {
                    title: l('Display:TradePartner:THead:Country'),
                    data: "countryName",
                    width: '300px'
                }
            ]
        })
    );

   
    $('#Search').click(function (e) {
        dataTable.ajax.reload();
    });
    

    $('#AddTradePartnerButton').click(function (e) {
        window.location = "\TradePartnerInfo";
    });

    //var editModal = new abp.ModalManager({
    //    viewUrl: '/Sales/TradePartner/Credit/ModalWithEditCreditLimitGroup'
    //});

    //editModal.onResult(function () {
    //    console.log('open the CreditLimitGroup editModal');
    //    dataTable.ajax.reload();
    //});
   
});

function exportToExcel() {


    var table = document.getElementById("TPListTable");
    var columnsToExclude = [1,2];

    // Create a new worksheet
    var ws = XLSX.utils.aoa_to_sheet([]);

    // Create a style object for the header row
    var headerStyle = {
        font: { bold: true },
        fill: { bgColor: { indexed: 23 }, fgColor: { indexed: 23 } }, // Blue color
        alignment: { horizontal: "center" }, // Center-align text
    };

    // Iterate through table rows and build the new worksheet
    for (var rowIndex = 0; rowIndex < table.rows.length; rowIndex++) {
        var row = table.rows[rowIndex];
        var newRow = [];
        for (var colIndex = 0; colIndex < row.cells.length; colIndex++) {
            if (columnsToExclude.includes(colIndex)) {
                newRow.push(row.cells[colIndex].innerText);
            }
        }
        XLSX.utils.sheet_add_aoa(ws, [newRow], { origin: -1 });
    }

    // Apply the header style to the first row
    XLSX.utils.format_cell(ws['A1'], headerStyle);
    XLSX.utils.format_cell(ws['B1'], headerStyle);
    XLSX.utils.format_cell(ws['C1'], headerStyle);
    // Add more cells as needed for additional columns

    // Create a new workbook and add the worksheet
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Sheet1");

    // Save the workbook as an Excel file
    XLSX.writeFile(wb, "TradePartnerList.xlsx");
}