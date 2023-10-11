var l = abp.localization.getResource('Freight');
var dataTable;
function createNew() {
    window.location.href = "/Accounting/Payment/CustomerPayment";
}
function copyMbl() {
    debugger;
    var checkedCheckboxes = $('.selectCheckbox:checked');
    var mblId = checkedCheckboxes.attr('data-id');
    window.location.href = "/Accounting/Payment/CustomerPayment?copyId=" + mblId;
}
$(function () {
    
 

     dataTable = $('#CustomerListTable').DataTable(
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
            ajax: abp.libs.datatables.createAjax(dolphin.freight.accounting.payment.customerPayment.getDataList, function () {

                return {};
            }),
            columnDefs: [
                //{
                //    className: 'dtr-control',
                //    orderable: false,
                //    "defaultContent": ""
                //},
                {
                    title: '<input type="checkbox" id="selectAllCheckbox" disable="true" onclick="selectAllCheckbox(this)" style=" cursor: pointer;">',
                    data: null,
                    orderable: false,
                    render: function (data, type, row) {
                        var id = row.id;
                        $('#selectAllCheckbox').prop('checked', false);
                        return '<input type="checkbox" class="selectCheckbox" data-id="' + id + '" onclick="selectCheckbox(this)" style=" cursor: pointer;">';
                    }
                },
                //{
                //    title: '<div  style=" cursor: pointer;"><span><i class="fa fa-lock"></i></span></div>',
                //    orderable: false,
                //    "render": function (data, type, row) {
                //        var isCkecked = row.isLocked;
                //        var Mblid = row.id;
                //        if (isCkecked) {
                //            return '<input type="checkbox" class="lockUnlockCheckbox" data-id="' + Mblid + '"  checked="' + isCkecked + '" onclick="lockCheckBox(this)"  style=" cursor: pointer;">';
                //        } else {
                //            return '<input type="checkbox" class="lockUnlockCheckbox" data-id="' + Mblid + '" onclick="lockCheckBox(this)"   style=" cursor: pointer;">';
                //        }
                //    }
                //},

                        {
                            title: l('Actions'),
                            rowAction: {
                                items:
                                    [
                                        {
                                            text: l('Edit'),
                                            visible: abp.auth.isGranted('Accounting.Customer.Edit'), //CHECK for the PERMISSION
                                            action: function (data) {
                                                if (data.record.isLocked) {

                                                }
                                                location.href = '/Accounting/Payment/CustomerPayment?Id=' + data.record.id + '&edit=Y';

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
                                                dolphin.freight.accounting.payment.customerPayment
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
                            title: l('CustomerList:ReleaseDate'),
                            data: "releaseDate",
                            type: "readonly"
                        },
                        {
                            title: l('CustomerList:ReceivablesSources'),
                            data: "receivablesSourcesName",
                            type: "readonly"
                        },
                        {
                            title: l('CustomerList:Category'),
                            data: "category",
                            type: "readonly"
                        },
                        {
                            title: l('CustomerList:CheckNo'),
                            data: "checkNo",
                            type: "readonly"
                        },
                        {
                            title: l('CustomerList:Bank'),
                            data: "bank",
                            type: "readonly"
                        },
                        {
                            title: l('CustomerList:ReceivedAmount') + '(' + "TWD" + ')',
                            data: "bankCurrency",
                            type: "readonly"
                        },
                     
                        {
                            title: l('CustomerList:DepositDate'),
                            data: "deposit",
                            type: "readonly"
                        },
                        {
                            title: l('CustomerList:Invalid'),
                            data: "invalid",
                            type: "readonly"
                        },
                        {
                            title: l('CustomerList:InvalidDate'),
                            data: "invalid",
                            type: "readonly"
                        },
                        {
                            title: l('CustomerList:Substation'),
                            data: "officeName",
                            type: "readonly"
                        },
                        {
                            title: l('CustomerList:CreatorId'),
                            data: "creatorId",
                            type: "readonly"
                        },
                        {
                            title: l('CustomerList:Memo'),
                            data: "memo",
                            type: "readonly"
                        }
                        //,
                        //{
                        //    title: l('CustomerList:Status'),
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
function selectAllCheckbox(element) {
    var isChecked = $(element).prop('checked');
    $('#CustomerListTable tbody input.selectCheckbox[type="checkbox"]').prop('checked', isChecked);

    if (isChecked) {
        $('#lockId').prop('disabled', !isChecked);
        $('#unlockId').prop('disabled', !isChecked);
    } else {
        $('#lockId').prop('disabled', !isChecked);
        $('#unlockId').prop('disabled', !isChecked);
    }
}
function selectCheckbox(checkbox) {
    var checkedCheckboxes = $('.selectCheckbox:checked');
    if (checkbox.checked) {
        var isAnyLocked = false;
        var isAnyUnlocked = false
        checkedCheckboxes.each(function (index, checkbox1) {
            $('#deleteBtn').prop('disabled', false);
            var id = $(checkbox1).data('id');

            if (checkedCheckboxes.length == 1) {
                $('#copyBtn').prop('disabled', false);
            } else {
                $('#copyBtn').prop('disabled', true);
            }

            var isLock = $('#lock_' + id).find('i').hasClass('fa-lock');
            if (isLock) {
                isAnyLocked = true;
            }
            else {
                isAnyUnlocked = true;
            }
        });
        $('#lockId').prop('disabled', !isAnyUnlocked);
        $('#unlockId').prop('disabled', !isAnyLocked);
    } else {
        var checkedCheckboxes = $('.selectCheckbox:checked');

        if (checkedCheckboxes && checkedCheckboxes.length == 0) {
            $('#deleteBtn').prop('disabled', true);
            $('#copyBtn').prop('disabled', true);
        }
        else if (checkedCheckboxes.length == 1) {
            $('#copyBtn').prop('disabled', false);
        }

        checkedCheckboxes.each(function (index, checkbox1) {

            var id = $(checkbox1).data('id');

            var isLock = $('#lock_' + id).find('i').hasClass('fa-lock');
            if (isLock) {
                isAnyLocked = true;
            }
            else {
                isAnyUnlocked = true;

            }

        });
        $('#deleteBtn').prop('disabled', true);
        $('#lockId').prop('disabled', !isAnyUnlocked);
        $('#unlockId').prop('disabled', !isAnyLocked);
    }
    if (!$(checkbox).prop('checked')) {
        $('#selectAllCheckbox').prop('checked', false);
    } else {
        var allChecked = true;
        $('#CustomerListTable tbody input.selectCheckbox[type="checkbox"]').each(function () {
            if (!$(this).prop('checked')) {
                allChecked = false;
                return false;
            }
        });
        $('#selectAllCheckbox').prop('checked', allChecked);
    }
}
function deleteMbls() {
    var ids = [];
    var selectedCheckboxes = $('#CustomerListTable tbody input.selectCheckbox[type="checkbox"]:checked');
    for (var i = 0; i < selectedCheckboxes.length; i++) {
        var id = selectedCheckboxes[i].attributes[2].value;
        var isLock = $('#lock_' + id).find('i').hasClass('fa-lock')
        if (!isLock) {
            ids.push(id);
        }
        abp.message.confirm(l('DeleteConfirmationMessage')).then(function (confirmed) {
            if (confirmed) {
                dolphin.freight.accounting.payment.customerPayment.deleteMultiplePayments(ids)
                    .done(function () {
                        abp.message.success(l('Message:SuccessDelete'));
                        dataTable.ajax.reload();
                    });
            }
        });
    }
}
