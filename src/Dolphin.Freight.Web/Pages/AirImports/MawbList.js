var l = abp.localization.getResource('Freight');
var dataTable;
var copyModalMawbList = new abp.ModalManager({
    viewUrl: '/AirImports/CopyModalMawbList',
});
$(function () {
  
    var _changeInterval = null;
    var queryListFilter = function () {
        return {
            search: $("#Keyword").val(),


            freightLocationId: $("#VesselSchedule_FreightLocationId").val() == '' ? null : $("#VesselSchedule_FreightLocationId").val(),

            overseaAgentId: $("#VesselSchedule_OverseaAgentId").val() == '' ? null : $("#VesselSchedule_OverseaAgentId").val(),
            carrierId: $("#VesselSchedule_CarrierId").val() == '' ? null : $("#VesselSchedule_CarrierId").val(),
            depatureId: $("#VesselSchedule_DepatureId").val() == '' ? null : $("#VesselSchedule_DepatureId").val(),
            destinationId: $("#VesselSchedule_DepatureId").val() == '' ? null : $("#VesselSchedule_DepatureId").val(),
            directMaster: $("#DirectMaster").val() == '' ? null : $("#DirectMaster").val(),
            block: $("#block").val() == '' ? null : $("#block").val(),
            flightNo: $("#FlightNo").val(),
            officeId: $("#VesselSchedule_OfficeId").val() == '' ? null : $("#VesselSchedule_OfficeId").val(),
            awbType: $("#AwbType").val() == '' ? null : $("#AwbType").val(),

            creatorId: $("#VesselSchedule_CreatorId").val() == '' ? null : $("#VesselSchedule_CreatorId").val(),
            depatureDate: $("#DepatureDate").val() == '' || $("#DepatureDate").val() == null ? null : new Date($("#DepatureDate").val()),
            arrivalDate: $("#ArrivalDate").val() == '' || $("#ArrivalDate").val() == null ? null : new Date($("#ArrivalDate").val()),
            postDate: $("#PostDate").val() == '' || $("#PostDate").val() == null ? null : new Date($("#PostDate").val()),
            creationDate: $("#CreationDate").val() == '' || $("#CreationDate").val() == null ? null : new Date($("#CreationDate").val()),

        };
    };
    var columns = [
        {
            title: '<input type="checkbox" id="selectAllCheckbox" onclick="AirImportsMawbList.selectAllCheckbox(this)" style=" cursor: pointer;">',
            data: null,
            orderable: false,
            "render": function (data, type, row) {
                var id = row.id;
                var filingNo = row.filingNo;
                return '<input type="checkbox" class="selectCheckbox" data-id="' + id + '" data-filingNo="' + filingNo + '" onclick="AirImportsMawbList.selectCheckbox(this)" style=" cursor: pointer;">';
            }
        },
        {
            title: '<div  style=" cursor: pointer;"><span><i class="fa fa-lock"></i></span></div>',
            orderable: false,
            "render": function (data, type, row) {
                var isCkecked = row.isLocked;
                var id = row.id;
                if (isCkecked) {
                    return '<input type="checkbox" class="lockUnlockCheckbox" data-id="' + id + '"  checked="' + isCkecked + '" onclick="AirImportsMawbList.lockCheckBox(this)"  style=" cursor: pointer;">';
                } else {
                    return '<input type="checkbox" class="lockUnlockCheckbox" data-id="' + id + '" onclick="AirImportsMawbList.lockCheckBox(this)"   style=" cursor: pointer;">';
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
                            location.href = 'EditMawb?Id=' + data.record.id;

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
                                            dolphin.freight.importExport.airImports.airImportMawb
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
    dolphin.freight.web.controllers.configuration.getJsonConfig('AirImportMawbList').done(function (data) {
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

        var col = (columns.length > 1) ? [[1, 'asc']] : [[0, 'asc']];

        dataTable = $('#MawbListTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
                pagingType: 'full_numbers',
                order: col,
                searching: false,
                scrollX: true,
                processing: true,
                ajax: abp.libs.datatables.createAjax(dolphin.freight.importExport.airImports.airImportMawb.getList, queryListFilter),
                columnDefs: columns
            })
        );

    })

   
    $('#Search').click(function (e) {
        dataTable.ajax.reload();
    });
    

    $('#AddMawbButton').click(function (e) {
        window.location = "\CreateMawb";
    });
    $('#btnConfiguration').click(function (e) {
        var _configurationModal = new abp.ModalManager({
            viewUrl: abp.appPath + 'Configuration',
            modalClass: 'ConfigurationViewModel'
        });

        _configurationModal.open({
            src: 'AirImportMawbList'
        });
    })
});
var AirImportsMawbList = {
    selectAllCheckbox: function (element) {
        var isChecked = $(element).prop('checked');
        $('#MawbListTable tbody input.selectCheckbox[type="checkbox"]').prop('checked', isChecked);
        var checkedCheckboxes = $('.selectCheckbox:checked');
        var isAnyLocked = false;
        var isAnyUnlocked = false
        if (isChecked) {
            $('#summaryId').prop('disabled', false);
            $('#detailedId').prop('disabled', false);
            debugger;
            checkedCheckboxes.each(function (index, checkbox1) {
                $('#deleteBtn').prop('disabled', false);
                var id = $(checkbox1).data('id');
                var isLock = $('.lockUnlockCheckbox[data-id="' + id + '"]').prop('checked');

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
            $('#summaryId').prop('disabled', true);
            $('#detailedId').prop('disabled', true);
            checkedCheckboxes.each(function (index, checkbox1) {
                var id = $(checkbox1).data('id');
                var isLock = $('.lockUnlockCheckbox[data-id="' + id + '"]').prop('checked');
                if (isLock) {
                    isAnyLocked = true;
                }
                else {
                    isAnyUnlocked = true;
                }
            });
            $('#lockId').prop('disabled', !isAnyUnlocked);
            $('#unlockId').prop('disabled', !isAnyLocked);
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
                $('#deleteBtn').prop('disabled', false);
                var id = $(checkbox1).data('id');
                var isLock = $('.lockUnlockCheckbox[data-id="' + id + '"]').prop('checked');

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
            $('#summaryId').prop('disabled', true);
            $('#detailedId').prop('disabled', true);
            var checkedCheckboxes = $('.selectCheckbox:checked');
            if (checkedCheckboxes.length == 1) {
                $('#copyId').prop('disabled', false);
            }
            else {
                $('#copyId').prop('disabled', true);
            }
            if (checkedCheckboxes.length == 1 || checkedCheckboxes.length > 1) {
                $('#deleteBtn').prop('disabled', false);
            }
            else {

                $('#deleteBtn').prop('disabled', true);
            }
            checkedCheckboxes.each(function (index, checkbox1) {
                var id = $(checkbox1).data('id');
                var isLock = $('.lockUnlockCheckbox[data-id="' + id + '"]').prop('checked');
                if (isLock) {
                    isAnyLocked = true;
                }
                else {
                    isAnyUnlocked = true;
                }
            });
            $('#lockId').prop('disabled', !isAnyUnlocked);
            $('#unlockId').prop('disabled', !isAnyLocked);
            
        }
        if (!$(checkbox).prop('checked')) {
            $('#selectAllCheckbox').prop('checked', false);
        } else {
            var allChecked = true;
            $('#MawbListTable tbody input.selectCheckbox[type="checkbox"]').each(function () {
                if (!$(this).prop('checked')) {
                    allChecked = false;
                    return false;
                }
            });
            $('#selectAllCheckbox').prop('checked', allChecked);
        }
    },
    lockCheckBox: function (checkbox) {
        var selectedCheckboxes = $('#MawbListTable tbody input.lockUnlockCheckbox[type="checkbox"]:checked');
        var id = checkbox.attributes[2].value;
        var isLock = $('#lock_' + id).find('i').hasClass('fa-lock');
        abp.message.confirm(l(isLock ? 'UnlockConfirmationMessage' : 'LockConfirmationMessage')).then(function (confirmed) {
            if (confirmed) {
                dolphin.freight.importExport.airImports.airImportMawb.lockedOrUnLockedAirImportMawb(id).done(function () {
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
                if (checkbox.checked) {
                    $(checkbox).prop('checked', false);
                }
                else {
                    $(checkbox).prop('checked', true);
                }
            }
        });
    },
    selectedLock: function () {
        var ids = [];
        var selectedCheckboxes = $('#MawbListTable tbody input.selectCheckbox[type="checkbox"]:checked');
        for (var i = 0; i < selectedCheckboxes.length; i++) {
            var id = selectedCheckboxes[i].attributes[2].value;
            var isLock = $('.lockUnlockCheckbox[data-id="' + id + '"]').prop('checked');
            if (!isLock) {
                ids.push(id);
            }
            abp.message.confirm(l('LockConfirmationMessage')).then(function (confirmed) {
                if (confirmed) {
                    dolphin.freight.importExport.airImports.airImportMawb.selectedLockedAirImportMawb(ids).done(function () {
                        debugger;
                        abp.message.success(l('Message:SuccessLock'));
                        dataTable.ajax.reload();
                        $('#selectAllCheckbox').prop('checked', false);
                    });
                }
            });
        }
    },
    selectedUnLock: function () {
        var ids = [];
        var selectedCheckboxes = $('#MawbListTable tbody input.selectCheckbox[type="checkbox"]:checked');
        for (var i = 0; i < selectedCheckboxes.length; i++) {
            var id = selectedCheckboxes[i].attributes[2].value;
            var isLock = $('.lockUnlockCheckbox[data-id="' + id + '"]').prop('checked');
            if (isLock) {
                ids.push(id);
            }
            abp.message.confirm(l('UnlockConfirmationMessage')).then(function (confirmed) {
                if (confirmed) {
                    dolphin.freight.importExport.airImports.airImportMawb.selectedUnLockedAirImportMawb(ids).done(function () {
                        dataTable.ajax.reload();
                        abp.message.success(l('Message:Message:SuccessUnlock'));
                        $('#selectAllCheckbox').prop('checked', false);
                    });
                }
            });
        }
    },
    openCopyModal: function () {
        var selectedCheckboxes = $('#MawbListTable tbody input.selectCheckbox[type="checkbox"]:checked');
        var id = selectedCheckboxes[0].attributes[2].value;
        copyModalMawbList.open({
            id,
        });
    },
    getProfitReport: function (reportType) {
        var params = "";
        var selectedCheckboxes = $('#MawbListTable tbody input.selectCheckbox[type="checkbox"]:checked');
        for (var i = 0; i < selectedCheckboxes.length; i++) {
            var id = selectedCheckboxes[i].attributes[2].value;
            var filingNo = selectedCheckboxes[i].attributes[3].value;
            params += id + ' / ' + filingNo + ',';
        }
        params = params.replace(/^,|,$/g, '');
        OpenWindow('/Docs/ProfitReportMawbListAirImport?reportType=' + reportType + '&pageType=@Dolphin.Freight.Common.FreightPageType.AIMBL&param=' + params);
    },
    createNew: function () {
        window.location.href = "/AirImports/CreateMawb";
    }
}
var lock = function (id) {
    var isLock = $('#lock_' + id).find('i').hasClass('fa-lock');
    abp.message.confirm(l(isLock ? 'UnlockConfirmationMessage' : 'LockConfirmationMessage')).then(function (confirmed) {
        if (confirmed) {
            dolphin.freight.importExport.airImports.airImportMawb.lockedOrUnLockedAirImportMawb(id)
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
function deleteMbls() {
    var ids = [];
    var selectedCheckboxes = $('#MawbListTable tbody input.selectCheckbox[type="checkbox"]:checked');
    for (var i = 0; i < selectedCheckboxes.length; i++) {
        var id = selectedCheckboxes[i].attributes[2].value;
        var isLock = $('.lockUnlockCheckbox[data-id="' + id + '"]').prop('checked');
        if (!isLock) {
            ids.push(id);
        }
        abp.message.confirm(l('DeleteConfirmationMessage')).then(function (confirmed) {
            if (confirmed) {
                dolphin.freight.importExport.airImports.airImportMawb.deleteMultipal(ids)
                    .done(function () {
                        abp.message.success(l('Message:SuccessDelete'));
                        dataTable.ajax.reload();
                    });
            }
        });
    }
}