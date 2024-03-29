﻿var l = abp.localization.getResource('Freight');
var dataTable;
var _changeInterval = null;
var queryListFilter = function () {
    return {
        search: $("#Keyword").val(),

        ovearseaAgentId: $("#VesselSchedule_OverseaAgentId").val() == '' ? null : $("#VesselSchedule_OverseaAgentId").val(),
        shipperId: $("#VesselSchedule_ShipperId").val() == '' ? null : $("#VesselSchedule_ShipperId").val(),
        notify: $("#VesselSchedule_NotifyId").val() == '' ? null : $("#VesselSchedule_NotifyAgentId").val(),
        consigneeId: $("#VesselSchedule_ConsigneeId").val() == '' ? null : $("#VesselSchedule_ConsigneeId").val(),
        finalDestId: $("#VesselSchedule_FinalDestId").val() == '' ? null : $("#VesselSchedule_FinalDestId").val(),
        truckerId: $("#VesselSchedule_TruckerId").val() == '' ? null : $("#VesselSchedule_TruckerId").val(),
        svcTermId: $("#VesselSchedule_SvcTermId").val() == '' ? null : $("#VesselSchedule_SvcTermId").val(),
        customerId: $("#VesselSchedule_CustomerId").val() == '' ? null : $("#VesselSchedule_CustomerId").val(),
        porId: $("#VesselSchedule_PorId").val() == '' ? null : $("#VesselSchedule_PorId").val(),
        pod: $("#VesselSchedule_PodId").val() == '' ? null : $("#VesselSchedule_PodId").val(),
       
        pol: $("#VesselSchedule_PolId").val() == '' ? null : $("#VesselSchedule_PolId").val(),
        del: $("#VesselSchedule_DelId").val() == '' ? null : $("#VesselSchedule_DelId").val(),
       
        shipModeId: $("#VesselSchedule_ShipModeId").val() == '' ? null : $("#VesselSchedule_ShipModeId").val(),
        saleId: $("#VesselSchedule_SaleId").val() == '' ? null : $("#VesselSchedule_SaleId").val(),
       

        eta: $("#ETA").val() == '' || $("#ETA").val() == null ? null : new Date($("#ETA").val()),
        etd: $("#ETD").val() == '' || $("#ETD").val() == null ? null : new Date($("#ETD").val()),
        delEta: $("#DelEta").val() == '' || $("#DelEta").val() == null ? null : new Date($("#DelEta").val()),
        finalDestEta: $("#FinalDestEta").val() == '' || $("#FinalDestEta").val() == null ? null : new Date($("#FinalDestEta").val()),
        creationDate: $("#CreationDate").val() == '' || $("#CreationDate").val() == null ? null : new Date($("#CreationDate").val()),
      
    };
};
var columns = [
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
    {
        title: '<div  style=" cursor: pointer;"><span><i class="fa fa-lock"></i></span></div>',
        orderable: false,
        "render": function (data, type, row) {
            var isCkecked = row.isLocked;
            var Hblid = row.id;
            if (isCkecked) {
                return '<input type="checkbox" class="lockUnlockCheckbox" data-id="' + Hblid + '"  checked="' + isCkecked + '" onclick="lockCheckBox(this)"  style=" cursor: pointer;">';
            } else {
                return '<input type="checkbox" class="lockUnlockCheckbox" data-id="' + Hblid + '" onclick="lockCheckBox(this)"   style=" cursor: pointer;">';
            }
        }
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
                            if (data.record.isLocked) {

                            }
                            location.href = 'EditModal?Id=' + data.record.mblId + '&Hid=' + data.record.id;

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
                                            dolphin.freight.importExport.oceanExports.oceanExportHbl
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

                            /*
                            if (!data.record.isLocked) {
                                dolphin.freight.importExport.oceanExports.oceanExportMbl
                                    .delete(data.record.id)
                                    .then(function () {
                                        abp.message.success(l('SuccessfullyDeleted'));
                                        dataTable.ajax.reload();
                                    });
                            } else {
                                abp.message.warn("鎖定不能刪除")
                            }*/

                        }
                    }
                ]
        }
    },

]

function deleteHbl() {
    var ids = [];
    var selectedCheckboxes = $('#HblListTable tbody input.selectCheckbox[type="checkbox"]:checked');
    for (var i = 0; i < selectedCheckboxes.length; i++) {
        var id = selectedCheckboxes[i].attributes[2].value;
        ids.push(id);

        abp.message.confirm(l('DeleteConfirmationMessage')).then(function (confirmed) {
            if (confirmed) {
                dolphin.freight.importExport.oceanExports.oceanExportHbl.deleteMultipleHbls(ids)
                    .done(function () {
                        abp.message.success(l('Message:SuccessDelete'));
                        $('#deleteBtn').prop('disabled', confirmed);
                        dataTable.ajax.reload();
                    });
            }
        });
    }
}

function selectAllCheckbox(element) {
    var isChecked = $(element).prop('checked');
    $('#HblListTable tbody input.selectCheckbox[type="checkbox"]').prop('checked', isChecked);

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
        $('#HblListTable tbody input.selectCheckbox[type="checkbox"]').each(function () {
            if (!$(this).prop('checked')) {
                allChecked = false;
                return false;
            }
        });
        $('#selectAllCheckbox').prop('checked', allChecked);
    }
}

