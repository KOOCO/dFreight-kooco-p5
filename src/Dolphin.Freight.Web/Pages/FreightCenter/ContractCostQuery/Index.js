﻿// #region 整理搜尋條件

var getFilter = function () {
    return {
        filterPortLoadingCode: $('#From_Loading_Port').val(), // $('#From_Loading_Port').dxSelectBox('instance').option('value'),
        filterPortDestinationCode: $('#To_Discharging_Port').val(),
        filterSortingMethod: document.querySelector("input[name=SortingMethod]:checked").value,
        filterCarrier: $('#SeaAirCompany').val(),
        filterSVCModeTerm: $('#TradeTerms').val(),
        filterCurrency: $('#CompareCurrencies').val(),
        filterLoad1: $('#Load1').val(),
        filterLoad2: $('#Load2').val(),
        filterLoad3: $('#Load3').val(),
        filterLoad4: $('#Load4').val(),
        filterLoad5: $('#Load5').val(),
        filterLoad6: $('#Load6').val(),
    };
};

// #endregion

// #region 頁面載入

$(function () {
    var l = abp.localization.getResource('Freight');

    // #region 表格 QUOTContractTable 設定

    // #region 自定義內建語言

    abp.libs.datatables.defaultConfigurations.language = function () {
        return {
            info: l("PagerInfo"),
            infoFiltered: l("PagerInfoFiltered"),
            infoEmpty: l("PagerInfoEmpty"),
            search: l("PagerSearch"),
            processing: l("ProcessingWithThreeDot"),
            loadingRecords: l("LoadingWithThreeDot"),
            lengthMenu: l("PagerShowMenuEntries"),
            emptyTable: l("NoDataAvailableInDatatable"),
            paginate: {
                first: l("PagerFirst"),
                last: l("PagerLast"),
                previous: l("PagerPrevious"),
                next: l("PagerNext")
            }
        };
    };

    // #endregion

    var dataTable = $('#QUOTContractTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            processing: true,
            serverSide: true,
            paging: false,
            order: false, // [[7, "desc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(dolphin.freight.iFreightDB.freightCenters.freightCenter.getQuotContract, getFilter),
            columnDefs: [
                {
                    title: '<input type="checkbox" class="form-check-input" id="tableselectall" /> <label for="tableselectall">全選</label>',
                    data: null,
                    orderable: false,
                    render: function (data, type, row, meta) {
                        return '<input type="checkbox" class="form-check-input" name="tablecheckbox" onchange=\'checkboxchange(this)\' />'; // index: meta.row
                    },
                    width: '10px'
                },
                {
                    title: null,
                    orderable: false,
                    data: "customField01",
                    render: function (data) {
                        return data;
                    }
                },
                {
                    title: null,
                    orderable: false,
                    data: "customField02",
                    render: function (data) {
                        return data;
                    }
                },
                {
                    title: null,
                    orderable: false,
                    data: "customField03",
                    render: function (data) {
                        return data;
                    },
                    width: '80px'
                },
                {
                    title: null,
                    orderable: false,
                    data: "customField04",
                    render: function (data) {
                        return data;
                    }
                },
                {
                    title: null,
                    orderable: false,
                    data: "customField05",
                    render: function (data) {
                        return data;
                    },
                    width: '50px'
                    // defaultContent: "無"
                },
                {
                    title: null,
                    orderable: false,
                    data: "customField06",
                    render: function (data) {
                        return data;
                    }
                },
                {
                    title: null,
                    orderable: false,
                    data: "customField07",
                    render: function (data) {
                        return data;
                    }
                },
            ]
        })
    );

    // #endregion

    // #region 按鈕 SearchButton.click

    $('#SearchButton').click(function () {
        dataTable.ajax.reload();

        console.log(getFilter());
        // console.log(newData);
 
    });

    // #endregion

    // #region 表格全選的事件設定

    var tableselectall = $('#tableselectall');
    tableselectall.change(function () {
        var input = document.getElementsByName('tablecheckbox');
        input.forEach(x => x.checked = this.checked);

    });

    var tablecheckbox = document.getElementsByName('tablecheckbox');
    var asssssss = $("input[name='tablecheckbox']");
    document.getElementsByName('tablecheckbox').forEach(x =>
        x.addEventListener('change', (event) => {
            if (event.currentTarget.checked) {
                alert('checked');
            } else {
                alert('not checked');
            }
        })
    );

    // #endregion

    // #region 給 Select2 用的樣式

    function formatRepo(repo) {
        if (repo.loading) {
            return repo.text;
        }

        var $container = $(
            //"<div class='select2-result-repository clearfix'>" +
            "<div style='display: inline;' style='color: Red; width: 50px;'>" +
            "<img style='width: 15px; vertical-align: middle;' src='https://maps.gstatic.com/mapfiles/transit/iw2/svg/tw-train.svg' />&nbsp;" +
            "<span class='ssss'></span>" +
            "</div>"
        );

        $container.find(".ssss").text(repo.text);
        $container.find(".ssss").append(" <span style='color: #B9B9B9;'>(" + repo.id + ")</span>");

        return $container;
    }

    function formatRepoSelection(repo) {
        var toShow = repo.display_text || repo.text || repo.id;
        return toShow;
    }

    function formatIdSelection(repo) {
        var toShow = repo.id || '_';
        return toShow;
    }

    // #endregion

    // #region Select2 的設定

    // #region 港口

    $(".SelectPort").select2({
        ajax: {
            url: '/api/app/lookup/select2Port-lookup',
            type: 'get',
            data: function (params) {
                var query = {
                    FilterText: params.term
                    // , PageIndex: params.page || 1
                }
                return query;
            },
            dataType: 'json',
            processResults: function (data, params) {
                // params.page = params.page || 1;

                /*
                // 自定義的打法
                var results = [];
                $.each(data, function (k, v) {
                    results.push({
                        id: v.id,
                        text: v.text,
                        image: v.image,
                        display_text: v.display_text
                    });
                });
                */

                return {
                    results: data, // 自定義回傳請回傳 results
                    // pagination:
                    // {
                    //     more: (params.page * 20) < data.length // 此處要透過 API 合作傳送分頁的資訊
                    // }
                };
            },
            cache: true,
            delay: 1500
        },
        language: l('GetCurrentLanguage') == 'en' ? 'en' : 'zh-TW',
        placeholder: l('PleaseChoose'),
        closeOnSelect: true,
        allowClear: true,
        minimumInputLength: 1,
        width: '100%',
        templateResult: formatRepo,
        templateSelection: formatRepoSelection,
    });

    // #endregion

    // #region 海空運公司

    $(".SelectCompany").select2({
        ajax: {
            url: '/api/app/lookup/select2Sea-air-company-lookup',
            type: 'get',
            data: function (params) {
                var query = { FilterText: params.term }
                return query;
            },
            dataType: 'json',
            processResults: function (data, params) {
                return { results: data };
            },
            cache: true,
            delay: 1500
        },
        language: l('GetCurrentLanguage') == 'en' ? 'en' : 'zh-TW',
        placeholder: l('PleaseChoose'),
        dropdownAutoWidth: true,
        closeOnSelect: true,
        allowClear: true,
        minimumInputLength: 1,
        width: '100%',
        templateResult: formatRepo,
        templateSelection: formatRepoSelection,
    });

    // #endregion

    // #region 幣別

    $(".SelectCurrency").select2({
        ajax: {
            url: '/api/app/lookup/select2Currency-lookup',
            type: 'get',
            data: function (params) {
                var query = { FilterText: params.term }
                return query;
            },
            dataType: 'json',
            processResults: function (data, params) {
                return { results: data };
            },
            cache: true,
            delay: 1500
        },
        language: l('GetCurrentLanguage') == 'en' ? 'en' : 'zh-TW',
        placeholder: l('PleaseChoose'),
        multiple: true,
        allowClear: true,
        closeOnSelect: false,
        dropdownAutoWidth: true,
        maximumSelectionLength: 3,
        width: '100%',
        templateResult: formatRepo,
        templateSelection: formatRepoSelection,
    });

    // #endregion

    // #region 貿易條件

    $(".SelectTradeTerms").select2({
        ajax: {
            url: '/api/app/lookup/select2SVCMode-term-lookup',
            type: 'get',
            data: function (params) {
                var query = { FilterText: params.term }
                return query;
            },
            dataType: 'json',
            processResults: function (data, params) {
                return { results: data };
            },
            cache: true,
            delay: 1500
        },
        language: l('GetCurrentLanguage') == 'en' ? 'en' : 'zh-TW',
        placeholder: l('PleaseChoose'),
        closeOnSelect: true,
        allowClear: true,
        minimumInputLength: 1,
        width: '100%',
        templateResult: formatRepo,
        templateSelection: formatRepoSelection,
    });

    // #endregion

    // #region 公司

    $(".SelectCmp").select2({
        ajax: {
            url: '/api/app/lookup/select2Bscode-lookup',
            type: 'get',
            data: function (params) {
                var query = {
                    FilterText: params.term,
                    CdType: 'CP'
                }
                return query;
            },
            dataType: 'json',
            processResults: function (data, params) {
                return { results: data };
            },
            cache: true,
            delay: 1500
        },
        language: l('GetCurrentLanguage') == 'en' ? 'en' : 'zh-TW',
        placeholder: l('PleaseChoose'),
        multiple: true,
        allowClear: true,
        closeOnSelect: false,
        dropdownAutoWidth: true,
        // minimumInputLength: 1,
        width: '100%',
        templateResult: formatRepo,
        templateSelection: formatIdSelection,
    });

    // #endregion

    // #endregion

});

// #endregion

// #region 選擇方塊選取變更的副程式

function checkboxchange(e) {
    var tablecheckbox = document.getElementsByName('tablecheckbox');
    var tt = 0;
    var ff = 0;
    tablecheckbox.forEach(x => { if (x.checked) tt++; else ff++; });

    if (tt > 0 & ff == 0) {
        document.getElementById('tableselectall').indeterminate = false;
        document.getElementById('tableselectall').checked = true;
    } else if (tt == 0 & ff > 0) {
        document.getElementById('tableselectall').indeterminate = false;
        document.getElementById('tableselectall').checked = false;
    } else {
        document.getElementById('tableselectall').indeterminate = true;
        document.getElementById('tableselectall').checked = false;
    }
}

// #endregion




