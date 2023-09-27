/*!
    * Start Bootstrap - SB Admin v7.0.5 (https://startbootstrap.com/template/sb-admin)
    * Copyright 2013-2022 Start Bootstrap
    * Licensed under MIT (https://github.com/StartBootstrap/startbootstrap-sb-admin/blob/master/LICENSE)
    */
    // 
// Scripts
// 

window.addEventListener('DOMContentLoaded', event => {

    // Toggle the side navigation
    const sidebarToggle = document.body.querySelector('#sidebarToggle');
    if (sidebarToggle) {
        // Uncomment Below to persist sidebar toggle between refreshes
        // if (localStorage.getItem('sb|sidebar-toggle') === 'true') {
        //     document.body.classList.toggle('sb-sidenav-toggled');
        // }
        sidebarToggle.addEventListener('click', event => {
            event.preventDefault();
            document.body.classList.toggle('sb-sidenav-toggled');
            localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));
        });
    }

});

$(document).ready(function () {
    initMenuActive();
    if (location.pathname != null && location.pathname != "") {
        $('.nav-link[href="' + location.pathname + '"]').parents('div.collapse').collapse('show');
    }

    $(".checkdate").keyup(function () {
        var checkValue = $(this).val();
        var checkValues = checkValue.split("");
        var rsValue = "";
        for (var i = 0; i < checkValues.length; i++) {
            if (i == 0) {
                rsValue = rsValue + "2";
            }
            if (i == 1) {
                rsValue = rsValue + "0";
            }
            if (i == 2) {
                if ("23456789".indexOf(checkValues[2]) > -1) rsValue = rsValue + checkValues[2];
                else {
                    rsValue = rsValue + "";
                    break;
                }
            }
            if (i == 3) {
                if ("0123456789".indexOf(checkValues[3]) > -1) rsValue = rsValue + checkValues[3];
                else {
                    rsValue = rsValue + "";
                    break;
                }
            }
            if (i == 4 || i == 7) {
                rsValue = rsValue + "-";
            }
            if (i == 5) {
                if ("01".indexOf(checkValues[i]) > -1) rsValue = rsValue + checkValues[i];
                else {
                    rsValue = rsValue + "";
                    break;
                }
            }
            if (i == 6 || i == 9) {
                if ("0123456789".indexOf(checkValues[i]) > -1) rsValue = rsValue + checkValues[i];
                else {
                    rsValue = rsValue + "";
                    break;
                }
            }
            if (i == 8) {
                if ("012".indexOf(checkValues[i]) > -1) rsValue = rsValue + checkValues[i];
                else {
                    rsValue = rsValue + "";
                    break;
                }
            }
        }
        $(this).val(rsValue);
    });
    $(".cdatetime").keyup(function () {
        var checkValue = $(this).val();
        var checkValues = checkValue.split("");
        var rsValue = "";
        for (var i = 0; i < checkValues.length; i++) {
            if (i == 0) {
                rsValue = rsValue + "2";
            }
            if (i == 1) {
                rsValue = rsValue + "0";
            }
            if (i == 2) {
                if ("23456789".indexOf(checkValues[2]) > -1) rsValue = rsValue + checkValues[2];
                else {
                    rsValue = rsValue + "";
                    break;
                }
            }
            if (i == 3) {
                if ("0123456789".indexOf(checkValues[3]) > -1) rsValue = rsValue + checkValues[3];
                else {
                    rsValue = rsValue + "";
                    break;
                }
            }
            if (i == 4 || i == 7) {
                rsValue = rsValue + "-";
            }
            if (i == 5) {
                if ("01".indexOf(checkValues[i]) > -1) rsValue = rsValue + checkValues[i];
                else {
                    rsValue = rsValue + "";
                    break;
                }
            }
            if (i == 6 || i == 15) {
                if ("0123456789".indexOf(checkValues[i]) > -1) rsValue = rsValue + checkValues[i];
                else {
                    rsValue = rsValue + "";
                    break;
                }
            }
            if (i == 8) {
                if ("013".indexOf(checkValues[i]) > -1) rsValue = rsValue + checkValues[i];
                else {
                    rsValue = rsValue + "";
                    break;
                }
            }
            if (i == 9) {
                if (checkValues[8] == "3") {
                    if ("013".indexOf(checkValues[i]) > -1) rsValue = rsValue + checkValues[i];
                    else {
                        rsValue = rsValue + "";
                        break;
                    }
                } else {
                    if ("0123456789".indexOf(checkValues[i]) > -1) rsValue = rsValue + checkValues[i];
                    else {
                        rsValue = rsValue + "";
                        break;
                    }
                }
            }
            if (i == 10) rsValue = rsValue + " ";
            if (i == 11) {
                if ("012".indexOf(checkValues[i]) > -1) rsValue = rsValue + checkValues[i];
                else {
                    rsValue = rsValue + "";
                    break;
                }
            }
            if (i == 12) {
                if (checkValues[11] == "2") {
                    if ("01234".indexOf(checkValues[i]) > -1) rsValue = rsValue + checkValues[i];
                    else {
                        rsValue = rsValue + "";
                        break;
                    }
                } else {
                    if ("0123456789".indexOf(checkValues[i]) > -1) rsValue = rsValue + checkValues[i];
                    else {
                        rsValue = rsValue + "";
                        break;
                    }
                }

            }
            if (i == 13) rsValue = rsValue + ":";
            if (i == 14) {
                if ("012345".indexOf(checkValues[i]) > -1) rsValue = rsValue + checkValues[i];
                else {
                    rsValue = rsValue + "";
                    break;
                }
            }

        }
        $(this).val(rsValue);
    });

});
function initMenuActive() {
    var url = window.location.href
    $(".nav-item").removeClass("active");
    //$(".menu-item").removeClass("active");
    $(".menuul").removeClass("show");
    $(".menuli").removeClass("show");
    var identityUser = document.getElementById('MenuItem_AbpIdentity.Users');
    identityUser && identityUser.classList.add('d-none');
    if (url.indexOf("TenantManagement/Tenants") > -1) {
        $("#collapseAbp_Application_Main_Administration").addClass(" show ")
        $("#n_Abp_Application_Main_Administration").addClass(" active ")
        $("#collapseTenantManagement").addClass(" show ")
        $("#p_TenantManagement_Tenants").addClass(" active ")
    }
    if (url.indexOf("Identity/Roles") > -1) {
        $("#collapseAbp_Application_Main_Administration").addClass(" show ")
        $("#n_Abp_Application_Main_Administration").addClass(" active ")
        $("#collapseAbpIdentity").addClass(" show ")
        $("#p_AbpIdentity_Roles").addClass(" active ")
    }
    if (url.indexOf("Identity/Users") > -1) {
        $("#collapseAbp_Application_Main_Administration").addClass(" show ")
        $("#n_Abp_Application_Main_Administration").addClass(" active ")
        $("#collapseAbpIdentity").addClass(" show ")
        $("#p_AbpIdentity_Users").addClass(" active ")
    }
    if (url.indexOf("SettingManagement") > -1) {
        $("#collapseAbp_Application_Main_Administration").addClass(" show ")
        $("#n_Abp_Application_Main_Administration").addClass(" active ")
        $("#p_SettingManagement").addClass(" active ")
    }
    if (url.indexOf("OceanExports/CreateMbl") > -1) {
        $("#n_OceanExports").addClass(" active ")
        $("#collapseOceanExports").addClass(" show ")
        $("#p_OceanExports_CreateMbl").addClass(" active ")
    }
    if (url.indexOf("OceanExports/Index") > -1) {
        $("#n_OceanExports").addClass(" active ")
        $("#collapseOceanExports").addClass(" show ")
        $("#p_OceanExports_IndexList").addClass(" active ")
    }
    if (url.indexOf("OceanExports/MblList") > -1) {
        $("#n_OceanExports").addClass(" active ")
        $("#collapseOceanExports").addClass(" show ")
        $("#p_OceanExports_MblList").addClass(" active ")
    }
    if (url.indexOf("OceanExports/HblList") > -1) {
        $("#n_OceanExports").addClass(" active ")
        $("#collapseOceanExports").addClass(" show ")
        $("#p_OceanExports_HblList").addClass(" active ")
    }
    if (url.indexOf("OceanExports/ExportBookings/Create") > -1) {
        $("#n_OceanExports").addClass(" active ")
        $("#collapseOceanExports").addClass(" show ")
        $("#p_ExportBookings_Create").addClass(" active ")
    }
    if (url.indexOf("OceanExports/ExportBookings/Index") > -1) {
        $("#n_OceanExports").addClass(" active ")
        $("#collapseOceanExports").addClass(" show ")
        $("#p_ExportBookings_Index").addClass(" active ")
    }
    if (url.indexOf("OceanExports/VesselSchedules/Create") > -1) {
        $("#n_OceanExports").addClass(" active ")
        $("#collapseOceanExports").addClass(" show ")
        $("#p_VesselSchedules_Create").addClass(" active ")
    }
    if (url.indexOf("OceanExports/VesselSchedules/Index") > -1) {
        $("#n_OceanExports").addClass(" active ")
        $("#collapseOceanExports").addClass(" show ")
        $("#p_VesselSchedules_Index").addClass(" active ")
    }
    if (url.indexOf("OceanImports/CreateMbl") > -1) {
        $("#n_OceanImports").addClass(" active ")
        $("#collapseOceanImports").addClass(" show ")
        $("#p_OceanImports_CreateMbl").addClass(" active ")
    }
    if (url.indexOf("OceanImports/Index") > -1) {
        $("#n_OceanImports").addClass(" active ")
        $("#collapseOceanImports").addClass(" show ")
        $("#p_OceanImports_IndexList").addClass(" active ")
    }
    if (url.indexOf("OceanImports/MblList") > -1) {
        $("#n_OceanImports").addClass(" active ")
        $("#collapseOceanImports").addClass(" show ")
        $("#p_OceanImports_MblList").addClass(" active ")
    }
    if (url.indexOf("OceanImports/HblList") > -1) {
        $("#n_OceanImports").addClass(" active ")
        $("#collapseOceanImports").addClass(" show ")
        $("#p_OceanImports_HblList").addClass(" active ")
    }
    if (url.indexOf("Settings/ItNoRanges") > -1) {
        $("#n_Settings").addClass(" active ")
        $("#collapseSettings").addClass(" show ")
        $("#p_Settings_ItNoRanges").addClass(" active ")
    }
    if (url.indexOf("Settings/AwbNoRanges") > -1) {
        $("#n_Settings").addClass(" active ")
        $("#collapseSettings").addClass(" show ")
        $("#p_Settings_AwbNoRanges").addClass(" active ")
    }
    if (url.indexOf("Settings/PackageUnits") > -1) {
        $("#n_Settings").addClass(" active ")
        $("#collapseSettings").addClass(" show ")
        $("#p_Settings_PackageUnits").addClass(" active ")
    }
    if (url.indexOf("Settings/ContainerSizes") > -1) {
        $("#n_Settings").addClass(" active ")
        $("#collapseSettings").addClass(" show ")
        $("#p_Settings_ContainerSizes").addClass(" active ")
    }
    if (url.indexOf("AccountingSettings/GlCodes") > -1) {
        $("#n_AccountingSettings").addClass(" active ")
        $("#collapseAccountingSettings").addClass(" show ")
        $("#p_AccountingSettings_GlCodes").addClass(" active ")
    }
    if (url.indexOf("AccountingSettings/BillingCodes") > -1) {
        $("#n_AccountingSettings").addClass(" active ")
        $("#collapseAccountingSettings").addClass(" show ")
        $("#p_AccountingSetting_BillingCodes").addClass(" active ")
    }
    if (url.indexOf("Sales/TradePartner/TradePartnerInfo") > -1) {
        $("#n_Dolphin_Freight_TradePartnerManagement").addClass(" active ")
        $("#collapseDolphin_Freight_TradePartnerManagement").addClass(" show ")
        $("#p_Dolphin_Freight_TradePartnerManagement_Addition").addClass(" active ")
    }
    if (url.indexOf("Sales/TradePartner/Credit") > -1) {
        $("#n_Dolphin_Freight_TradePartnerManagement").addClass(" active ")
        $("#collapseDolphin_Freight_TradePartnerManagement").addClass(" show ")
        $("#p_Dolphin_Freight_TradePartnerManagement_CreditEntry").addClass(" active ")
    }
    if (url.indexOf("Sales/TradePartner/List") > -1) {
        $("#n_Dolphin_Freight_TradePartnerManagement").addClass(" active ")
        $("#collapseDolphin_Freight_TradePartnerManagement").addClass(" show ")
        $("#p_Dolphin_Freight_TradePartnerManagement_List").addClass(" active ")
    }
    if (url.indexOf("Settings/Users/Management") > -1) {
        $("#n_Settings").addClass(" active ")
        $("#collapseSettings").addClass(" show ")
        $("#p_Settings_Users").addClass(" active ")
    }
    if (url.indexOf("AirExports/CreateMawb") > -1) {
        $("#n_Dolphin_Freight_AirExportManagement").addClass(" active ")
        $("#collapseDolphin_Freight_AirExportManagement").addClass(" show ")
        $("#p_Dolphin_Freight_AirExportManagement_Shipment").addClass(" active ")
    }
    if (url.indexOf('AirExports/Index') > -1) {
        $("#n_Dolphin_Freight_AirExportManagement").addClass(" active ")
        $("#collapseDolphin_Freight_AirExportManagement").addClass(" show ")
        $("#p_Dolphin_Freight_AirExportManagement_ShipmentList").addClass(" active ")
    }
    if (url.indexOf('AirImports/CreateMawb') > -1) {
        $("#n_Dolphin_Freight_AirImportManagement").addClass(" active ")
        $("#collapseDolphin_Freight_AirImportManagement").addClass(" show ")
        $("#p_Dolphin_Freight_AirImportManagement_Shipment").addClass(" active ")
    }
    if (url.indexOf('AirImports/Index') > -1) {
        $("#n_Dolphin_Freight_AirImportManagement").addClass(" active ")
        $("#collapseDolphin_Freight_AirImportManagement").addClass(" show ")
        $("#p_Dolphin_Freight_AirImportManagement_ShipmentList").addClass(" active ")
    }
}
function removeDuplicatesFromLinkedList(linkedList) {
    var ptr1 = null, ptr2 = null, dup = null;
    ptr1 = linkedList;

    /* Pick elements one by one */
    while (ptr1 != null && ptr1.next != null) {
        ptr2 = ptr1;

        /*
         * Compare the picked element with rest of the elements
         */
        while (ptr2.next != null) {

            /* If duplicate then delete it */
            if (ptr1.data == ptr2.next.data) {

                /* sequence of steps is important here */
                dup = ptr2.next;
                ptr2.next = ptr2.next.next;

            } else /* This is tricky */ {
                ptr2 = ptr2.next;
            }
        }
        ptr1 = ptr1.next;
    }
}
function setFilteredDropdown() {
    setTimeout(() => {
        $('.filteredDropdown').select2({
            placeholder: 'Select',
            allowClear: true
        })
    }, 400)
}
function setDateTimeforCards(card) {
    setTimeout(() => {
        switch (card) {
            case 'OEHBL':
                const ids = [
                    'OceanExportHbl_PorEtd',
                    'OceanExportHbl_PolEtd',
                    'OceanExportHbl_PodEta',
                    'OceanExportHbl_DelEta',
                    'OceanExportHbl_FdestEta',
                    'OceanExportHbl_CargoArrivalDate',
                    'OceanExportHbl_HblWhCutOffTime',
                    'OceanExportHbl_EarlyReturnDatetime',
                    'OceanExportHbl_LcIssueDate',
                    'OceanExportHbl_OnBoardDate',
                    'OceanExportHbl_HblReleaseDate',
                    'OceanExportHbl_CanceledDate'
                ];

                ids.forEach(function (id) {
                    let dateElem = $('#' + id);

                    if (dateElem.length === 0) {
                        return;
                    }

                    dateElem.removeAttr('type').datetimepicker({
                        format: 'Y-m-d H:i',
                        step: 15,
                        allowInput: false
                    });

                    let currentVal = dateElem.val();
                    if (currentVal.includes('T')) {
                        dateElem.val(currentVal.replace('T', ' '));
                    }
                });
                break;
            case 'OIHBL':

                var hblId = $('#OceanImportHBL').val();
                dolphin.freight.importExport.oceanImports.oceanImportHbl.get(hblId).done(function (res) {
                    if (res.isLocked) {
                        $('#lockButtonHbl').empty().html('<i class="fa fa-lock-open"></i> ' + l("Btn:UnLock"));
                        $('#ActionDropHbl').empty().html('<i class="fa fa-lock"></i> ' + l("Display:Function"));
                    } else {
                        $('#lockButtonHbl').empty().html('<i class="fa fa-lock"></i> ' + l("Btn:Lock"));
                        $('#ActionDropHbl').empty().html('<i class="fa fa-lock-open"></i> ' + l("Display:Function"));
                    }
                });

                const OceanImportids = [
                    "OceanImportHbl_PorEtd",
                    "OceanImportHbl_PodEta",
                    "OceanImportHbl_DelEta",
                    "OceanImportHbl_FdestEta",
                    "OceanImportHbl_CargoArrivalDate",
                    "OceanImportHbl_HblWhCutOffTime",
                    "OceanImportHbl_EarlyReturnDatetime",
                    "OceanImportHbl_LcIssueDate",
                    "OceanImportHbl_OnBoardDate",
                    "OceanImportHbl_HblReleaseDate",
                    "OceanImportHbl_CanceledDate"
                ];

                OceanImportids.forEach(function (id) {
                    let dateElem = $('#' + id);

                    if (dateElem.length === 0) {
                        return;
                    }

                    dateElem.removeAttr('type').datetimepicker({
                        format: 'Y-m-d H:i',
                        step: 15,
                        allowInput: false
                    });

                    let currentVal = dateElem.val();
                    if (currentVal.includes('T')) {
                        dateElem.val(currentVal.replace('T', ' '));
                    }
                });
                break;
            default:
        }
    }, 1000);
}
function DangerousGoods(url) {
    myWindow = window.open(url, "Dangerous Goods", 'width=1200,height=800')
    myWindow.focus()
}
function getPickupDeliveryOrderAirExportHawb(url) {
    myWindow = window.open(url, 'Empty', 'width=1200,height=800');
    myWindow.focus()
}

