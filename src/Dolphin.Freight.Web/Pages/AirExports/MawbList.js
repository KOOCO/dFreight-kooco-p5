$(function () {
    var l = abp.localization.getResource('Freight');
    var _changeInterval = null;
    var queryListFilter = function () {
        return {
            search: $("input[name='Search'").val()
        };
    };
    var dataTable = $('#MawbListTable').DataTable(
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
            ajax: abp.libs.datatables.createAjax(dolphin.freight.importExport.airExports.airExportMawb.getList, queryListFilter),
            columnDefs: [
                {
                    title: '<input type="checkbox" id="selectAllCheckbox" onclick="selectAllCheckbox(this)" style=" cursor: pointer;">',
                    data: null,
                    orderable: false,
                    "render": function (data, type, row) {

                        var id = row.id;
                        var filingNo = row.filingNo;

                        return '<input type="checkbox" class="selectCheckbox" data-id="' + id + '" data-filingNo="' + filingNo + '" onclick="selectCheckbox(this)" style=" cursor: pointer;">';
                    }
                },
                {
                    title: '<div  style=" cursor: pointer;"><span><i class="fa fa-lock"></i></span></div>',
                    orderable: false,
                    "render": function (data, type, row) {
                        return '<input type="checkbox"  style=" cursor: pointer;">';
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
                                                        dolphin.freight.importExport.airExports.airExportMawb
                                                            .delete(data.record.id)
                                                            .then(function () {
                                                                abp.message.success(l('SuccessfullyDeleted')).then(function () {
                                                                    dataTable.ajax.reload();
                                                                });
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
                    title: '<div  style=" cursor: pointer;"><span><i class="fa fa-unlock"></i>Status</span></div>',
                    orderable: false,
                    render: function (data) {
                        if (!data) {
                            return '';
                        }
                    }
                },
                {
                    title: l('FileNo'),
                    data: "filingNo",
                    targets: 0,
                    render: function (data) {
                        if (!data) {
                            var td = '-';
                            return td;
                        }
                        return data;
                    }
                },
                {
                    title: l('MawbNo'),
                    data: "mawbNo",
                    render: function (data) {
                        if (!data) {
                            var td = '-';
                            return td;
                        }
                        return data;
                    }
                },
                {
                    title: l('Office'),
                    data: "officeName",
                    render: function (data) {
                        if (!data) {
                            var td = '-';
                            return td;
                        }
                        return data;
                    }
                },
                {
                    title: l('DepatureDate'),
                    data: "depatureDate",
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            }).toFormat('yyyy-MM-dd HH:mm');
                    }
                },
                {
                    title: l('ArrivalDate'),
                    data: "arrivalDate",
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            }).toFormat('yyyy-MM-dd HH:mm');
                    }
                },
                {
                    title: l('AES'),
                    data: "",
                    render: function (data) {
                        if (!data) {
                            return '-';
                        } else {
                            return data;
                        }
                    }
                },
                {
                    title: l('Depature'),
                    data: "depatureAirportName",
                    render: function (data) {
                        if (!data) {
                            var td = '-';
                            return td;
                        }
                        return data;
                    }
                },
                {
                    title: l('Destination'),
                    data: "destinationAirportName",
                    render: function (data) {
                        if (!data) {
                            var td = '-';
                            return td;
                        }
                        return data;
                    }
                },
                {
                    title: l('B/L Date'),
                    data: "awbDate",
                    render: function (data, type, row) {
                        if (data) {
                            var date = new Date(data);
                            return date.getFullYear() + '-' + (date.getMonth() + 1).toString().padStart(2, '0') + '-' + date.getDate().toString().padStart(2, '0');
                        } else {
                            var td = "-";
                            return td;
                        }
                    }
                },
                {
                    title: l('Shipper'),
                    data: "shipper",
                    render: function (data) {
                        if (!data) {
                            var td = '-';
                            return td;
                        }
                        return data;
                    }
                },
                {
                    title: l('Consignee'),
                    data: "consigneeName",
                    render: function (data) {
                        if (!data) {
                            var td = '-';
                            return td;
                        }
                        return data;
                    }
                },
                {
                    title: l('Carrier'),
                    data: "carrierTPName",
                    render: function (data) {
                        if (!data) {
                            var td = '-';
                            return td;
                        }
                        return data;
                    }
                },
                {
                    title: l('FlightNo'),
                    data: "flightNo",
                    render: function (data) {
                        if (!data) {
                            var td = '-';
                            return td;
                        }
                        return data;
                    }
                },
                {
                    title: l('HawbNo'),
                    data: "hawbNos",
                    render: function (data) {
                        if (!data) {
                            var td = '-';
                            return td;
                        }
                        return data;
                    }
                },
                {
                    title: l('BillTo'),
                    data: "awbAccountCarrierName",
                    render: function (data) {
                        if (!data) {
                            var td = '-';
                            return td;
                        }
                        return data;
                    }
                },
                {
                    title: l('AR Balance'),
                    data: "arTotal",
                    render: function (data) {
                        if (data) {
                            var parseData = parseFloat(data).toFixed(2);
                            return parseData;
                        } else {
                            var td = '-';
                            return td;
                        }
                    }
                },
                {
                    title: l('A/P Balance'),
                    data: "apTotal",
                    render: function (data) {
                        if (data) {
                            var parseData = parseFloat(data).toFixed(2);
                            return parseData;
                        } else {
                            var td = '-';
                            return td;
                        }
                    }
                },
                {
                    title: l('D/C Balance'),
                    data: "dcTotal",
                    render: function (data) {
                        if (data) {
                            var parseData = parseFloat(data).toFixed(2);
                            return parseData;
                        } else {
                            var td = '-';
                            return td;
                        }
                    }
                },
                {
                    title: l('GrossWeight'),
                    data: null,
                    render: function (data, row) {
                        return `<span class="kg">KG : ${data.grossWeightKg}</span><span class="lb">LB : ${data.grossWeightLb}</span>`;
                    }
                },
                {
                    title: l('ChargableWeight'),
                    data: null,
                    render: function (data, row) {
                        return `<span class="kg">KG : ${data.chargeableWeightKg}</span><span class="lb">LB : ${data.chargeableWeightLb}</span>`;
                    }
                },
                {
                    title: l('PostDate'),
                    data: "postDate",
                    render: function (data, type, row) {
                        if (data) {
                            var date = new Date(data);
                            return date.getFullYear() + '-' + (date.getMonth() + 1).toString().padStart(2, '0') + '-' + date.getDate().toString().padStart(2, '0');
                        } else {
                            var td = '-';
                            return td;
                        }
                    }
                },
                {
                    title: l('Sales'),
                    data: "sales",
                    render: function (data) {
                        if (!data) {
                            var td = '-';
                            return td;
                        }
                        return data;
                    }
                },
                {
                    title: l('OP'),
                    data: "oP",
                    render: function (data) {
                        if (!data) {
                            var td = '-';
                            return td;
                        }
                        return data;
                    }
                }
            ],
            drawCallback: function (settings) {
                $('.sorting_asc').removeClass('sorting_asc');
            }
        })
    );

   
    $('#Search').keyup(function () {
        clearInterval(_changeInterval)
        _changeInterval = setInterval(function () {
            dataTable.ajax.reload();
            clearInterval(_changeInterval)
        }, 1000);
    });
    

    $('#AddMawbButton').click(function (e) {
        window.location = "\CreateMawb";
    });

});