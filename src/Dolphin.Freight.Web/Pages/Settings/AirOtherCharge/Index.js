$(function () {
    var l = abp.localization.getResource('Freight');

    var dataTable = $('#AirOtherCharge').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(dolphin.freight.settings.airOtherCharge.airOtherCharge.getList),
            columns: [
                {
                    title: l('Actions'),
                    orderable: false,
                    searchable: false,
                    width: "100px",
                },
                {
                    //航空公司/代理
                    title: l('companyType'),
                    data: "companyType"
                },
                {
                    //運費到付/預付
                    title: l('paymentType'),
                    data: "paymentType"
                },
                {
                    //顯示於	
                    title: l('showingAt'),
                    data: "showOnMAWB",
                    width: "500px", // Set the width of the third column
                    render: function (data, type, row, meta) {
                        if (row.showOnMAWB == true) {
                            if (row.showOnHAWB == true) {
                                return '<div class="form-check form-check-inline"><input class="form-check-input" type="checkbox" id="showOnMAWBCheckBox" value="" checked disabled><label class="form-check-label" for="inlineCheckbox">MAWB</label></div><div class="form-check form-check-inline"><input class="form-check-input" type="checkbox" id="showOnHAWBCheckBox" value="" checked disabled><label class="form-check-label" for="inlineCheckbox">HAWB</label></div>';
                            } else {
                                return '<div class="form-check form-check-inline"><input class="form-check-input" type="checkbox" id="showOnMAWBCheckBox" value="" checked disabled><label class="form-check-label" for="inlineCheckbox">MAWB</label></div><div class="form-check form-check-inline"><input class="form-check-input" type="checkbox" id="showOnHAWBCheckBox" value="" disabled><label class="form-check-label" for="inlineCheckbox">HAWB</label></div>';
                            }
                        } else {
                            if (row.showOnHAWB = true) {
                                return '<div class="form-check form-check-inline"><input class="form-check-input" type="checkbox" id="showOnMAWBCheckBox" value="" disabled><label class="form-check-label" for="inlineCheckbox">MAWB</label></div><div class="form-check form-check-inline"><input class="form-check-input" type="checkbox" id="showOnHAWBCheckBox" value="" checked disabled><label class="form-check-label" for="inlineCheckbox">HAWB</label></div>';
                            } else {
                                return '<div class="form-check form-check-inline"><input class="form-check-input" type="checkbox" id="showOnMAWBCheckBox" value="" disabled><label class="form-check-label" for="inlineCheckbox">MAWB</label></div><div class="form-check form-check-inline"><input class="form-check-input" type="checkbox" id="showOnHAWBCheckBox" value="" disabled><label class="form-check-label" for="inlineCheckbox">HAWB</label></div>';
                            }

                        }


                    },
                },
                {
                    //顯示於
                    title: l('showOnHAWB'),
                    data: "showOnHAWB"
                },
                {
                    //計費項目	
                    title: l('chargeItem'),
                    data: "chargeItem"
                },
                {
                    //子計費項目	
                    title: l('chargeItemSubitem'),
                    data: "chargeItemSubitem"
                },
                {
                    //費率	
                    title: l('chargeRate'),
                    data: "chargeRate"
                },
                {
                    //收費費率單位
                    title: l('chargeRateUnit'),
                    data: "chargeRateUnit"
                },
                {
                    //最低收費
                    title: l('minPrice'),
                    data: "minPrice"
                },
            ],
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Edit'),
                                    visible: abp.auth.isGranted('Settings.AirOtherCharge.Edit'), //CHECK for the PERMISSION
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    visible: abp.auth.isGranted('Settings.AirOtherCharge.Delete'), //CHECK for the PERMISSION
                                    confirmMessage: function (data) {
                                        return l('AirOtherChargeDeletionConfirmationMessage', data.record.startNo, data.record.endNo);
                                    },
                                    action: function (data) {
                                        dolphin.freight.settings.airOtherCharge.airOtherCharge
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
    
            ]
        })
    );

    var createModal = new abp.ModalManager(abp.appPath + 'Settings/AirOtherCharge/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Settings/AirOtherCharge/EditModal');

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewAirOtherChargeButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});