function DocumentPackage(url) {
    myWindow = window.open(url, "Document Package", 'width=1200,height=800');
    myWindow.focus();
}
function getPreAlertAirExportHawb(url) {
    myWindow = window.open(url, 'Empty', 'width=1200,height=800');
    myWindow.focus()
}
function getCommercialInvoiceAirExportHawb(url) {
    myWindow = window.open(url, 'Empty', 'width=1200,height=800');
    myWindow.focus()
}
function getPackageLabelAirExportMawb(url) {
    myWindow = window.open(url, 'Empty', 'width=1200,height=800');
    myWindow.focus()
}
function getBookingConfirmationAirExportMawb(url) {
    myWindow = window.open(url, 'Empty', 'width=1200,height=800');
    myWindow.focus()
}
function getAllHawbPackageLabelAirExportMawb(url) {
    myWindow = window.open(url, 'Empty', 'width=1200,height=800');
    myWindow.focus();
}
function OpenWindow(url) {
    myWindow = window.open(url, 'Empty', 'width=1200,height=800');
    myWindow.focus();
}
function checkGuid(id) {
    return id && id.length == 36;
}
function copyHawb(hawbId) {
    let hawbcard = createHawbCard();
    let index = $('.hbl_sm_area').find('.card').length;
    hawbcard = setHawbCardValues(hawbcard, hawbId, '0', index);
    $('#hblCards').append(hawbcard);
    setTimeout(() => {
        $('.hblCardTitle')[index].click();
    }, 500);
}

