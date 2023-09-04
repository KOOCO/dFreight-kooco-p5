$(function () {
    var l = abp.localization.getResource('Freight');

    var _changeInterval = null;
    var queryListFilter = function () {
        return {
            search: $("input[name='Search'").val()
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

   
    $('#Search').keyup(function () {
        clearInterval(_changeInterval)
        _changeInterval = setInterval(function () {
            dataTable.ajax.reload();
            clearInterval(_changeInterval)
        }, 1000);
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