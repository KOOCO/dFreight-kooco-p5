var l = abp.localization.getResource('Freight');
var createMemoModal = new abp.ModalManager({
    viewUrl: '/AirExports/CopyMawbModal',
});
var dataTable;
var _changeInterval = null;
var queryListFilter = function () {
    return {
        search: $("#Keyword").val(),
       
        ovearseaAgentId: $("#VesselSchedule_OverseaAgentId").val() == '' ? null : $("#VesselSchedule_OverseaAgentId").val(),
        carrierId: $("#VesselSchedule_CarrierId").val() == '' ? null : $("#VesselSchedule_CarrierId").val(),
        shippingAgentId: $("#VesselSchedule_ShippingAgentId").val() == '' ? null : $("#VesselSchedule_ShippingAgentId").val(),
        forwardingAgentId: $("#VesselSchedule_ForwardingAgentId").val() == '' ? null : $("#VesselSchedule_ForwardingAgentId").val(),
        pod: $("#VesselSchedule_PodId").val() == '' ? null : $("#VesselSchedule_PodId").val(),
        pol: $("#VesselSchedule_PolId").val() == '' ? null : $("#VesselSchedule_PolId").val(),
        pol: $("#VesselSchedule_PolId").val() == '' ? null : $("#VesselSchedule_PolId").val(),
        del: $("#VesselSchedule_DelId").val() == '' ? null : $("#VesselSchedule_DelId").val(),
        deliverTo: $("#VesselSchedule_DeliverToId").val() == '' ? null : $("#VesselSchedule_DeliverToId").val(),
        shipModeId: $("#VesselSchedule_ShipModeId").val() == '' ? null : $("#VesselSchedule_ShipModeId").val(),
        blCancelled: $("#BlCancelled").val() == '' ? null : $("#BlCancelled").val(),
        postDate: $("#PostDate").val() == '' || $("#PostDate").val() == null ? null : new Date($("#PostDate").val()),
        
        eta: $("#ETA").val() == '' || $("#ETA").val() == null? null : new Date($("#ETA").val()),
        etd: $("#ETD").val() == '' || $("#ETD").val() == null ? null : new Date($("#ETD").val()),
        creationDate: $("#CreationDate").val() == '' || $("#CreationDate").val() == null ? null : new Date($("#CreationDate").val()),
        vessel: $("#Vessel").val(),
    };
};

var columns = [{
    title: '<input type="checkbox" id="selectAllCheckbox" disable="true" onclick="selectAllCheckbox(this)" style=" cursor: pointer;">',
    data: null,
    orderable: false,
    render: function (data, type, row) {
        var id = row.id;
        $('#selectAllCheckbox').prop('checked', false);
        return '<input type="checkbox" class="selectCheckbox" data-id="' + id + '" onclick="selectCheckbox(this)" style=" cursor: pointer;">';
    }
},
    {
        title: '<div  style=" cursor: pointer;"><span><i class="fa fa-lock"></i></span></div>',
        orderable: false,
        "render": function (data, type, row) {
            var isCkecked = row.isLocked;
            var Mblid = row.id;
            if (isCkecked) {
                return '<input type="checkbox" class="lockUnlockCheckbox" data-id="' + Mblid + '"  checked="' + isCkecked + '" onclick="lockCheckBox(this)"  style=" cursor: pointer;">';
            } else {                                                      
                return '<input type="checkbox" class="lockUnlockCheckbox" data-id="' + Mblid + '" onclick="lockCheckBox(this)"   style=" cursor: pointer;">';
            }
        }
    },
    {
        title: l('Actions'),
        orderable: false,
        rowAction: {
            items:
                [
                    {
                        text: l('Edit'),
                        visible: abp.auth.isGranted('Settings.ItNoRanges.Edit'), //CHECK for the PERMISSION
                        action: function (data) {
                            if (data.record.isLocked) {

                            }
                            location.href = 'EditModal?Id=' + data.record.id;
                        }
                    },
                    {
                        text: l('Delete'),
                        visible: function (data) {

                            return !data.isLocked && abp.auth.isGranted('Settings.ItNoRanges.Delete')
                        },
                        action: function (data) {
                            if (!data.record.isLocked) {
                                abp.message.confirm(l('DeletionConfirmationMessage'))
                                    .then(function (confirmed) {
                                        if (confirmed) {
                                            dolphin.freight.importExport.oceanExports.oceanExportMbl
                                                .delete(data.record.id)
                                                .then(function () {
                                                    abp.message.success(l('SuccessfullyDeleted'));
                                                    dataTable.ajax.reload();
                                                });
                                        }
                                    });

                        } else {
                            abp.message.warn("鎖定不能刪除")
                        }
                    }
                }
            ]
        }

    },
 
]

