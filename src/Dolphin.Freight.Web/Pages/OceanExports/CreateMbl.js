$(function () {
    var l = abp.localization.getResource('Freight');
    $(document).ready(function () {
        const ids = [
            'OceanExportMbl_PostDate',
            'OceanExportMbl_PorEtd',
            'OceanExportMbl_PolEtd',
            'OceanExportMbl_PodEta',
            'OceanExportMbl_DelEta',
            'OceanExportMbl_FdestEta',
            'OceanExportMbl_DocCutOffTime',
            'OceanExportMbl_PortCutOffTime',
            'OceanExportMbl_VgmCutOffTime',
            'OceanExportMbl_RailCutOffTime',
            'OceanExportMbl_OnBoardDate', 
            'OceanExportMbl_Trans1Eta'
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
    });


    tempusDominus.extend(window.tempusDominus.plugins.customDateFormat)
    $(".cdatetime").tempusDominus({
        restrictions: {
            minDate: '2023-02-10 00:00',
            maxDate: '2023-04-10 00:00'
        },
        useCurrent: true,

        localization: {
            format: 'yyyy-MM-dd HH:mm',
        }
    });


    $(".cdatetime").change(function () {
        checkDateTime($(this).attr("id"), $(this).val())
    })

   
    //$("#saveBtn").click(function () {
    //    debugger;
    //    $("#mOfficeId").val($('#OceanExportMbl_OfficeId').val());
    //        //var OfficeId = $("#mOfficeId").val();
    //        //if (OfficeId == "" || OfficeId == "00000000-0000-0000-0000-000000000000" ) {
    //        //    $("#err_OfficeId").show();
    //        //} else {
    //            $("#mMblSalesTypeId").val($("#MblSalesTypeId").val());
    //            $("#mPreCarriageVesselNameId").val($("#PreCarriageVesselNameId").val());
    //            $("#mSvcTermFromId").val($("#SvcTermFromId").val());
    //            $("#mSvcTermToId").val($("#SvcTermToId").val());
    //            $("#mShipModeId").val($("#ShipModeId").val());
    //            $("#mFreightTermId").val($("#FreightTermId").val());
    //            $("#mBlTypeId").val($("#BlTypeId").val());
    //            $("#mMblCarrierId").val($("#MblCarrierId").val());
    //            $("#mBlAcctCarrierId").val($("#BlAcctCarrierId").val());
    //            $("#mShippingAgentId").val($("#ShippingAgentId").val());
    //            $("#mMblOverseaAgentId").val($("#MblOverseaAgentId").val());
    //            $("#mMblNotifyId").val($("#MblNotifyId").val());
    //            $("#mMblReferralById").val($("#MblReferralById").val());
    //            $("#mEmptyPickupId").val($("#EmptyPickupId").val());
    //            $("#mDeliveryToId").val($("#DeliveryToId").val());
    //            $("#mCoLoaderId").val($("#CoLoaderId").val());
    //            $("#mForwardingAgentId").val($("#ForwardingAgentId").val());
    //            $("#mCareOfId").val($("#CareOfId").val());
    //            $("#mPorId").val($("#PorId").val());
    //            $("#mPolId").val($("#PolId").val());
    //            $("#mPodId").val($("#PodId").val());
    //            $("#mDelId").val($("#DelId").val());
    //            $("#mFdestId").val($("#FdestId").val());
    //            $("#mTransPort1Id").val($("#TransPort1Id").val());
    //            $("#mOblTypeId").val($("#OblTypeId").val());
    //            $("#hCargoTypeId").val($("#CargoTypeId").val());
    //            $("#hHSvcTermFromId").val($("#HSvcTermFromId").val());
    //            $("#hHSvcTermToId").val($("#HSvcTermToId").val());
    //            $("#hHShipModeId").val($("#HShipModeId").val());
    //            $("#hFreightTermForBuyerId").val($("#FreightTermForBuyerId").val());
    //            $("#hFreightTermForSalerId").val($("#FreightTermForSalerId").val());
    //            $("#hHblCustomerId").val($("#HblCustomerId").val());
    //            $("#hHblShipperId").val($("#HblShipperId").val());
    //            $("#hHblBillToId").val($("#HblBillToId").val());
    //            $("#hHblConsigneeId").val($("#HblConsigneeId").val());
    //            $("#hHblNotifyId").val($("#HblNotifyId").val());
    //            $("#hCustomsBrokerId").val($("#CustomsBrokerId").val());
    //            $("#hTruckerId").val($("#TruckerId").val());
    //            $("#hAgentId").val($("#AgentId").val());
    //            $("#hHblForwardingAgentId").val($("#HblForwardingAgentId").val());
    //            $("#hReceivingAgentId").val($("#ReceivingAgentId").val());
    //            $("#hFbaFcId").val($("#FbaFcId").val());
    //            $("#hHEmptyPickupId").val($("#HEmptyPickupId").val());
    //            $("#hHDeliveryToId").val($("#HDeliveryToId").val());
    //            $("#hHMblReferralById").val($("#HMblReferralById").val());
    //            $("#hHPorId").val($("#HPorId").val());
    //            $("#hHPodId").val($("#HPodId").val());
    //            $("#hHDelId").val($("#HDelId").val());
    //            $("#hHFdestId").val($("#HFdestId").val());
    //            $("#mMblCarrierContent").val($("#MblCarrierContent").val());
    //            $("#mBlAcctCarrierContent").val($("#BlAcctCarrierContent").val());
    //            $("#mShippingAgentContent").val($("#ShippingAgentContent").val());
    //            $("#mMblOverseaAgentContent").val($("#MblOverseaAgentContent").val());
    //            $("#mMblNotifyContent").val($("#MblNotifyContent").val());
    //            $("#mForwardingAgentContent").val($("#ForwardingAgentContent").val());
    //            $("#mCoLoaderContent").val($("#CoLoaderContent").val());
    //            $("#mCareOfContent").val($("#CareOfContent").val());
    //            $("#mEmptyPickupContent").val($("#EmptyPickupContent").val());
    //            $("#mDeliveryToContent").val($("#DeliveryToContent").val());
    //            $("#mMblReferralByContent").val($("#MblReferralByContent").val());
    //            $("#mCancelById").val($("#CancelById").val());
    //            $("#mCancelReason").val($("#CancelReason").val());

    //            $("#createForm").submit();
    //        /*}*/

    //});

    //$("#saveEditBtn").click(function () {
    //    debugger;
    //    var OfficeId = $("#mOfficeId").val();
    //    if (OfficeId == "" || OfficeId == "00000000-0000-0000-0000-000000000000") {
    //        $("#err_OfficeId").show();
    //    } else {
    //        $("#mMblSalesTypeId").val($("#MblSalesTypeId").val());
    //        $("#mPreCarriageVesselNameId").val($("#PreCarriageVesselNameId").val());
    //        $("#mSvcTermFromId").val($("#SvcTermFromId").val());
    //        $("#mSvcTermToId").val($("#SvcTermToId").val());
    //        $("#mShipModeId").val($("#ShipModeId").val());
    //        $("#mFreightTermId").val($("#FreightTermId").val());
    //        $("#mBlTypeId").val($("#BlTypeId").val());
    //        $("#mMblCarrierId").val($("#MblCarrierId").val());
    //        $("#mBlAcctCarrierId").val($("#BlAcctCarrierId").val());
    //        $("#mShippingAgentId").val($("#ShippingAgentId").val());
    //        $("#mMblOverseaAgentId").val($("#MblOverseaAgentId").val());
    //        $("#mMblNotifyId").val($("#MblNotifyId").val());
    //        $("#mMblReferralById").val($("#MblReferralById").val());
    //        $("#mEmptyPickupId").val($("#EmptyPickupId").val());
    //        $("#mDeliveryToId").val($("#DeliveryToId").val());
    //        $("#mCoLoaderId").val($("#CoLoaderId").val());
    //        $("#mForwardingAgentId").val($("#ForwardingAgentId").val());
    //        $("#mCareOfId").val($("#CareOfId").val());
    //        debugger;
    //        $("#mPorId").val($("#PorId").val());
    //        $("#mPolId").val($("#PolId").val());
    //        $("#mPodId").val($("#PodId").val());
    //        $("#mDelId").val($("#DelId").val());
    //        $("#mFdestId").val($("#FdestId").val());
    //        $("#mTransPort1Id").val($("#TransPort1Id").val());
    //        $("#hCargoTypeId").val($("#CargoTypeId").val());
    //        $("#hHSvcTermFromId").val($("#HSvcTermFromId").val());
    //        $("#hHSvcTermToId").val($("#HSvcTermToId").val());
    //        $("#hHShipModeId").val($("#HShipModeId").val());
    //        $("#hFreightTermForBuyerId").val($("#FreightTermForBuyerId").val());
    //        $("#hFreightTermForSalerId").val($("#FreightTermForSalerId").val());
    //        $("#hHblCustomerId").val($("#HblCustomerId").val());
    //        $("#hHblShipperId").val($("#HblShipperId").val());
    //        $("#hHblBillToId").val($("#HblBillToId").val());
    //        $("#hHblConsigneeId").val($("#HblConsigneeId").val());
    //        $("#hHblNotifyId").val($("#HblNotifyId").val());
    //        $("#hCustomsBrokerId").val($("#CustomsBrokerId").val());
    //        $("#hTruckerId").val($("#TruckerId").val());
    //        $("#hAgentId").val($("#AgentId").val());
    //        $("#hHblForwardingAgentId").val($("#HblForwardingAgentId").val());
    //        $("#hReceivingAgentId").val($("#ReceivingAgentId").val());
    //        $("#hFbaFcId").val($("#FbaFcId").val());
    //        $("#hHEmptyPickupId").val($("#HEmptyPickupId").val());
    //        $("#hHDeliveryToId").val($("#HDeliveryToId").val());
    //        $("#hHMblReferralById").val($("#HMblReferralById").val());
    //        $("#hHPorId").val($("#HPorId").val());
    //        $("#hHPodId").val($("#HPodId").val());
    //        $("#hHDelId").val($("#HDelId").val());
    //        $("#hHFdestId").val($("#HFdestId").val());
    //        $("#mMblCarrierContent").val($("#MblCarrierContent").val());
    //        $("#mBlAcctCarrierContent").val($("#BlAcctCarrierContent").val());
    //        $("#mShippingAgentContent").val($("#ShippingAgentContent").val());
    //        $("#mMblOverseaAgentContent").val($("#MblOverseaAgentContent").val());
    //        $("#mMblNotifyContent").val($("#MblNotifyContent").val());
    //        $("#mForwardingAgentContent").val($("#ForwardingAgentContent").val());
    //        $("#mCoLoaderContent").val($("#CoLoaderContent").val());
    //        $("#mCareOfContent").val($("#CareOfContent").val());
    //        $("#mEmptyPickupContent").val($("#EmptyPickupContent").val());
    //        $("#mDeliveryToContent").val($("#DeliveryToContent").val());
    //        $("#mMblReferralByContent").val($("#MblReferralByContent").val());
    //        $("#mCancelById").val($("#CancelById").val());
    //        $("#mCancelReason").val($("#CancelReason").val());
    //        $("#mOblTypeId").val($("#OblTypeId").val());
    //        $("#editForm").submit();
    //    }
        
    //});
    $("#OceanExportMbl_MblNo").change(function () {
        $("#title0").text($(this).val());
    });
    $("#OceanExportMbl_PolEtd").change(function () {
        $("#title2").text("("+$(this).val()+")");
    });
    $("#OceanExportMbl_PodEta").change(function () {
        $("#title4").text("("+$(this).val()+")");
    });
    
    $("#OceanExportMbl_VesselName").change(function () {
        $("#title5").text('<i class="fa fa-anchor"></i>'+$(this).val());
    });
    $('#editForm').on('abp-ajax-success', function () {
        location.reload();
    });
    $('#createForm').on('abp-ajax-success', function (result, rs) {
        debugger;
        event.preventDefault();
        location.href = 'EditModal?ShowMsg=true&Id=' + rs.responseText.id + '&Hid=' + rs.responseText.HId 
        
    });
    
    initHideBtn();
    $("#checkHideBtn").click(function () {
        initHideBtn();
    });
    initHideHblBtn();
    $("#checkHideHblBtn").click(function () {
        initHideHblBtn();
    });
    initReasonStatus();
    initHReasonStatus();
    $("#OceanExportMbl_IsCanceled").change(function () {
        initReasonStatus()
    });
    $("#OceanExportHbl_IsCanceled").change(function () {
        initHReasonStatus()
    });
});

/*
function initPortsTag(selectItems, tagName, tagValue) {
    var drophtml = "";
    var l = abp.localization.getResource('Freight');
    var tag = l("Dropdown:Select");
    for (var i = 0; i < selectItems.length; i++) {
        drophtml = drophtml + "<li class='form-control' style='width:450px;'><a  style='width:400px;' class='dropdown-item'  href='#' onclick='changeDropdownValue(\"" + tagName + "\",\"" + selectItems[i].id + "\",\"" + selectItems[i].portName + "\")'>" + selectItems[i].portName + "(" + selectItems[i].locode +")"+ "</div></a></li>"
        if (tagValue == selectItems[i].id) tag = selectItems[i].portName;
    }
    $("#drop_" + tagName).html(tag);
    $("#dropdownMenuButton_" + tagName).html(drophtml);
}
function initSysCodeTag(selectItems, tagName, tagValue)
{
    var drophtml = "";
    var l = abp.localization.getResource('Freight');
    var tag = l("Dropdown:Select");
    for (var i = 0; i < selectItems.length; i++) {
        drophtml = drophtml + "<li class='form-control' style='width:450px;'><a  style='width:400px;' class='dropdown-item'  href='#' onclick='changeDropdownValue(\"" + tagName + "\",\"" + selectItems[i].codeValue + "\",\"" + selectItems[i].showName + "\")'>" + selectItems[i].showName +  "</div></a></li>"
        if (tagValue == selectItems[i].codeValue) tag = selectItems[i].showName;
    }
    $("#drop_" + tagName).html(tag);
    $("#dropdownMenuButton_" + tagName).html(drophtml);
}

function initSubstationsTag(selectItems, tagName, tagValue) {
    var drophtml = "";
    var l = abp.localization.getResource('Freight');
    var tag = l("Dropdown:Select");
    for (var i = 0; i < selectItems.length; i++) {
        drophtml = drophtml + "<li class='form-control' style='width:450px;'><a  style='width:400px;' class='dropdown-item'  href='#' onclick='changeDropdownValue(\"" + tagName + "\",\"" + selectItems[i].id + "\",\"" + selectItems[i].substationName + "\")'>" + selectItems[i].substationName + "</div></a></li>"
        if (tagValue == selectItems[i].id) tag = selectItems[i].substationName;
    }
    $("#drop_" + tagName).html(tag);
    $("#dropdownMenuButton_" + tagName).html(drophtml);
}
function changeDropdownValue(tagName, tagValue, showCode)
{
    $("#" + tagName).val(tagValue);
    $("#drop_" + tagName).html(showCode);
}
function initTradePartnerSelect(selectItems, tagName, tagValue) {
    var l = abp.localization.getResource('Freight');
    var tag = l("Dropdown:Select");
    var drophtml = "<li class='dropdown-item' id='li_" + tagName +"' ><input type='text' class='form-control' id='qid_" + tagName +"'></li>";
    if (selectItems.length > 0) {
        for (var i = 0; i < selectItems.length; i++) {
            drophtml = drophtml + "<li class='form-control' style='width:450px;'><a  style='width:400px;' class='dropdown-item'  href='#' onclick='changeDropdownValue(\"" + tagName + "\",\"" + selectItems[i].id + "\",\"" + selectItems[i].tpName + "\")'><div class='col-sm-9'>" + selectItems[i].tpName + "</di><div class='col-sm-2 pull-right'>  " + selectItems[i].tpAliasName + "</div><div class='col-sm-12'> " + selectItems[i].tpCode + "</div></a></li>"
        }
    }
     $("#drop_" + tagName).html(tag);
    $("#dropdownMenuButton_" + tagName).html(drophtml);
}*/
function initHeader() {
    var polname = $("#drop_PolId").html();
    if (polname != '請選擇') $("#f1").html(polname);
    else $("#f1").html("");
}
function initHideBtn() {
    var isHide = $("#isHide").val();
    if (isHide == 1) {
        $(".hideArea").hide();
        $("#hideLi").hide();
        $("#showLi").show();
        $("#isHide").val(0);
    } else {
        $(".hideArea").show();
        $("#hideLi").show();
        $("#showLi").hide();
        $("#isHide").val(1);
    }

}
function initHideHblBtn() {
    var isHide = $("#isHideHbl").val();
    if (isHide == 1) {
        $(".hideAreaHbl").hide();
        $("#hideLiHbl").hide();
        $("#showLiHbl").show();
        $("#isHideHbl").val(0);
    } else {
        $(".hideAreaHbl").show();
        $("#hideLiHbl").show();
        $("#showLiHbl").hide();
        $("#isHideHbl").val(1);
    }

}
function showMblPrint(id) {
    url = "/OceanExports/PrintExport/MblPrint?Id="+id
    window.open(url, 'printpage');
}
function initReasonStatus() {
    var locked = !$("#OceanExportMbl_IsCanceled").is(":checked");
    $("#OceanExportMbl_CanceledDate").attr('readonly', locked);

    $("#drop_CancelReason").attr('disabled', locked);
    if (!locked) {
        $("#OceanExportMbl_CancelByName").val(abp.currentUser.userName);
        $("#CancelById").val(abp.currentUser.id);
    } else {
        $("#OceanExportMbl_CancelByName").val("");
        $("#CancelById").val("");
    }
}
function initHReasonStatus() {
    var locked = !$("#OceanExportHbl_IsCanceled").is(":checked");
    $("#OceanExportHbl_CanceledDate").attr('readonly', locked);

    $("#drop_CancelReason").attr('disabled', locked);
    if (!locked) {
        $("#OceanExportMbl_CancelByName").val(abp.currentUser.userName);
        $("#CancelById").val(abp.currentUser.id);
    } else {
        $("#OceanExportMbl_CancelByName").val("");
        $("#CancelById").val("");
    }
}
function checkDateTime(id,checkdate )
{
    var startdate = '2023-02-01';
    var enddate = '2023-04-01';
    var d = checkdate.toString().split(" ");
    var checkIndex = 0;
    if (d.length == 2) {
        var d2 = d[0].split("-");
        if (d2.length == 3) {
            if (d2[0].length == 4 ) {
                var date1 = new Date(d2[0], d2[1], d2[3]);
                var date2 = new Date(startdate);
                var date3 = new Date(enddate);
                var dateCheck1 = date2 > date1;
                var dateCheck2 = date1 > date3;
                var dateCheck3 = date2.getTime() - date3.getTime();
                if (dateCheck1) checkIndex = 1;
                if (dateCheck2) checkIndex = 1;

            } else {
                checkIndex = 1;
            }
            
           
        } else {
            console.log("2");
            checkIndex = 1;
        }
    } else {
        console.log("1");
        checkIndex = 1;
    }
    if (checkIndex > 0) {
        var date = new Date(startdate);
        $("#" + id).val(startdate+" 00:00");
    }
}