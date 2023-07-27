$(function () {
    var l = abp.localization.getResource('Freight');
    var getFilter = function () {
        return {


            code: $("#GlCode").val(),
            remark: $("#Remark").val(),

        }
    };

    var dataTable = $('#GlCodesTable').DataTable(
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
            ajax: abp.libs.datatables.createAjax(dolphin.freight.accountingSettings.glCodes.glCode.queryList, getFilter),
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Edit'),
                                    visible: abp.auth.isGranted('Settings.ItNoRanges.Edit'), //CHECK for the PERMISSION
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    visible: abp.auth.isGranted('Settings.ItNoRanges.Delete'), //CHECK for the PERMISSION
                                    confirmMessage: function (data) {
                                        return l('DeletionConfirmationMessage');
                                    },
                                    action: function (data) {
                                        dolphin.freight.accountingSettings.glCodes.glCode
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.message.success(l('SuccessfullyDeleted'));
                                                dataTable.ajax.reload();
                                            });
                                    }
                                }
                            ]
                    }
                }, {
                    //科目代碼
                    title: l('GlCode'),
                    data: "code"
                },

                {
                    //類別
                    title: l('GlTypeName'),
                    data: "glTypeName"
                },
                {
                    //群組
                    title: l('GlGroupName'),
                    data: "glGroupName"
                },
                {
                    //描述
                    title: l('Remark'),
                    data: "remark"
                },
                {
                    //子項目
                    title: l('SubInfo'),
                    data: "subInfo"
                },
                {
                    //會計用代碼
                    title: l('AccountingGlCode'),
                    data: "accountingGlCode"
                },
                {
                    //啟用
                    title: l('IsUsed'),
                    data: function (row, type, set) {
                        if (row.isUsed) return "V";
                        else return "";

                    }
                },
                {
                    //存款
                    title: l('IsDeposit'),
                    data: function (row, type, set) {
                        if (row.isDeposit) return "V";
                        else return "";

                    }
                },
                {
                    //收付款
                    title: l('IsPayment'),
                    data: function (row, type, set) {
                        if (row.isPayment) return "V";
                        else return "";

                    }
                },
                {
                    //重估
                    title: l('IsRevaluation'),
                    data: function (row, type, set) {
                        if (row.isRevaluation) return "V";
                        else return "";

                    }
                }
            ]
        })
    );
  
    $('[type=search]').on('keyup', function () {
        dataTable.search(this.value).draw();
    });
 
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'AccountingSettings/GlCodes/CreateModal',
        scriptUrl:"/Pages/AccountingSettings/GlCodes/createModal.js",
        modelClass:"giCodeCreate"
    });
    var editModal = new abp.ModalManager(abp.appPath + 'AccountingSettings/GlCodes/EditModal');

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });
    $('#NewGlCodeButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
    $('#SearchGICode').click(function (e) {
        debugger;
        dataTable.ajax.reload();
    });


});