var l = abp.localization.getResource('Freight');
var columns = [


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
    }
]
$(function () {
  
    dolphin.freight.web.controllers.configuration.getJsonConfig('PaymentMadeList').done(function (data) {
        data.forEach(function (item) {
            if (!item.lock && item.checkable) {
                var column;


                if (item.text.toLowerCase() == 'paymentmadelist:invalid') {
                    column = {
                        //是否鎖定
                        title: l('PaymentMadeList:Invalid'),
                        orderable: false,
                        render: function (data, type, row, meta) {
                            if (row.void === true) {
                                return '<i class="fas fa-check"></i>'; // Use an icon for true (checkmark)
                            } else {
                                return '<i class="fas fa-times"></i>'; // Use an icon for false (cross)
                            }
                        }
                    }
                }
                else {
                    column = {
                        title: l(item.text),
                        data: item.name,
                        render: function (data, type, row) {
                            return data === null ? " " : data;
                        }
                    };
                }
                columns.push(column);
            }
        });

        var col = (columns.length > 2) ? [[2, 'asc']] : [[0, 'asc']];

        dataTable = $('#PaymentMadeListTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
                order: col,
                searching: false,
                scrollX: true,
                processing: true,
                responsive: {
                    details: {
                        type: 'column'
                    }
                },
                ajax: abp.libs.datatables.createAjax(dolphin.freight.accounting.payment.payment.getDataList, function () {
                    return {};
                }),
                columnDefs: columns
            })
        );

       
    });

    $('#btnConfiguration').click(function (e) {
        var _configurationModal = new abp.ModalManager({
            viewUrl: abp.appPath + 'Configuration',
            modalClass: 'ConfigurationViewModel'
        });

        _configurationModal.open({
            src: 'PaymentMadeList'
        });
    })

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