function lockCheckBox(checkbox) {
    var selectedCheckboxes = $('#MblListTable tbody input.lockUnlockCheckbox[type="checkbox"]:checked');

    var id = checkbox.attributes[2].value;

    var isLock = $('#lock_' + id).find('i').hasClass('fa-lock');
    abp.message.confirm(l(isLock ? 'UnlockConfirmationMessage' : 'LockConfirmationMessage')).then(function (confirmed) {
        if (confirmed) {
            dolphin.freight.importExport.oceanExports.oceanExportMbl.lockedOrUnLockedOceanExportMbl({ MbId: id })
                .done(function () {
                    if (isLock) {
                        abp.message.success(l('Message:SuccessUnlock'));
                    } else {
                        abp.message.success(l('Message:SuccessLock'));
                    }
                    dataTable.ajax.reload();
                });
        }
    });
}

function selectAllCheckbox(element) {
    var isChecked = $(element).prop('checked');
    $('#MblListTable tbody input.selectCheckbox[type="checkbox"]').prop('checked', isChecked);
}

function copyMbl() {
    var checkedCheckboxes = $('.selectCheckbox:checked');
    var mblId = checkedCheckboxes.attr('data-id');
    createMemoModal.open({
        mblId,
    });
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
        $('#MblListTable tbody input.selectCheckbox[type="checkbox"]').each(function () {
            if (!$(this).prop('checked')) {
                allChecked = false;
                return false;
            }
        });
        $('#selectAllCheckbox').prop('checked', allChecked);
    }
}

function selectedLock() {
    var ids = [];
    var selectedCheckboxes = $('#MblListTable tbody input.selectCheckbox[type="checkbox"]:checked');
    for (var i = 0; i < selectedCheckboxes.length; i++) {
        var id = selectedCheckboxes[i].attributes[2].value;
        var isLock = $('#lock_' + id).find('i').hasClass('fa-lock')
        if (!isLock) {
            ids.push(id);
        }
        abp.message.confirm(l('LockConfirmationMessage')).then(function (confirmed) {
            if (confirmed) {
                dolphin.freight.importExport.oceanExports.oceanExportMbl.selectedLockedOceanExportMbl(ids)
                    .done(function () {
                        abp.message.success(l('Message:SuccessLock'));
                        dataTable.ajax.reload();
                    });
            }
        });
    }

}

function deleteMbls() {
    var ids = [];
    var selectedCheckboxes = $('#MblListTable tbody input.selectCheckbox[type="checkbox"]:checked');
    for (var i = 0; i < selectedCheckboxes.length; i++) {
        var id = selectedCheckboxes[i].attributes[2].value;
        var isLock = $('#lock_' + id).find('i').hasClass('fa-lock')
        if (!isLock) {
            ids.push(id);
        }
        abp.message.confirm(l('DeleteConfirmationMessage')).then(function (confirmed) {
            if (confirmed) {
                dolphin.freight.importExport.oceanExports.oceanExportMbl.deleteMultipleMbls(ids)
                    .done(function () {
                        abp.message.success(l('Message:SuccessDelete'));
                        dataTable.ajax.reload();
                    });
            }
        });
    }
}

function createNew() {
    window.location.href = "/OceanExports/CreateMbl";
}

function selectedUnLock() {
    var ids = [];
    var selectedCheckboxes = $('#MblListTable tbody input.selectCheckbox[type="checkbox"]:checked');
    for (var i = 0; i < selectedCheckboxes.length; i++) {
        var id = selectedCheckboxes[i].attributes[2].value;
        var isLock = $('#lock_' + id).find('i').hasClass('fa-lock')
        if (isLock) {
            ids.push(id);
        }
        abp.message.confirm(l('UnlockConfirmationMessage')).then(function (confirmed) {
            if (confirmed) {
                dolphin.freight.importExport.oceanExports.oceanExportMbl.selectedUnLockedOceanExportMbl(ids)
                    .done(function () {
                        abp.message.success(l('Message:SuccessUnlock'));
                        dataTable.ajax.reload();
                    });
            }
        });
    }

}

