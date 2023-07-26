$(function () {
    var l = abp.localization.getResource('Freight');

    var getQueryfilter = function () {
        return {
            code: $("#Code").val(),
            name: $("#Name").val(),
            glCode: $("#GlCode").val(),
            glDescription: $("#GlDescription").val()
        };
    };

    var dataTable = $('#BillingCodesTable').DataTable(
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
            autoWidth: true,
            ajax: abp.libs.datatables.createAjax(dolphin.freight.accountingSettings.billingCodes.billingCode.queryList, getQueryfilter),
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
                                        dolphin.freight.accountingSettings.billingCodes.billingCode
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
                    //代碼
                    title: l('BCode'),
                   
                    data: "code"
                },

                {
                    //名稱(英文)
                    title: l('BillingName'),
                    width: "300px;",
                    data: "billingName"
                },
                {
                    //當地名稱
                    title: l('LocalName'),
                    data: "localName"
                },
                {
                    //收入
                    title: l('RevenueName'),
                    data: "revenueName"
                },
                {
                    //成本
                    title: l('CostName'),
                    data: "costName"
                },
                {
                    //貸記
                    title: l('CreditName'),
                    data: "creditName"
                },
                {
                    //借記
                    title: l('DeitName'),
                    data: "deitName"
                },
                {
                    //A/R
                    title: l('IsAR'),
                    data: function (row, type, set) {
                        if (row.isAR) return "V";
                        else return "";

                    }
                },
                {
                    //A/P
                    title: l('IsAP'),
                    data: function (row, type, set) {
                        if (row.isAP) return "V";
                        else return "";

                    }
                },
                {
                    //A/P
                    title: l('IsDC'),
                    data: function (row, type, set) {
                        if (row.isDC) return "V";
                        else return "";

                    }
                },
                {
                    //GA
                    title: l('IsGA'),
                    data: function (row, type, set) {
                        if (row.isGA) return "V";
                        else return "";

                    }
                },
                {
                    //海運進口主單
                    title: l('IsOceanImportMbl'),
                    data: function (row, type, set) {
                        if (row.isOceanImportMbl) return "V";
                        else return "";

                    }
                },
                {
                    //海運進口分單
                    title: l('IsOceanImportHbl'),
                    data: function (row, type, set) {
                        if (row.isOceanImportHbl) return "V";
                        else return "";

                    }
                },
                {
                    //海運出口主單
                    title: l('IsOceanExportMbl'),
                    data: function (row, type, set) {
                        if (row.isOceanExportMbl) return "V";
                        else return "";

                    }
                },
                {
                    //海運出口分單
                    title: l('IsOceanExportHbl'),
                    data: function (row, type, set) {
                        if (row.isOceanExportHbl) return "V";
                        else return "";

                    }
                },
                {
                    //空運進口主單
                    title: l('IsAirImportMbl'),
                    data: function (row, type, set) {
                        if (row.isAirImportMbl) return "V";
                        else return "";

                    }
                },
                {
                    //空運進口分單
                    title: l('IsAirImportHbl'),
                    data: function (row, type, set) {
                        if (row.isAirImportHbl) return "V";
                        else return "";

                    }
                },
                {
                    //空運出口主單
                    title: l('IsAirExportMbl'),
                    data: function (row, type, set) {
                        if (row.isAirExportMbl) return "V";
                        else return "";

                    }
                },
                {
                    //空運出口分單
                    title: l('IsAirExportHbl'),
                    data: function (row, type, set) {
                        if (row.isAirExportHbl) return "V";
                        else return "";

                    }
                },
                {
                    //卡車
                    title: l('IsTkm'),
                    data: function (row, type, set) {
                        if (row.isTkm) return "V";
                        else return "";

                    }
                },
                {
                    //綜合業務
                    title: l('IsMsm'),
                    data: function (row, type, set) {
                        if (row.isMsm) return "V";
                        else return "";

                    }
                },
                {
                    //倉儲
                    title: l('IsWhs'),
                    data: function (row, type, set) {
                        if (row.isWhs) return "V";
                        else return "";

                    }
                },
                {
                    //啟用
                    title: l('IsUsed'),
                    data: function (row, type, set) {
                        if (row.isUsed) return "V";
                        else return "";

                    }
                }


            ]
        })
    );

    //$('[type=search]').on('keyup', function () {
    //    dataTable.search(this.value).draw();
    //});
    $('#SearchButton').click(function (e) {
        dataTable.ajax.reload();
    });
 
    var createModal = new abp.ModalManager(abp.appPath + 'AccountingSettings/BillingCodes/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'AccountingSettings/BillingCodes/EditModal');

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });
    $('#NewBillingCodeButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });

});