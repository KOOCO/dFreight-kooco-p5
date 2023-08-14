$(function () {
    var l = abp.localization.getResource('Freight');

    var dataTable = $('#TPCreditsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            responsive: {
                details: {
                    type: 'column'
                }
            },
            ajax: abp.libs.datatables.createAjax(dolphin.freight.tradePartners.tradePartner.getList),
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
                                        window.location = "\EditTradePartnerInfo?id=" + data.record.id;
                                        //editModal.open({ id: data.record.id });
                                    }
                                }
                            ]
                    }
                },
                {
                    title: l('Display:Credit:THead:Code'),
                    data: "tpCode"
                },
                {
                    title: l('Display:Credit:THead:TradePartner'),
                    data: "tpName"
                },
                {
                    title: l('Display:Credit:THead:Alias'),
                    data: "tpAliasName"
                },
                {
                    title: l('Display:Credit:THead:Type'),
                    data: "tpType",
                    render: function (data) {
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
                },
                {
                    title: l('Display:Credit:THead:PaymentType'),
                    data: "paymentType",
                    render: function (data) {
                        return l([
                            "Enum:PaymentType.Cod",
                            "Enum:PaymentType.Credit",
                            "Enum:PaymentType.CreditHold",
                            "Enum:PaymentType.Koinfo"
                        ][data]);
                    }
                },
                {
                    title: l('Display:Credit:THead:CreditTerm'),
                    data: "creditTermType",
                    render: function (data) {
                        return l([
                            "Enum:CreditTermType.DaysAfterETA",
                            "Enum:CreditTermType.DaysAfterETD",
                            "Enum:CreditTermType.Days",
                            "Enum:CreditTermType.DueOnReceipt",
                            "Enum:CreditTermType.EndOfNextMonth",
                            "Enum:CreditTermType.EndOfThisMonth",
                            "Enum:CreditTermType.OfNextMonth"
                        ][data]);
                    }
                },
                {
                    title: l('Display:Credit:THead:Day'),
                    data: "creditTermDays"
                },
                {
                    title: l('Display:Credit:THead:CreditLimit'),
                    data: "creditLimit"
                },
                {
                    title: l('Display:Credit:THead:Remark'),
                    data: "remark"
                }
            ]
        })
    );

    

});