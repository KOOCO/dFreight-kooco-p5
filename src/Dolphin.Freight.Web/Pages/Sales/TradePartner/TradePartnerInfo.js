$(function () {
    
    var l = abp.localization.getResource('Freight');
    var tradePartnerId = $('#tpId').val();
    console.log("tradePartnerId:" + tradePartnerId);
    var dataTable;

    function showContactPersonDataTable(inputTpId) {
        console.log("showContactPersonDataTable with inputTpId:" + inputTpId);

        var inputAction = function () {
            return {
                tradePartnerId: inputTpId
            };
        };

        dataTable = $('#ContactPersonTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
                order: [[1, "asc"]],
                searching: false,
                scrollX: true,
                scrollCollapse: true,
                destroy: true,
                ajax: abp.libs.datatables.createAjax(dolphin.freight.tradePartners.contactPerson.getList, inputAction), //pass the filter to the GetList method

                columnDefs: [
                    {
                        title: l('Actions'),
                        rowAction: {
                            items:
                                [
                                    {
                                        text: l('Edit'),
                                        //visible: abp.auth.isGranted('BookStore.Books.Edit'),
                                        action: function (data) {
                                            editModal.open({
                                                id: data.record.id
                                            });
                                        }
                                    },
                                    {
                                        text: l('Delete'),
                                        // visible: abp.auth.isGranted('BookStore.Authors.Delete'),
                                        confirmMessage: function (data) {
                                            return l(
                                                'AreYouSureToDelete',
                                                data.record.contactName
                                            );
                                        },
                                        action: function (data) {
                                            dolphin.freight.tradePartners.contactPerson
                                                .deleteContactPerson(data.record.id)
                                                .then(function () {
                                                    abp.notify.info(
                                                        l('SuccessfullyDeleted')
                                                    );
                                                    dataTable.ajax.reload();
                                                });
                                        }
                                    }
                                ]
                        }
                    },
                    {
                        title: l('Display:ContactPerson:THead:isRep'),
                        data: "isRep",
                        render: function (data) {
                            if (data == true) {
                                return "v";
                            } else {
                                return "";
                            }
                        }
                    },
                    {
                        title: l('Display:ContactPerson:THead:isEmailNotification'),
                        data: "isEmailNotification",
                        render: function (data) {
                            if (data == true) {
                                return "v";
                            } else {
                                return "";
                            }
                        }
                    },
                    {
                        title: l('Display:ContactPerson:THead:ContactName'),
                        data: "contactName"
                    },
                    {
                        title: l('Display:ContactPerson:THead:ContactTitle'),
                        data: "contactTitle"
                    },
                    {
                        title: l('Display:ContactPerson:THead:ContactDivision'),
                        data: "contactDivision"
                    },
                    {
                        title: l('Display:ContactPerson:THead:ContactCellPhone'),
                        data: "contactCellPhone"
                    },
                    {
                        title: l('Display:ContactPerson:THead:ContactPhone'),
                        data: "contactPhone"
                    },
                    {
                        title: l('Display:ContactPerson:THead:ContactFax'),
                        data: "contactFax"
                    },
                    {
                        title: l('Display:ContactPerson:THead:ContactEmailAddress'),
                        data: "contactEmailAddress"
                    },
                    {
                        title: l('Display:ContactPerson:THead:ContactRemark'),
                        data: "contactRemark"
                    }
                ]
            })
        );

    }

    showContactPersonDataTable(tradePartnerId);

    /* ==================== Memo Begin ==================== */
    let memoDataTable;

    const createMemoModal = new abp.ModalManager({
        viewUrl: '/Sales/TradePartner/ModalWithCreateTradePartyMemo'
    });

    createMemoModal.onResult(function () {
        memoDataTable.ajax.reload();
    });

    function showMemoDataTable() {
        memoDataTable = $('#memo-table').DataTable({
            order: [[3, 'desc']],
            paging: false,
            info: false,
            searching: false,
            ajax: function (d, cb) {
                dolphin.freight.tradePartners.tradePartnerMemo.getListByTradePartnerId(tradePartnerId).then(data => cb({ data }));
            },
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items: [
                            {
                                text: l('Edit'),
                                action: function (data) {
                                    createMemoModal.open({
                                        id: data.record.id,
                                        tradePartnerId,
                                    });
                                }
                            },
                            {
                                text: l('Delete'),
                                confirmMessage: function (data) {
                                    return l(
                                        'AreYouSureToDelete',
                                        data.record.title
                                    );
                                },
                                action: function (data) {
                                    dolphin.freight.tradePartners.tradePartnerMemo.delete(data.record.id)
                                        .then(function () {
                                            abp.notify.info(l('SuccessfullyDeleted'));
                                            memoDataTable.ajax.reload();
                                        });
                                }
                            },
                        ]
                    },
                    targets: 0,
                    render: function (data, type, row) {
                        return "";
                    },
                    orderable: false,
                },
                {
                    title: '<i class="fa fa-bell"></i>',
                    render: function (data, type, row) {
                        return `<input type="checkbox" class="memo-highlight-checkbox" data-id="${row.id}" ${row.highlight === true ? 'checked' : ''} />`;
                    },
                    className: "text-center",
                    targets: 1,
                    orderable: false,
                },
                {
                    title: l('Display:Memo:THead:Title'),
                    render: function (data, type, row) {
                        return `${row.highlight === true ? '<i class="fa fa-bell memo-highlight"></i>' : ''}${row.title}`;
                    },
                    targets: 2,
                    orderable: false,
                },
                {
                    title: l('Display:Memo:THead:LastModified'),
                    render: function (data, type, row) {
                        if (row.lastModificationTime == null) {
                            return "";
                        }
                        const time = row.lastModificationTime.replaceAll('T', ' ').split('.')[0];
                        return `${row.lastUpdatedUserName}(${time})`;
                    },
                    targets: 3,
                },
                {
                    title: l('Display:Memo:THead:CreateTime'),
                    render: function (data, type, row) {
                        const time = row.creationTime.replaceAll('T', ' ').split('.')[0];
                        return `${row.createdUserName}(${time})`;
                    },
                    targets: 4,
                },
            ],
        });
    }

    $(document).on('click', '.memo-highlight-checkbox', function (e) {
        const self = $(this);

        dolphin.freight.tradePartners.tradePartnerMemo.switchHighlight({
            Id: self.data('id'),
            Highlight: self.prop('checked'),
        }).always(result => {
            memoDataTable.ajax.reload();
        });

        return false;
    });

    $(document).on('click', '#memo-table tbody tr', function () {
        $('.memo-focus').removeClass('memo-focus');
        $(this).addClass('memo-focus');
        const data = memoDataTable.row(this).data();
        $('#memo-content').html(data.memo);
    });

    $('#add-memo').click(function () {
        if (tradePartnerId !== '00000000-0000-0000-0000-000000000000') {
            createMemoModal.open({
                tradePartnerId: tradePartnerId,
            });
            return;
        }

        const tpNameInput = $('#tpNameInput').val();
        if ((tpNameInput == null) || (tpNameInput === '')) {
            abp.message.warn(l('Display:Memo:WarnMsg'));
            return;
        }

        dolphin.freight.tradePartners.tradePartner.findByTpName(tpNameInput).then((result) => {
            if (result == null) {
                abp.message.warn(l('Display:Memo:WarnMsg'));
                return;
            } 

            tradePartnerId = result.id;
            createMemoModal.open({
                tradePartnerId: tradePartnerId,
            });
        });
    });

    showMemoDataTable();
    /* ==================== Memo End ==================== */

    /* ==================== TradeParty Begin ==================== */
    const tradePartyDataTable = [];
    const createTradePartyModal = [];

    function reloadAllTradePartyDataTable() {
        tradePartyDataTable.forEach(row => row.ajax.reload());
    }

    $('.trade-party-table').each((index, value) => {
        const dom = $(value);
        const partyType = dom.data('type');

        createTradePartyModal[partyType] = new abp.ModalManager({
            viewUrl: '/Sales/TradePartner/ModalWithCreateTradeParty',
        });

        createTradePartyModal[partyType].onResult(function () {
            reloadAllTradePartyDataTable();
        });

        tradePartyDataTable[partyType] = dom.DataTable({
            paging: false,
            info: false,
            searching: false,
            ajax: function (d, cb) {
                dolphin.freight.tradePartners.tradeParties.tradeParty.getListByPartnerIdAndType(tradePartnerId, partyType).then(data => cb({ data }));
            },
            rowsGroup: [0, 1, 2, 3, 4, 10],
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items: [
                            {
                                text: l('Edit'),
                                action: function (data) {
                                    createTradePartyModal[partyType].open({
                                        id: data.record.id,
                                        tradePartnerId,
                                        tradePartyType: partyType,
                                    });
                                }
                            },
                            {
                                text: l('Delete'),
                                confirmMessage: function (data) {
                                    return l(
                                        'AreYouSureToDelete',
                                        data.record.title
                                    );
                                },
                                action: function (data) {
                                    dolphin.freight.tradePartners.tradeParties.tradeParty.delete(data.record.id)
                                        .then(function () {
                                            abp.notify.info(l('SuccessfullyDeleted'));
                                            reloadAllTradePartyDataTable();
                                        });
                                }
                            },
                        ]
                    },
                    data: 'id',
                    targets: 0,
                    orderable: false,
                },
                {
                    title: l('Display:TradeParty:THead:Default'),
                    data: 'id',
                    render: function (data, type, row) {
                        return `<input type="checkbox" class="trade-party-default-checkbox" data-id="${row.id}" data-party-type="${partyType}" ${row.isDefault === true ? 'checked' : ''} />`;
                    },
                    className: 'text-center',
                    targets: 1,
                    orderable: false,
                },
                {
                    title: l('Display:TradeParty:THead:Description'),
                    data: 'tradePartyDescription',
                    targets: 2,
                    orderable: false,
                },
                {
                    title: l('Display:TradeParty:THead:CompanyName'),
                    data: 'companyName',
                    targets: 3,
                },
                {
                    title: l('Display:TradeParty:THead:Address'),
                    data: 'address',
                    targets: 4,
                },
                {
                    title: l('Display:TradeParty:THead:Rep'),
                    render: function (data, type, row) {
                        if (row.contact === '' || row.contact === null) {
                            return '';
                        }
                        return `<input type="checkbox" class="contact-person-default-checkbox" data-id="${row.contactPersonId}" data-party-type="${partyType}" ${row.isRep === true ? 'checked' : ''} />`;
                    },
                    className: 'text-center',
                    targets: 5,
                },
                {
                    title: l('Display:TradeParty:THead:ContactPerson'),
                    data: 'contact',
                    targets: 6,
                },
                {
                    title: l('Display:TradeParty:THead:Tel'),
                    data: 'tel',
                    targets: 7,
                },
                {
                    title: l('Display:TradeParty:THead:Fax'),
                    data: 'fax',
                    targets: 8,
                },
                {
                    title: l('Display:TradeParty:THead:Email'),
                    data: 'email',
                    targets: 9,
                },
                {
                    title: '',
                    data: 'id',
                    render: function (data, type, row) {
                        return `<a target="_blank" href="/Sales/TradePartner/EditTradePartnerInfo/${row.targetTradePartnerId}"><i class="fa-sharp fa-solid fa-share-from-square"></i></a>`;
                    },
                    targets: 10,
                },
            ],
        });

        //var editor = new $.fn.dataTable.altEditor({
        //    table: '#' + tradePartyDataTable[partyType].table().container().id,
        //    fields: [
        //        { label: 'Name:', name: 'name' },
        //        { label: 'Age:', name: 'age' }
        //    ]
        //});

        //new $.fn.dataTable.Buttons(tradePartyDataTable[partyType], {
        //    buttons: [
        //        { extend: 'edit', editor: editor },
        //        { extend: 'remove', editor: editor }
        //    ]
        //});
        //tradePartyDataTable[partyType].buttons(1, null).container().prependTo(tradePartyDataTable[partyType].table());

    });

    $(document).on('click', '.trade-party-default-checkbox', function (e) {
        const self = $(this);

        dolphin.freight.tradePartners.tradeParties.tradeParty.switchDefault({
            Id: self.data('id'),
            IsDefault: self.prop('checked'),
        }).always(result => {
            reloadAllTradePartyDataTable();
        });

        return false;
    });

    $(document).on('click', '.contact-person-default-checkbox', function (e) {
        const self = $(this);

        dolphin.freight.tradePartners.contactPerson.switchRep({
            Id: self.data('id'),
            IsRep: self.prop('checked'),
        }).always(result => {
            reloadAllTradePartyDataTable();
        });

        return false;
    });

    //$(document).on('click', '.add-trade-party', function (e) {
    //    const self = $(this);
    //    const tradePartyType = self.data('party-type');

    //    var table = $('table[data-type="' + tradePartyType + '"]').DataTable();
    //    var testRow = { id: '00000000-0000-0000-0000-000000000000', 'tradePartyDescription': '', 'companyName': '', 'address': '', 'row.contactPersonId': '', 'contact': '', 'tel': '', 'fax': '', 'email': '', isHideAction: true };
    //    table.row.add(testRow);
    //    table.draw();

    //    if (tradePartnerId !== '00000000-0000-0000-0000-000000000000') {
    //        createTradePartyModal[tradePartyType].open({
    //            tradePartnerId,
    //            tradePartyType,
    //        });
    //        return;
    //    }

    //    const tpNameInput = $('#tpNameInput').val();
    //    if ((tpNameInput == null) || (tpNameInput === '')) {
    //        abp.message.warn(l('Display:TradeParty:WarnMsg'));
    //        return;
    //    }

    //    dolphin.freight.tradePartners.tradePartner.findByTpName(tpNameInput).then((result) => {
    //        if (result == null) {
    //            abp.message.warn(l('Display:TradeParty:WarnMsg'));
    //            return;
    //        }

    //        tradePartnerId = result.id;

    //        createTradePartyModal[tradePartyType].open({
    //            tradePartnerId,
    //            tradePartyType,
    //        });
    //    });
    //});
    /* ==================== TradeParty End ==================== */

    $('#TPForm').on('abp-ajax-success', function () {
        event.preventDefault();
        var tpName = $('#tpNameInput').val();
        $('#nav-accounting-tab').removeClass("disabled");
        $('#nav-doc-tab').removeClass("disabled");
        $('#nav-status-tab').removeClass("disabled");
        abp.message.success('Successfully saved.').then(function () {
            dolphin.freight.tradePartners.tradePartner.findByTpName(tpName).then(function (res) {
                var tpId = res.id;
                window.location.pathname = '/Sales/TradePartner/EditTradePartnerInfo/' + tpId;
            });
        });
    });

    var createModal = new abp.ModalManager({
        viewUrl: '/Sales/TradePartner/ModalWithCreateContactPerson'
    });

   /* var newTPId;*/

    $('#AddContactPersonButton').click(function (e) {
        var tpNameInput = $('#tpNameInput').val();
        console.log("tpNameInput:" + tpNameInput);
        if ((tpNameInput == null) || (tpNameInput == "")) {
            abp.message.warn(l('Display:ContactPerson:WarnMsg'));
        } else {
            dolphin.freight.tradePartners.tradePartner.findByTpName(tpNameInput)
                .then(function (result) {
                    if (result == null) {
                        abp.message.warn(l('Display:ContactPerson:WarnMsg'));
                    } else {
                        console.log("result:" + result.id);
                        newTPId = result.id;
                        console.log("newTPId:" + newTPId);
                        e.preventDefault();
                        createModal.open({
                            tradePartnerId: result.id
                        });
                    }
                    
                });
        }
    });

    createModal.onResult(function () {
        console.log('open the ModalWithCreateCreditLimitGroup');
        console.log("create modal newTPId:" + newTPId);
        showContactPersonDataTable(newTPId);
    });



    var editModal = new abp.ModalManager({
        viewUrl: '/Sales/TradePartner/ModalWithEditContactPerson'
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#AddTradePartyButton').click(function (e) {
        var tpNameInput = $('#tpNameInput').val();
        console.log("tpNameInput:" + tpNameInput);
        if ((tpNameInput == null) || (tpNameInput == "")) {
            abp.message.warn(l('Display:ContactPerson:WarnMsg'));
        } else {
            dolphin.freight.tradePartners.tradePartner.findByTpName(tpNameInput)
                .then(function (result) {
                    if (result == null) {
                        abp.message.warn(l('Display:ContactPerson:WarnMsg'));
                    } else {
                        console.log("result:" + result.id);
                        newTPId = result.id;
                        console.log("newTPId:" + newTPId);
                        e.preventDefault();
                        createTradePartyModal.open({
                            tradePartnerId: result.id
                        });
                    }

                });
        }
    });

});