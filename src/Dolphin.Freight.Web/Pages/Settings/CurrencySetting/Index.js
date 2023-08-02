$(function () {
    var l = abp.localization.getResource('Freight');
    var _changeInterval = null;
    var queryListFilter = function () {
        return {
            filter: $("input[name='Search'").val()
        };
    };

    var dataTable = $('#CurrencySetting').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            processing: true,
            responsive: {
                details: {
                    type: 'column'
                }
            },
            ajax: abp.libs.datatables.createAjax(dolphin.freight.settings.currencySetting.currencySetting.getList, queryListFilter),
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Edit'),
                                    visible: abp.auth.isGranted('Settings.CurrencySetting.Edit'), //CHECK for the PERMISSION
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    visible: abp.auth.isGranted('Settings.CurrencySetting.Delete'), //CHECK for the PERMISSION
                                    confirmMessage: function (data) {
                                        return l('CurrencySettingDeletionConfirmationMessage', data.record.startNo, data.record.endNo);
                                    },
                                    action: function (data) {
                                        dolphin.freight.settings.currencySetting.currencySetting
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
                    //起始號碼
                    title: l('CurrencyDepartment'),
                    data: "currencyDepartment"
                },
                {
                    //結束號碼
                    title: l('CustomerShortCode'),
                    data: "customerShortCode"
                },
                {
                    //當前分配的號碼
                    title: l('StartingCurrency'),
                    data: "startingCurrency"
                },
                {
                    //備註
                    title: l('EndCurrency'),
                    data: "endCurrency"
                },
                {
                    //備註
                    title: l('ExChangeRate'),
                    data: "exChangeRate"
                },
                {
                    //日期
                    title: l('EffectDate'), data: "effectDate",
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            }).toLocaleString(luxon.DateTime.DATETIME_SHORT);
                    }
                },
            ]
        })
    );

    $('#Search').keyup(function () {
        clearInterval(_changeInterval)
        _changeInterval = setInterval(function () {
            dataTable.ajax.reload();
            clearInterval(_changeInterval)
        }, 1000);
    });

    var createModal = new abp.ModalManager(abp.appPath + 'Settings/CurrencySetting/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Settings/CurrencySetting/EditModal');

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewCurrencySettingButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});