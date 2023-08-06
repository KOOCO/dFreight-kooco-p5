﻿$(function () {
    var l = abp.localization.getResource('Freight');
    $("#OceanImportMbl_PorEtd").datepicker({
        format: "yyyy-mm-dd",
        startDate: '-15d',
        endDate: '+60d'
    });
    $("#OceanImportMbl_PolEtd").datepicker({
        format: "yyyy-mm-dd",
        startDate: '-15d',
        endDate: '+60d'
    });
    $("#OceanImportMbl_PodEta").datepicker({
        format: "yyyy-mm-dd",
        startDate: '-15d',
        endDate: '+60d'
    });
    $("#OceanImportMbl_DelEta").datepicker({
        format: "yyyy-mm-dd",
        startDate: '-15d',
        endDate: '+60d'
    });
    $("#OceanImportMbl_FdestEta").datepicker({
        format: "yyyy-mm-dd",
        startDate: '-15d',
        endDate: '+60d'
    });
    $("#OceanImportHbl_PorEtd").datepicker({
        format: "yyyy-mm-dd",
        startDate: '-15d',
        endDate: '+60d'
    });
    $("#OceanImportHbl_PolEtd").datepicker({
        format: "yyyy-mm-dd",
        startDate: '-15d',
        endDate: '+60d'
    });
    $("#OceanImportHbl_PodEta").datepicker({
        format: "yyyy-mm-dd",
        startDate: '-15d',
        endDate: '+60d'
    });
    $("#OceanImportHbl_DelEta").datepicker({
        format: "yyyy-mm-dd",
        startDate: '-15d',
        endDate: '+60d'
    });
    $("#OceanImportHbl_FdestEta").datepicker({
        format: "yyyy-mm-dd",
        startDate: '-15d',
        endDate: '+60d'
    });
    $("#OceanImportMbl_CanceledDate").datepicker({
        format: "yyyy-mm-dd",
        startDate: '-15d',
        endDate: '+60d'
    });
    $("#OceanImportHbl_PorEtd").datepicker({
        format: "yyyy-mm-dd",
        startDate: '-15d',
        endDate: '+60d'
    });
    $("#OceanImportHbl_PodEta").datepicker({
        format: "yyyy-mm-dd",
        startDate: '-15d',
        endDate: '+60d'
    });
    $("#OceanImportHbl_DelEta").datepicker({
        format: "yyyy-mm-dd",
        startDate: '-15d',
        endDate: '+60d'
    });
    $("#OceanImportHbl_FdestEta").datepicker({
        format: "yyyy-mm-dd",
        startDate: '-15d',
        endDate: '+60d'
    });
    $("#OceanImportHbl_CargoArrivalDate").datepicker({
        format: "yyyy-mm-dd",
        startDate: '-15d',
        endDate: '+60d'
    });

    $("#OceanImportMbl_Etb").datepicker({
        format: "yyyy-mm-dd",
        startDate: '-15d',
        endDate: '+60d'
    });

    $("#OceanImportMbl_ItDate").datepicker({
        format: "yyyy-mm-dd",
        startDate: '-15d',
        endDate: '+60d'
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
 
    $("#saveBtn").click(function () {
        $("#mOfficeId").val($("#OfficeId").val());
        var OfficeId = $("#mOfficeId").val();
        if (OfficeId == "" || OfficeId == "00000000-0000-0000-0000-000000000000") {
            $("#err_OfficeId").show();
        } else {
            $("#hHblSalesTypeId").val($("#HblSalesTypeId").val());
            $("#mPreCarriageVesselNameId").val($("#PreCarriageVesselNameId").val());
            $("#mSvcTermFromId").val($("#SvcTermFromId").val());
            $("#mSvcTermToId").val($("#SvcTermToId").val());
            $("#mShipModeId").val($("#ShipModeId").val());
            $("#mFreightTermId").val($("#FreightTermId").val());
            $("#mBlTypeId").val($("#BlTypeId").val());
            $("#mMblCarrierId").val($("#MblCarrierId").val());
            $("#mBlAcctCarrierId").val($("#BlAcctCarrierId").val());
            $("#mShippingAgentId").val($("#ShippingAgentId").val());
            $("#mMblOverseaAgentId").val($("#MblOverseaAgentId").val());
            $("#mMblNotifyId").val($("#MblNotifyId").val());
            $("#mMblReferralById").val($("#MblReferralById").val());
            $("#mEmptyPickupId").val($("#EmptyPickupId").val());
            $("#mDeliveryToId").val($("#DeliveryToId").val());
            $("#mCoLoaderId").val($("#CoLoaderId").val());
            $("#mForwardingAgentId").val($("#ForwardingAgentId").val());
            $("#mCareOfId").val($("#CareOfId").val());
            $("#mPorId").val($("#PorId").val());
            $("#mPolId").val($("#PolId").val());
            $("#mPodId").val($("#PodId").val());
            $("#mDelId").val($("#DelId").val());
            $("#mFdestId").val($("#FdestId").val());
            $("#mTransPort1Id").val($("#TransPort1Id").val());
            $("#mOblTypeId").val($("#OblTypeId").val());
            $("#hCargoTypeId").val($("#CargoTypeId").val());
            $("#hHSvcTermFromId").val($("#HSvcTermFromId").val());
            $("#hHSvcTermToId").val($("#HSvcTermToId").val());
            $("#hHShipModeId").val($("#HShipModeId").val());
            $("#hFreightTermForBuyerId").val($("#FreightTermForBuyerId").val());
            $("#hFreightTermForSalerId").val($("#FreightTermForSalerId").val());
            $("#hHblCustomerId").val($("#HblCustomerId").val());
            $("#hHblShipperId").val($("#HblShipperId").val());
            $("#hHblBillToId").val($("#HblBillToId").val());
            $("#hHblConsigneeId").val($("#HblConsigneeId").val());
            $("#hHblNotifyId").val($("#HblNotifyId").val());
            $("#hCustomsBrokerId").val($("#CustomsBrokerId").val());
            $("#hTruckerId").val($("#TruckerId").val());
            $("#hAgentId").val($("#AgentId").val());
            $("#hHblForwardingAgentId").val($("#HblForwardingAgentId").val());
            $("#hReceivingAgentId").val($("#ReceivingAgentId").val());
            $("#hFbaFcId").val($("#FbaFcId").val());
            $("#hHEmptyPickupId").val($("#HEmptyPickupId").val());
            $("#hHDeliveryToId").val($("#HDeliveryToId").val());
            $("#hHMblReferralById").val($("#HMblReferralById").val());
            $("#hHPorId").val($("#HPorId").val());
            $("#hHPodId").val($("#HPodId").val());
            $("#hHDelId").val($("#HDelId").val());
            $("#hHFdestId").val($("#HFdestId").val());
            $("#mMblCarrierContent").val($("#MblCarrierContent").val());
            $("#mBlAcctCarrierContent").val($("#BlAcctCarrierContent").val());
            $("#mShippingAgentContent").val($("#ShippingAgentContent").val());
            $("#mMblOverseaAgentContent").val($("#MblOverseaAgentContent").val());
            $("#mMblNotifyContent").val($("#MblNotifyContent").val());
            $("#mForwardingAgentContent").val($("#ForwardingAgentContent").val());
            $("#mCoLoaderContent").val($("#CoLoaderContent").val());
            $("#mCareOfContent").val($("#CareOfContent").val());
            $("#mEmptyPickupContent").val($("#EmptyPickupContent").val());
            $("#mDeliveryToContent").val($("#DeliveryToContent").val());
            $("#mMblReferralByContent").val($("#MblReferralByContent").val());
            $("#mCancelById").val($("#CancelById").val());
            $("#mCancelReason").val($("#CancelReason").val());

            $("#mMblCustomerId").val($("#MblCustomerId").val());
            $("#mMblBillToId").val($("#MblBillToId").val());
            $("#mMblSalesTypeId").val($("#MblSalesTypeId").val());
            $("#mCargoTypeId").val($("#MblCargoTypeId").val());
            $("#mMblShipperId").val($("#MblShipperId").val());
            $("#mMblConsigneeId").val($("#MblConsigneeId").val());
            $("#mMblNotifyId").val($("#MblNotifyId").val());

            $("#createForm").submit();
        }

    });
    $("#saveEditBtn").click(function () {
        var OfficeId = $("#mOfficeId").val();
        if (OfficeId == "" || OfficeId == "00000000-0000-0000-0000-000000000000") {
            $("#err_OfficeId").show();
        } else {
            $("#hHblSalesTypeId").val($("#HblSalesTypeId").val());
            $("#mPreCarriageVesselNameId").val($("#PreCarriageVesselNameId").val());
            $("#mSvcTermFromId").val($("#SvcTermFromId").val());
            $("#mSvcTermToId").val($("#SvcTermToId").val());
            $("#mShipModeId").val($("#ShipModeId").val());
            $("#mFreightTermId").val($("#FreightTermId").val());
            $("#mBlTypeId").val($("#BlTypeId").val());
            $("#mMblCarrierId").val($("#MblCarrierId").val());
            $("#mBlAcctCarrierId").val($("#BlAcctCarrierId").val());
            $("#mShippingAgentId").val($("#ShippingAgentId").val());
            $("#mMblOverseaAgentId").val($("#MblOverseaAgentId").val());
            $("#mMblNotifyId").val($("#MblNotifyId").val());
            $("#mMblReferralById").val($("#MblReferralById").val());
            $("#mEmptyPickupId").val($("#EmptyPickupId").val());
            $("#mDeliveryToId").val($("#DeliveryToId").val());
            $("#mCoLoaderId").val($("#CoLoaderId").val());
            $("#mForwardingAgentId").val($("#ForwardingAgentId").val());
            $("#mCareOfId").val($("#CareOfId").val());
            $("#mPorId").val($("#PorId").val());
            $("#mPolId").val($("#PolId").val());
            $("#mPodId").val($("#PodId").val());
            $("#mDelId").val($("#DelId").val());
            $("#mFdestId").val($("#FdestId").val());
            $("#mTransPort1Id").val($("#TransPort1Id").val());
            $("#hCargoTypeId").val($("#CargoTypeId").val());
            $("#hHSvcTermFromId").val($("#HSvcTermFromId").val());
            $("#hHSvcTermToId").val($("#HSvcTermToId").val());
            $("#hHShipModeId").val($("#HShipModeId").val());
            $("#hFreightTermForBuyerId").val($("#FreightTermForBuyerId").val());
            $("#hFreightTermForSalerId").val($("#FreightTermForSalerId").val());
            $("#hHblCustomerId").val($("#HblCustomerId").val());
            $("#hHblShipperId").val($("#HblShipperId").val());
            $("#hHblBillToId").val($("#HblBillToId").val());
            $("#hHblConsigneeId").val($("#HblConsigneeId").val());
            $("#hHblNotifyId").val($("#HblNotifyId").val());
            $("#hCustomsBrokerId").val($("#CustomsBrokerId").val());
            $("#hTruckerId").val($("#TruckerId").val());
            $("#hAgentId").val($("#AgentId").val());
            $("#hHblForwardingAgentId").val($("#HblForwardingAgentId").val());
            $("#hReceivingAgentId").val($("#ReceivingAgentId").val());
            $("#hFbaFcId").val($("#FbaFcId").val());
            $("#hHEmptyPickupId").val($("#HEmptyPickupId").val());
            $("#hHDeliveryToId").val($("#HDeliveryToId").val());
            $("#hHMblReferralById").val($("#HMblReferralById").val());
            $("#hHPorId").val($("#HPorId").val());
            $("#hHPodId").val($("#HPodId").val());
            $("#hHDelId").val($("#HDelId").val());
            $("#hHFdestId").val($("#HFdestId").val());
            $("#mMblCarrierContent").val($("#MblCarrierContent").val());
            $("#mBlAcctCarrierContent").val($("#BlAcctCarrierContent").val());
            $("#mShippingAgentContent").val($("#ShippingAgentContent").val());
            $("#mMblOverseaAgentContent").val($("#MblOverseaAgentContent").val());
            $("#mMblNotifyContent").val($("#MblNotifyContent").val());
            $("#mForwardingAgentContent").val($("#ForwardingAgentContent").val());
            $("#mCoLoaderContent").val($("#CoLoaderContent").val());
            $("#mCareOfContent").val($("#CareOfContent").val());
            $("#mEmptyPickupContent").val($("#EmptyPickupContent").val());
            $("#mDeliveryToContent").val($("#DeliveryToContent").val());
            $("#mMblReferralByContent").val($("#MblReferralByContent").val());
            $("#mCancelById").val($("#CancelById").val());
            $("#mCancelReason").val($("#CancelReason").val());
            $("#mOblTypeId").val($("#OblTypeId").val());

            $("#mMblCustomerId").val($("#MblCustomerId").val());
            $("#mMblBillToId").val($("#MblBillToId").val());
            $("#mMblSalesTypeId").val($("#MblSalesTypeId").val());
            $("#mCargoTypeId").val($("#MblCargoTypeId").val());
            $("#mMblShipperId").val($("#MblShipperId").val());
            $("#mMblConsigneeId").val($("#MblConsigneeId").val());
            $("#mMblNotifyId").val($("#MblNotifyId").val());

            $("#editForm").submit();
        }
    });
    $("#OceanImportMbl_MblNo").change(function () {
        $("#title0").text($(this).val());
    });
    $("#OceanImportMbl_PolEtd").change(function () {
        $("#title2").text("("+$(this).val()+")");
    });
    $("#OceanImportMbl_PodEta").change(function () {
        $("#title4").text("("+$(this).val()+")");
    });
    
    $("#OceanImportMbl_VesselName").change(function () {
        $("#title5").text('<i class="fa fa-anchor"></i>'+$(this).val());
    });
    $('#createForm').on('abp-ajax-success', function (result, rs) {
        event.preventDefault();
        location.href = 'EditModal?ShowMsg=true&Id=' + rs.responseText.id 
        
    });
    
    initHideBtn();
    $("#checkHideBtn").click(function () {
        initHideBtn();
    });
    initIsDirect()
    $("#OceanImportMbl_IsDirect").change(function () {
        initIsDirect();
    });
    initReasonStatus();
    initHReasonStatus();
    $("#OceanImportMbl_IsCanceled").change(function () {
        initReasonStatus()
    });
    $("#OceanImportHbl_IsCanceled").change(function () {
        initHReasonStatus()
    });
});

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
function initIsDirect() {
    if ($('#OceanImportMbl_IsDirect').prop('checked')) {
        $(".isDirect").show();
    } else {
        $(".isDirect").hide();
    }
}
function showMblPrint(id) {
    url = "/OceanImports/PrintImport/MblPrint?Id="+id
    window.open(url, 'printpage');
}
function initReasonStatus() {
    var locked = !$("#OceanImportMbl_IsCanceled").is(":checked");
    $("#OceanImportMbl_CanceledDate").attr('readonly', locked);

    $("#drop_CancelReason").attr('disabled', locked);
    if (!locked) {
        $("#OceanImportMbl_CancelByName").val(abp.currentUser.userName);
        $("#CancelById").val(abp.currentUser.id);
    } else {
        $("#OceanImportMbl_CancelByName").val("");
        $("#CancelById").val("");
    }
}
function initHReasonStatus() {
    var locked = !$("#OceanImportHbl_IsCanceled").is(":checked");
    $("#OceanImportHbl_CanceledDate").attr('readonly', locked);

    $("#drop_CancelReason").attr('disabled', locked);
    if (!locked) {
        $("#OceanImportMbl_CancelByName").val(abp.currentUser.userName);
        $("#CancelById").val(abp.currentUser.id);
    } else {
        $("#OceanImportMbl_CancelByName").val("");
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