$(function () {
    var l = abp.localization.getResource('Freight');

    var queryListFilter = function () {
        return {
            filter: $("input[name='Search'").val(),
            active: $("#Active").val() ? $("#Active").val() == 0 ? false : true : null
        };
    };

    var dataTable = $('#ContainerSizesTable').DataTable(
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
            ajax: abp.libs.datatables.createAjax(dolphin.freight.settings.containerSizes.containerSize.queryList, queryListFilter),
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
                                        dolphin.freight.settings.containerSizes.containerSize
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
                    title: l('ContainerCode'),
                    data: "containerCode"
                },
               
                {
                    //貨描
                    title: l('SizeDescription'),
                    data: "sizeDescription"
                },
                {
                    title: l('AmsTypeCode'),
                    data: "amsTypeCode"
                },
                {
                    //類別
                    title: l('ContainerGroup'),
                    data: "containerGroup"
                },
                {
                    //Teu
                    title: l('Teu'),
                    data: "teu"
                }, {
                    //是否啟用
                    title: l('IsUseed'),
                    orderable: false,
                    data: function (row, type, set) {
                        if (row.isUseed) return "是";
                        else return "否";
                        
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
 
    var createModal = new abp.ModalManager(abp.appPath + 'Settings/ContainerSizes/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Settings/ContainerSizes/EditModal');

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });
    $('#NewContainerSizeButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });

});