$(function () {
    var l = abp.localization.getResource('Freight');

    var dataTable = $('#PortsManagementTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: true,
            scrollX: true,
            responsive: {
                details: {
                    type: 'column'
                }
            },
            ajax: abp.libs.datatables.createAjax(dolphin.freight.settings.portsManagement.portsManagement.getList),
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Edit'),
                                    visible: abp.auth.isGranted('Settings.PortsManagement.Edit'), //CHECK for the PERMISSION
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    visible: abp.auth.isGranted('Settings.PortsManagement.Delete'), //CHECK for the PERMISSION
                                    confirmMessage: function (data) {
                                        return l('PortsManagementDeletionConfirmationMessage', data.record.startNo, data.record.endNo);
                                    },
                                    action: function (data) {
                                        dolphin.freight.settings.portsManagement.portsManagement
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
                    //是否顯示	
                    title: l('IsActive'),
                    data: "isActive",
                     // Set the width of the third column
                    render: function (data, type, row, meta) {
                        if (row.isActive == true) {
                            return '<input class="form-check-input" type="checkbox" id="showOnMAWBCheckBox" value="" checked disabled>'
                           
                        } else {
                            return '<input class="form-check-input" type="checkbox" id="showOnMAWBCheckBox" value="" disabled>'

                        }


                    },
                },
                {
                    //所屬國家
                    title: l('Country'),
                    data: "country"
                },
                {
                    //港口名稱
                    title: l('PortName'),
                    data: "portName"
                },
                {
                    //聯合國口岸及相關地點代碼
                    title: l('Locode'),
                    data: "locode"
                },
                {
                    //細分
                    title: l('SubDiv'),
                    data: "subDiv"
                },
                {
                    //是否港口
                    title: l('IsPort'),
                    data: "isPort"
                }
            ]
        })
    );

    $('[type=search]').on('keyup', function () {
        dataTable.search(this.value).draw();
    });

    var createModal = new abp.ModalManager(abp.appPath + 'Settings/PortsManagement/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Settings/PortsManagement/EditModal');

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewPortsManagementButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});