$(function () {
    var l = abp.localization.getResource('Freight');

    var queryListFilter = function () {
        return {
            filter: $("input[name='Search'").val(),
        };
    };

    var dataTable = $('#PackageUnitsTable').DataTable(
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
            ajax: abp.libs.datatables.createAjax(dolphin.freight.settings.packageUnits.packageUnit.queryList, queryListFilter),
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
                                        dolphin.freight.settings.packageUnits.packageUnit
                                            .delete(data.record.id)
                                            .then(function () {
                                                console.log("開始")
                                                console.log(l('SuccessfullyDeleted'))
                                                abp.message.info(l('SuccessfullyDeleted'), '系統訊息');
                                                // abp.notify.info(l('SuccessfullyDeleted'),'系統訊息');
                                                dataTable.ajax.reload();
                                            });
                                    }
                                }
                            ]
                    }
                }, {
                    //代碼
                    title: l('PackageCode'),
                    data: "packageCode"
                },
               
                {
                    //結束號碼
                    title: l('PackageName'),
                    data: "packageName"
                },
                {
                    //當前分配的號碼
                    title: l('AmsNo'),
                    data: "amsNo"
                },
                {
                    //當前分配的號碼
                    title: l('EManifestNo'),
                    data: "eManifestNo"
                },
                {
                    //備註
                    title: l('Description'),
                    data: "description"
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
 
    var createModal = new abp.ModalManager(abp.appPath + 'Settings/PackageUnits/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Settings/PackageUnits/EditModal');

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });
    $('#NewPackageUnitButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });

});