$(function () {
    dolphin.freight.web.controllers.configuration.getJsonConfig('OceanExports').done(function (data) {
        data.forEach(function (item) {

            if (!item.lock && item.checkable) {
                var column;

                if (item.text.toLowerCase().includes('islocked')) {
                    column = {
                        //是否鎖定
                        title: l('Status'),
                        orderable: false,
                        render: function (data, type, row, meta) {
                            if (row.isLocked)
                                return "<a href='javascript:lock(\"" + row.id + "\")' class='btn-lock' id='lock_" + row.id + "'><i class='fa-lg fa-solid fa-lock fa-lg'></i><span>解鎖</span></a>";
                            else
                                return "<a href='javascript:lock(\"" + row.id + "\")' class='btn-lock action' id='lock_" + row.id + "'><i class='fa-lg fa-solid fa-lock-open'></i><span>上鎖</span></a>";
                        }
                    }
                }
                else {
                    column = {
                        title: l(item.text),
                        data: item.name
                    };
                }
                columns.push(column);
            }
        });

        var col = (columns.length > 2) ? [[2, 'asc']] : [[0, 'asc']];


        dataTable = $('#MblListTable').DataTable(
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
                ajax: abp.libs.datatables.createAjax(dolphin.freight.importExport.oceanExports.oceanExportMbl.queryList, queryListFilter),
                columnDefs: columns
            })
        );
    })



    $('#Search').keyup(function () {
        clearInterval(_changeInterval)
        _changeInterval = setInterval(function () {
            dataTable.ajax.reload();
            clearInterval(_changeInterval)
        }, 1000);
    });
    $('#Search').click(function (e) {
        dataTable.ajax.reload();
    });
    $('#CancelFilter').click(function (e) {
        $("#Keyword").val(null);
       
        // $("#VesselSchedule_OfficeId").val(null);
        $("select[name='VesselSchedule_OverseaAgentId']").val(null);
        $("select[name = 'VesselSchedule_CarrierId']").val(null).trigger("change");
        $("select[name='VesselSchedule_ShippingAgentId']").val(null).trigger("change");
        $("select[name='VesselSchedule_ForwardingAgentId']").val(null).trigger("change");
        
        $("select[name='VesselSchedule_PolId']").val(null).trigger("change");
        $("select[name='VesselSchedule_PodId']").val(null).trigger("change");
        $("select[name='VesselSchedule_DelId']").val(null).trigger("change");
        $("select[name='VesselSchedule_DeliverToId']").val(null).trigger("change");
        $("select[name='VesselSchedule_ShipModeId']").val(null).trigger("change");
        $("#BlCancelled").val(null).trigger("change");
        $("#PostDate").val(null);
        $("#ETA").val(null);
        $("#ETD").val(null);
        $("#CreationDate").val(null);
        $("#Vessel").val(null);
        dataTable.ajax.reload();
    });
    $('#NewMblButton').click(function (e) {
        location.href = 'CreateMbl';
    });

    $('#btnConfiguration').click(function (e) {
        var _configurationModal = new abp.ModalManager({
            viewUrl: abp.appPath + 'Configuration',
            modalClass: 'ConfigurationViewModel'
        });

        _configurationModal.open({
            src: 'OceanExports'
        });
    })

});

var lock = function (id) {
    var isLock = $('#lock_' + id).find('i').hasClass('fa-lock');
    abp.message.confirm(l(isLock ? 'UnlockConfirmationMessage' : 'LockConfirmationMessage')).then(function (confirmed) {
        if (confirmed) {
            dolphin.freight.importExport.oceanExports.oceanExportMbl.lockedOrUnLockedOceanExportMbl({ MbId: id })
                .done(function () {
                    if (isLock) {
                        abp.message.success(l('Message:SuccessUnlock'));
                    } else {
                        abp.message.success(l('Message:SuccessLock'));
                    }
                    dataTable.ajax.reload();
                });
        }
    });
}