class CustomDateTimePicker {
    dateTimePicker(Ids, CurrentDate = false, DefaultTimeIds) {
        Ids.forEach(function (id) {
            let dateElem = $('#' + id);
            let formattedDate;

            if (dateElem.length === 0) {
                return;
            }

            if (CurrentDate) {
                let currentDate = new Date();
                formattedDate = currentDate.getFullYear() +
                    '-' + ('0' + (currentDate.getMonth() + 1)).slice(-2) +
                    '-' + ('0' + currentDate.getDate()).slice(-2) +
                    ' ' + ('0' + currentDate.getHours()).slice(-2) +
                    ':' + ('0' + currentDate.getMinutes()).slice(-2);
            }

            dateElem.removeAttr('type').datetimepicker({
                format: 'Y-m-d H:i',
                step: 15,
                allowInput: false,
                value: formattedDate,
                onChangeDateTime: function (dp, $input) {
                    if (DefaultTimeIds.length > 1) {
                        if (DefaultTimeIds.includes(id)) {
                            dateElem.val(dp.getFullYear() + '-' + ('0' + (dp.getMonth() + 1)).slice(-2) + '-' + ('0' + dp.getDate()).slice(-2) + ' 00:00');
                        }
                    }
                }
            });

            let currentVal = dateElem.val();
            if (currentVal.includes('T')) {
                dateElem.val(currentVal.replace('T', ' '));
            }
        });
    }
}