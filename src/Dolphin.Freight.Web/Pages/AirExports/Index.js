var l = abp.localization.getResource('Freight');
var dataTable;
var copyModalMawbList = new abp.ModalManager({
    viewUrl: '/AirExports/CopyModalMawbList',
});
$(function () {
    var l = abp.localization.getResource('Freight');
   
    var _changeInterval = null;
    var queryListFilter = function () {
        return {
            search: $("#Keyword").val(),


            shipperId: $("#VesselSchedule_ShipperId").val() == '' ? null : $("#VesselSchedule_ShipperId").val(),

            consigneeId: $("#VesselSchedule_ConsigneeId").val() == '' ? null : $("#VesselSchedule_ConsigneeId").val(),
            carrierId: $("#VesselSchedule_CarrierId").val() == '' ? null : $("#VesselSchedule_CarrierId").val(),
            depatureId: $("#VesselSchedule_DepatureId").val() == '' ? null : $("#VesselSchedule_DepatureId").val(),
            destinationId: $("#VesselSchedule_DepatureId").val() == '' ? null : $("#VesselSchedule_DepatureId").val(),
            awbCancelled: $("#AwbCancelled").val() == '' ? null : $("#AwbCancelled").val(),
            directMaster: $("#DirectMaster").val() == '' ? null : $("#DirectMaster").val(),
            flightNo: $("#FlightNo").val(),
            officeId: $("#VesselSchedule_OfficeId").val() == '' ? null : $("#VesselSchedule_OfficeId").val(),
            incotermsType: $("#IncotermsType").val() == '' ? null : $("#IncotermsType").val(),
            awbType: $("#awbType").val() == '' ? null : $("#awbType").val(),
            block: $("#block").val() == '' ? null : $("#block").val(),

            depatureDate: $("#DepatureDate").val() == '' || $("#DepatureDate").val() == null ? null : new Date($("#DepatureDate").val()),
            arrivalDate: $("#ArrivalDate").val() == '' || $("#ArrivalDate").val() == null ? null : new Date($("#ArrivalDate").val()),
            postDate: $("#PostDate").val() == '' || $("#PostDate").val() == null ? null : new Date($("#PostDate").val()),
            creationDate: $("#CreationDate").val() == '' || $("#CreationDate").val() == null ? null : new Date($("#CreationDate").val()),

        };
    };
    var columns = [{
        title: '<input type="checkbox" id="selectAllCheckbox" onclick="AirExports.selectAllCheckbox(this)" style=" cursor: pointer;">',
        data: null,
        orderable: false,
        "render": function (data, type, row) {

            var id = row.id;
            var filingNo = row.filingNo;

            return '<input type="checkbox" class="selectCheckbox" data-id="' + id + '" data-filingNo="' + filingNo + '" onclick="AirExports.selectCheckbox(this)" style=" cursor: pointer;">';
        }
    },
    {
        title: '<div  style=" cursor: pointer;"><span><i class="fa fa-lock"></i></span></div>',
        orderable: false,
        "render": function (data, type, row) {
            var isCkecked = row.isLocked;
            var id = row.id;
            if (isCkecked) {
                return '<input type="checkbox" class="lockUnlockCheckbox" data-id="' + id + '"  checked="' + isCkecked + '" onclick="AirExports.lockCheckBox(this)" style=" cursor: pointer;">';
            } else {
                return '<input type="checkbox" class="lockUnlockCheckbox" data-id="' + id + '" onclick="AirExports.lockCheckBox(this)" style=" cursor: pointer;">';
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
                                            dolphin.freight.importExport.airExports.airExportMawb
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
    }]

    var otherFixedColumns = [{
        title: l('grossWeightKg'),
        data: "grossWeightKg"
    },
    {
        title: l('grossWeightLb'),
        data: "grossWeightLb"
    },
    {
        title: l('chargeableWeightKg'),
        data: "chargeableWeightKg"
    },
    {
        title: l('chargeableWeightLb'),
        data: "chargeableWeightLb"
    }]

   

    dolphin.freight.web.controllers.configuration.getJsonConfig('AirExport').done(function (data) {
        data.forEach(function (item) {

            if (!item.lock && item.checkable) {
                var itemData = item.name;
                if (item.name.toLowerCase().includes('date')) {
                    itemData = function (row, type, set) {
                        var depatureDate = new Date(row.depatureDate);
                        if (depatureDate.getFullYear() == 1) {
                            return '';
                        }
                        return depatureDate.toLocaleDateString() + ' ' + depatureDate.toLocaleTimeString();
                    }
                }

                var column = {  
                    title: l(item.text),
                    data: itemData
                };
                columns.push(column);
            }
        });
        columns = columns.concat(otherFixedColumns);
        var col = (columns.length > 1) ? [[1, 'asc']] : [[0, 'asc']];

        dataTable = $('#MblListTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
                order: col,
                searching: false,
                scrollX: true,
                processing: true,
                ajax: abp.libs.datatables.createAjax(dolphin.freight.importExport.airExports.airExportMawb.queryList, queryListFilter),
                columnDefs: columns
            })
        );
    })

    $('#Search').click(function (e) {
        dataTable.ajax.reload();
    });

    $('#btnConfiguration').click(function (e) {
        var _configurationModal = new abp.ModalManager({
            viewUrl: abp.appPath + 'Configuration',
            modalClass: 'ConfigurationViewModel'
        });

        _configurationModal.open({
            src: 'AirExport'
        });
    })

});

