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
        return '<input type="checkbox" class="selectCheckbox" data-id="' + id + '" data-filingNo="' + filingNo + '" onclick="selectCheckbox(this)" style=" cursor: pointer;">';
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
},
{
    //是否鎖定
    title: l('IsLocked'),
    orderable: false,
    render: function (data, type, row, meta) {
        if (row.isLocked)
            return "<a href='javascript:lock(\"" + row.id + "\")' class='btn-lock' id='lock_" + row.id + "'><i class='fa-lg fa-solid fa-lock fa-lg'></i><span>解鎖</span></a>";
        else
            return "<a href='javascript:lock(\"" + row.id + "\")' class='btn-lock action' id='lock_" + row.id + "'><i class='fa-lg fa-solid fa-lock-open'></i><span>上鎖</span></a>";
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
function selectCheckbox(checkbox) {
    if ($("#HawbListTable input[type=checkbox]:checked").length > 0) {
        $('#btnAirExportHawbListProfit').prop('disabled', false);
        //check if all rows are selected then make main header checkbox selected
        $('#selectAllCheckbox').prop('checked', false);
        if ($("#HawbListTable input.selectCheckbox[type=checkbox]").length == $("#HawbListTable input[type=checkbox]:checked").length) {
            $('#selectAllCheckbox').prop('checked', true);
        }
    } else {
        $('#btnAirExportHawbListProfit').prop('disabled', true);
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


function onChangeSelection(e) {
    if ($("#HawbListTable input[type=checkbox]:checked").length > 0) {
        document.getElementById("btnAirExportHawbListProfit").style.display = 'block';;
    } else { document.getElementById("btnAirExportHawbListProfit").style.display = 'none'; }
}
$(function () {
    dolphin.freight.web.controllers.configuration.getJsonConfig('AirExportHawbList').done(function (data) {
        data.forEach(function (item) {
            if (!item.lock && item.checkable) {
                var itemData = item.text;

                var fieldMappings = {
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
                }

                var dataFields = ['File No.', 'HAWB Number', 'MawbId', 'Departure', 'AES', 'Destination', 'ActualShipper', 'Consignee (Oversea Agent)', 'OverSea Agent', 'Booking number', 'Operator', 'Status'];
                dataFields.forEach(function (field) {
                    if (item.text.includes(field)) {
                        var actualFieldName = fieldMappings[field];
                        itemData = createGenericFunction('data', actualFieldName);
                    }
                });

                var dateFields = ['Depature Date/Time', 'Arrival Date/Time'];
                dateFields.forEach(function (field) {
                    if (item.text.includes(field)) {
                        var actualFieldName = fieldMappings[field];
                        itemData = createGenericFunction('date', actualFieldName);
                    }
                });

                var floatFields = ['AR Balance', 'AP Balance', 'DC Balance'];
                floatFields.forEach(function (field) {
                    if (item.text.includes(field)) {
                        var actualFieldName = fieldMappings[field];
                        itemData = createGenericFunction('float', actualFieldName);
                    }
                });

                var column = {
                    title: l(item.text),
                    data: itemData
                };
                columns.push(column);
            }
        });

        dataTable = $('#HawbListTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
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

    function createGenericFunction(dataType, fieldName) {
        return function (row, type, set) {
            switch (dataType) {
                case 'data':
                    return row[fieldName] || '-';
                case 'date':
                    var dateValue = new Date(row[fieldName]);
                    debugger;
                    if (dateValue.getFullYear() == 1) {
                        return '-';
                    }
                    return dateValue.toLocaleDateString();
                case 'float':
                    if (!row[fieldName]) {
                        return '-';
                    }
                    return parseFloat(row[fieldName]).toFixed(2);
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

