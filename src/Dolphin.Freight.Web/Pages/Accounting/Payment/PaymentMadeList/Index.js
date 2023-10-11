$(function () {
    var l = abp.localization.getResource('Freight');


    var dataTable = $('#PaymentMadeListTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[2, "asc"]],
            searching: true,
            scrollX: true,
            responsive: {
                details: {
                    type: 'column'
                }
            },
            ajax: abp.libs.datatables.createAjax(dolphin.freight.accounting.payment.payment.getDataList, function () {
                return {};
            }),
            columnDefs: [
                {
                    className: 'dtr-control',
                    orderable: false,
                    "defaultContent": ""
                },
                        {
                            title: l('Actions'),
                            rowAction: {
                                items:
                                    [
                                        {
                                            text: l('Edit'),
                                            visible: abp.auth.isGranted('Accounting.PaymentMadeList.Edit'), //CHECK for the PERMISSION
                                            action: function (data) {
                                                if (data.record.isLocked) {

                                                }
                                                location.href = '/Accounting/Payment/Payment?Id=' + data.record.id + '&edit=Y';

                                            }
                                        },
                                        {
                                            text: l('Delete'),
                                            confirmMessage: function (data) {
                                                return l(
                                                    'AreYouSureToDelete'
                                                );
                                            },
                                            action: function (data) {
                                                dolphin.freight.accounting.payment.payment
                                                    .delete(data.record.id)
                                                    .then(function () {
                                                        abp.notify.info(l('SuccessfullyDeleted'));
                                                        dataTable.ajax.reload();
                                                    });
                                            }
                                        }
                                    ]
                            }
                        },
                        {
                            data: "id",
                            visible: false,
                            searchable: false,
                            type: "hidden"
                        },
                        {
                            title: l('PaymentMadeList:ReleaseDate'),
                            data: "releaseDate",
                            type: "readonly"
                        },
                        {
                            title: l('PaymentMadeList:PaidTo'),
                            data: "paidToName",
                            type: "readonly"
                        },
                        {
                            title: l('PaymentMadeList:Category'),
                            data: "category",
                            type: "readonly"
                        },
                        {
                            title: l('PaymentMadeList:CheckNo'),
                            data: "checkNo",
                            type: "readonly"
                        },
                        {
                            title: l('PaymentMadeList:Bank'),
                            data: "bank",
                            type: "readonly"
                        },
                        {
                            title: l('PaymentMadeList:PaidAmount') + '(' + "TWD" + ')',
                            data: "bankCurrency",
                            type: "readonly"
                        },
                     
                        {
                            title: l('PaymentMadeList:ClearDate'),
                            data: "clear",
                            type: "readonly"
                        },
                        {
                            title: l('PaymentMadeList:Invalid'),
                            data: "void",
                            render: function (data, type, row) {
                                if (data === true) {
                                    return '<i class="fas fa-check"></i>'; // Use an icon for true (checkmark)
                                } else {
                                    return '<i class="fas fa-times"></i>'; // Use an icon for false (cross)
                                }
                            }
                        },
                        {
                            title: l('PaymentMadeList:InvalidDate'),
                            data: "invalid",
                            type: "readonly"
                        },
                        {
                            title: l('PaymentMadeList:Substation'),
                            data: "officeName",
                            type: "readonly"
                        },
                        {
                            title: l('PaymentMadeList:CreatorId'),
                            data: "creatorId",
                            type: "readonly"
                        },
                        {
                            title: l('PaymentMadeList:Memo'),
                            data: "memo",
                            type: "readonly"
                        }
                        //,
                        //{
                        //    title: l('PaymentMadeList:Status'),
                        //    data: "",
                        //    type: "readonly"
                        //}
                    ]
        })
    );

    function FIndTpName(id)
    {
        var Tpname = '';
        dolphin.freight.tradePartners.tradePartner.findByTpId(id)
            .then(function (result)
            {
                console.log(result);
                if (result == null)
                {
                    return Tpname;
                }
                else
                {
                    Tpname = result;
                    return Tpname;
                }
            });
    }
});
