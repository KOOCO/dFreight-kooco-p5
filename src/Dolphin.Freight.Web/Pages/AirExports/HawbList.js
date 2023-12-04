var l = abp.localization.getResource('Freight');
var dataTable;
var _changeInterval = null;
var queryListFilter = function () {
    return {
        search: $("input[name='Search'").val()
    };
};

var columns = [{
    title: '<input type="checkbox" id="selectAllCheckbox" disable="true" onclick="selectAllCheckbox(this)" style=" cursor: pointer;">',
    data: null,
    orderable: false,
    render: function (data, type, row) {

        var id = row.hawbId;
        var filingNo = row.docNumber;
        $('#btnAirExportHawbListProfit').prop('disabled', true);
        $('#selectAllCheckbox').prop('checked', false);
        return '<input type="checkbox" class="selectCheckbox" data-id="' + id + '" data-mawbId="' + row.mawbId + '" data-filingNo="' + filingNo + '" onclick="selectCheckbox(this)" style=" cursor: pointer;">';
    }
},
{
    title: '<div  style=" cursor: pointer;"><span><i class="fa fa-lock"></i></span></div>',
    orderable: false,
    "render": function (data, type, row) {
        var isCkecked = row.isLocked;
        var id = row.mawbId;
        var hawbId = row.hawbId;
        if (isCkecked) {
            return '<input type="checkbox" class="lockUnlockCheckbox" data-hawbId="' + hawbId + '" data-id="' + id + '"  checked="' + isCkecked + '" onclick="lockCheckBox(this)"  style=" cursor: pointer;">';
        } else {
            return '<input type="checkbox" class="lockUnlockCheckbox" data-hawbId="' + hawbId + '" data-id="' + id + '" onclick="lockCheckBox(this)"   style=" cursor: pointer;">';
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
                        location.href = 'EditModal?Id=' + data.record.mawbId + '&Hid=' + data.record.id;

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
                                        dolphin.freight.importExport.airExports.airExportmawbNo
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

var dataTable;

function selectAllCheckbox(element) {
    var isChecked = $(element).prop('checked');
    $('#HawbListTable tbody input.selectCheckbox[type="checkbox"]').prop('checked', isChecked);

    if (isChecked) {
        $('#btnAirExportHawbListProfit').prop('disabled', false);
    } else {
        $('#btnAirExportHawbListProfit').prop('disabled', true);
    }
}
function lockCheckBox(checkbox) {
    var selectedCheckboxes = $('#HawbListTable tbody input.lockUnlockCheckbox[type="checkbox"]:checked');

    var id = checkbox.attributes[2].value;

    var isLock = $('#lock_' + id).find('i').hasClass('fa-lock');
    abp.message.confirm(l(isLock ? 'UnlockConfirmationMessage' : 'LockConfirmationMessage')).then(function (confirmed) {
        if (confirmed) {
            dolphin.freight.importExport.airExports.airExportHawb.lockedOrUnLockedAirExportHawb(id)
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
            if (checkbox.checked) {
                $(checkbox).prop('checked', false);
            }
            else {
                $(checkbox).prop('checked', true);
            }
        }
    });
}
function selectCheckbox(checkbox) {
    var checkedCheckboxes = $('.selectCheckbox:checked');
    if (checkbox.checked) {
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
        $('#lockId').prop('disabled', !isAnyUnlocked);
        $('#unlockId').prop('disabled', !isAnyLocked);
    }
    if (!$(checkbox).prop('checked')) {
        $('#selectAllCheckbox').prop('checked', false);
    } else {
        var allChecked = true;
        $('#HawbListTable tbody input.selectCheckbox[type="checkbox"]').each(function () {
            if (!$(this).prop('checked')) {
                allChecked = false;
                return false;
            }
        });
        $('#selectAllCheckbox').prop('checked', allChecked);
    }
}
function getHwabProfitReport(reportType) {
    var params = "";
    var selectedCheckboxes = $('#HawbListTable tbody input.selectCheckbox[type="checkbox"]:checked');
    for (var i = 0; i < selectedCheckboxes.length; i++) {
        var id = selectedCheckboxes[i].attributes['data-id'].value;
        var filingNo = selectedCheckboxes[i].attributes['data-filingNo'].value;


        params += id + ' / ' + filingNo + ',';
    }
    console.log(params);
    params = params.replace(/^,|,$/g, '');

    OpenWindow('/Docs/ProfitReportHawbListAirExport?pageType=@Dolphin.Freight.Common.FreightPageType.AEHBL&param=' + params);
}

function getDateTimeForAWB(data) {
    return luxon
        .DateTime
        .fromISO(data, {
            locale: abp.localization.currentCulture.name
        }).toFormat('yyyy-MM-dd HH:mm');
}

function selectedLock() {
    var ids = [];
    var selectedCheckboxes = $('#HawbListTable tbody input.selectCheckbox[type="checkbox"]:checked');
    for (var i = 0; i < selectedCheckboxes.length; i++) {
        var id = selectedCheckboxes[i].attributes[2].value;
        var isLock = $('#lock_' + id).find('i').hasClass('fa-lock')
        if (!isLock) {
            ids.push(id);
        }
        abp.message.confirm(l('LockConfirmationMessage')).then(function (confirmed) {
            if (confirmed) {
                dolphin.freight.importExport.airExports.airExportHawb.selectedLockedAirExportHawb(ids)
                    .done(function () {
                        abp.message.success(l('Message:SuccessLock'));
                        dataTable.ajax.reload();
                    });
            }
        });
    }

}

function selectedUnLock() {
    var ids = [];
    var selectedCheckboxes = $('#HawbListTable tbody input.selectCheckbox[type="checkbox"]:checked');
    for (var i = 0; i < selectedCheckboxes.length; i++) {
        var id = selectedCheckboxes[i].attributes[2].value;
        var isLock = $('#lock_' + id).find('i').hasClass('fa-lock')
        if (isLock) {
            ids.push(id);
        }
        abp.message.confirm(l('UnlockConfirmationMessage')).then(function (confirmed) {
            if (confirmed) {
                dolphin.freight.importExport.airExports.airExportHawb.selectedUnLockedAirExportHawb(ids)
                    .done(function () {
                        abp.message.success(l('Message:SuccessUnlock'));
                        dataTable.ajax.reload();
                    });
            }
        });
    }

}
function onChangeSelection(e) {
    if ($("#HawbListTable input[type=checkbox]:checked").length > 0) {
        document.getElementById("btnAirExportHawbListProfit").style.display = 'block';;
    } else { document.getElementById("btnAirExportHawbListProfit").style.display = 'none'; }
}
$(function () {
    dolphin.freight.web.controllers.configuration.getJsonConfig('AirExportHawbList').done(function (data) {
        data.forEach(function (item) {
            if (!item.lock && item.checkable) {
                columns.push(createColumnBasedOnText(item.text));
            }
        });

        dataTable = $('#HawbListTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
                pagingType: 'full_numbers',
                order: [[2, "asc"]],
                searching: false,
                scrollX: true,
                processing: true,
                responsive: {
                    details: {
                        type: 'column'
                    }
                },
                ajax: abp.libs.datatables.createAjax(dolphin.freight.importExport.airExports.airExportHawb.queryList, queryListFilter),
                columnDefs: columns
            })
        );
    });
    function createColumnBasedOnText(text) {
        if (text.toLowerCase().includes('islocked')) {
            return {
                title: '<div style=" cursor: pointer;"><span><i class="fa fa-unlock"></i>Status</span></div>',
                orderable: false,
                render: function (data, type, row, meta) {
                    let action = row.isLocked ? { icon: 'fa-lock', text: '解鎖', id: 'lock_' + row.hawbId } : { icon: 'fa-lock-open', text: '上鎖', id: 'lock_' + row.hawb };
                    return `<a href='javascript:lock("${action.id}")' class='btn-lock action' id='${action.id}'><i class='fa-lg fa-solid ${action.icon} fa-lg'></i><span>${action.text}</span></a>`;
                }
            }
        }
        else {
            const fieldMappings = {
                'File No.': 'docNumber',
                'HAWB Number': 'hawbNo',
                'MawbId': 'mawbId',
                'Departure': 'departureName',
                'Depature Date/Time': 'depatureDate',
                'Arrival Date/Time': 'arrivalDate',
                'AES': 'aes',
                'Destination': 'destinationName',
                'ActualShipper': 'shippperName',
                'Consignee (Oversea Agent)': 'consigneeName',
                'OverSea Agent': 'overSeaAgentName',
                'AR Balance': 'arTotal',
                'AP Balance': 'apTotal',
                'DC Balance': 'dcTotal',
                'Booking number': 'bookingNumber',
                'Operator': 'oP',
                'Status': 'status'
            };

            const fieldTypes = {
                data: ['File No.', 'HAWB Number', 'MawbId', 'Departure', 'AES', 'Destination', 'ActualShipper', 'Consignee (Oversea Agent)', 'OverSea Agent', 'Booking number', 'Operator', 'Status'],
                date: ['Depature Date/Time', 'Arrival Date/Time'],
                float: ['AR Balance', 'AP Balance', 'DC Balance']
            }

            let dataType = null;
            let fieldName = null;

            for (const type in fieldTypes) {
                for (const field of fieldTypes[type]) {
                    if (text.includes(field)) {
                        dataType = type;
                        fieldName = fieldMappings[field];
                        break;
                    }
                }
                if (dataType) break;
            }

            return {
                title: l(text),
                data: createGenericFunction(dataType, fieldName)
            };
        }
    }
    function createGenericFunction(dataType, fieldName) {
        return function (row, type, set) {
            switch (dataType) {
                case 'data':
                    return row[fieldName] || '-';
                case 'date':
                    var dateValue = new Date(row[fieldName]);
                    return (dateValue.getFullYear() == 1) ? '-' : dateValue.toLocaleDateString();
                case 'float':
                    return row[fieldName] ? parseFloat(row[fieldName]).toFixed(2) : '-';
            }
        }
    }

    $('#Search').keyup(function () {
        clearInterval(_changeInterval)
        _changeInterval = setInterval(function () {
            dataTable.ajax.reload();
            clearInterval(_changeInterval)
        }, 1000);
    });

    $('#btnConfiguration').click(function (e) {
        var _configurationModal = new abp.ModalManager({
            viewUrl: abp.appPath + 'Configuration',
            modalClass: 'ConfigurationViewModel'
        });

        _configurationModal.open({
            src: 'AirExportHawbList'
        });
    })
});