var AirExports = {
    selectAllCheckbox: function(element){
        var isChecked = $(element).prop('checked');
        $('#MblListTable tbody input.selectCheckbox[type="checkbox"]').prop('checked', isChecked);
        
        if (isChecked) {
            $('#summaryId').prop('disabled', false);
            $('#detailedId').prop('disabled', false);
        
        
        } else {
            $('#summaryId').prop('disabled', true);
            $('#detailedId').prop('disabled', true);
        }
    },
    selectCheckbox: function (checkbox) {
        var checkedCheckboxes = $('.selectCheckbox:checked');
        if (checkbox.checked) {
            $('#summaryId').prop('disabled', false);
            $('#detailedId').prop('disabled', false);
        
            if (checkedCheckboxes.length == 1) {
                $('#copyId').prop('disabled', false);
            }
            else {
                $('#copyId').prop('disabled', true);
            }
            var isAnyLocked = false;
            var isAnyUnlocked = false
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
            $('#lockId').prop('disabled', !isAnyLocked);
            $('#unlockId').prop('disabled', !isAnyUnlocked);
        } else {
            $('#summaryId').prop('disabled', true);
            $('#detailedId').prop('disabled', true);
            var checkedCheckboxes = $('.selectCheckbox:checked');
            if (checkedCheckboxes.length == 1) {
                $('#copyId').prop('disabled', false);
            }
            else {
                $('#copyId').prop('disabled', true);
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
            $('#lockId').prop('disabled', !isAnyLocked);
            $('#unlockId').prop('disabled', !isAnyUnlocked);
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
    },
    lockCheckBox: function (checkbox) {
        var selectedCheckboxes = $('#MblListTable tbody input.lockUnlockCheckbox[type="checkbox"]:checked');

        var id = checkbox.attributes[2].value;
        
        var isLock = $('#lock_' + id).find('i').hasClass('fa-lock');
        abp.message.confirm(l(isLock ? 'UnlockConfirmationMessage' : 'LockConfirmationMessage')).then(function (confirmed) {
            if (confirmed) {
                dolphin.freight.importExport.airExports.airExportMawb.lockedOrUnLockedAirExportMawb(id).done(function () {
                    if (isLock) {
                        abp.message.success(l('Message:SuccessUnlock'));
                    } else {
                        abp.message.success(l('Message:SuccessLock'));
                    }
                    dataTable.ajax.reload();
                });
            }
        });
    },
    selectedLock: function () {
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
                    dolphin.freight.importExport.airExports.airExportMawb.selectedLockedAirExportMawb(ids).done(function () {
                        abp.message.success(l('Message:SuccessLock'));
                        dataTable.ajax.reload();
                    });
                }
            });
        }
    },
    selectedUnLock: function () {
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
                    dolphin.freight.importExport.airExports.airExportMawb.selectedUnLockedAirExportMawb(ids).done(function () {
                        abp.message.success(l('Message:Message:SuccessUnlock'));
                        dataTable.ajax.reload();
                    });
                }
            });
        }
    },
    createNew: function () {
        window.location.href = '/AirExports/CreateMawb';
    }
} 

var lock = function (id) {
    var isLock = $('#lock_' + id).find('i').hasClass('fa-lock');
    abp.message.confirm(l(isLock ? 'UnlockConfirmationMessage' : 'LockConfirmationMessage')).then(function (confirmed) {
        if (confirmed) {
            dolphin.freight.importExport.airExports.airExportMawb.lockedOrUnLockedAirExportMawb(id)
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