function setSelectedLockStatus(isLock) {
    var ids = [];
    var selectedCheckboxes = $('#HblListTable tbody input.selectCheckbox[type="checkbox"]:checked');
    var lockCondition = isLock ? false : true;
    var confirmationMessage = isLock ? l('LockConfirmationMessage') : l('UnlockConfirmationMessage');
    var successMessage = isLock ? l('Message:SuccessLock') : l('Message:SuccessUnlock');

    for (var i = 0; i < selectedCheckboxes.length; i++) {
        var id = selectedCheckboxes[i].attributes[2].value;
        var currentLockStatus = $('#lock_' + id).find('i').hasClass('fa-lock');

        if (currentLockStatus === lockCondition) {
            ids.push(id);
        }
    }

    abp.message.confirm(confirmationMessage).then(function (confirmed) {
        if (confirmed) {
            dolphin.freight.importExport.oceanExports.oceanExportHbl.setLockStatusOnOceanExportHbl(ids, isLock).done(function () {
                abp.message.success(successMessage);
                $('#lockId').prop('disabled', confirmed);
                $('#unlockId').prop('disabled', confirmed);
                dataTable.ajax.reload();
            });
        }
    });
}

function lockCheckBox(checkbox) {
    var selectedCheckboxes = $('#HblListTable tbody input.lockUnlockCheckbox[type="checkbox"]:checked');

    var id = checkbox.attributes[2].value;

    var isLock = $('#lock_' + id).find('i').hasClass('fa-lock');
    abp.message.confirm(l(isLock ? 'UnlockConfirmationMessage' : 'LockConfirmationMessage')).then(function (confirmed) {
        if (confirmed) {
            dolphin.freight.importExport.oceanExports.oceanExportHbl.lockedOrUnLockedOceanExportHbl({ HblId: id })
                .done(function () {
                    if (isLock) {
                        abp.message.success(l('Message:SuccessUnlock'));
                    } else {
                        abp.message.success(l('Message:SuccessLock'));
                    }
                    dataTable.ajax.reload();
                });
        }
    
            else {
            debugger
                if(checkbox.checked) {
        $(checkbox).prop('checked', false);
    }
                else {
        $(checkbox).prop('checked', true);
    }
}

    });
}

$(function () {
    dolphin.freight.web.controllers.configuration.getJsonConfig('OceanExportHblList').done(function (data) {
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

        dataTable = $('#HblListTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
                pagingType: 'full_numbers',
                order: col,
                searching: false,
                scrollX: true,
                processing: true,
                responsive: {
                    details: {
                        type: 'column'
                    }
                },
                ajax: abp.libs.datatables.createAjax(dolphin.freight.importExport.oceanExports.oceanExportHbl.queryList, queryListFilter),
                columnDefs: columns
            })
        );

        $('#Search').click(function (e) {
            dataTable.ajax.reload();
        });

        $('#btnConfiguration').click(function (e) {
            var _configurationModal = new abp.ModalManager({
                viewUrl: abp.appPath + 'Configuration',
                modalClass: 'ConfigurationViewModel'
            });

            _configurationModal.open({
                src: 'OceanExportHblList'
            });
        })

        $('#CancelFilter').click(function (e) {
            $("#Keyword").val(null);

            // $("#VesselSchedule_OfficeId").val(null);
            $("select[name='VesselSchedule_OverseaAgentId']").val(null);
            $("select[name ='ShipperId']").val(null).trigger("change");
            $("select[name='VesselSchedule_NotifyId']").val(null).trigger("change");
            $("select[name='VesselSchedule_ConsigneeId']").val(null).trigger("change");
            $("select[name='VesselSchedule_PorId']").val(null).trigger("change");
            $("select[name='VesselSchedule_PolId']").val(null).trigger("change");
            $("select[name='VesselSchedule_PodId']").val(null).trigger("change");
            $("select[name='VesselSchedule_DelId']").val(null).trigger("change");
            $("select[name='VesselSchedule_DeliverToId']").val(null).trigger("change");
            $("select[name='VesselSchedule_ShipModeId']").val(null).trigger("change");
            $("select[name='VesselSchedule_CustomerId']").val(null).trigger("change");
            $("select[name='VesselSchedule_SvcTermId']").val(null).trigger("change");
            $("select[name='VesselSchedule_SaleId']").val(null).trigger("change");
            $("select[name='VesselSchedule_FinalDestId']").val(null).trigger("change");
            $("#PostDate").val(null);
            $("#ETA").val(null);
            $("#ETD").val(null);
            $("#DelEta").val(null);
            $("#FinalDestEta").val(null);
            $("#CreationDate").val(null);
            $("#Vessel").val(null);
            dataTable.ajax.reload();
        });
    });
})

var lock = function (id) {
    var isLock = $('#lock_' + id).find('i').hasClass('fa-lock');
    abp.message.confirm(l(isLock ? 'UnlockConfirmationMessage' : 'LockConfirmationMessage')).then(function (confirmed) {
        if (confirmed) {
            dolphin.freight.importExport.oceanExports.oceanExportHbl.lockedOrUnLockedOceanExportHbl({ HblId: id })
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

