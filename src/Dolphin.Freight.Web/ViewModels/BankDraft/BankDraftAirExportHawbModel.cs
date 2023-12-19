using System;

namespace Dolphin.Freight.Web.ViewModels.BankDraft
{
    public class BankDraftAirExportHawbModel
    {
        public string BaseUrl { get; set; }
        public string Amount { get; set; }
        public string Date { get; set; }
        public string AtSight { get; set; }
        //實際託運人
        public string ShipperName { get; set; }
        public string DrawnDocCredit { get; set; }
        //信用狀開立日期
        public string IssueDate { get; set; }
        //信用狀開立銀行
        public string LCIssueBank { get; set; }
        public string ToLCIssueBank { get; set; }
        //選擇Consignee或NotifyParty
        public string ToTradePartnerLoadFrom { get; set; }
        //收貨人
        public string Consignee { get; set; }
        //通知方
        public string NotifyParty { get; set; }
        public string ToTradePartnerName { get; set; }
        public string DraftNo { get; set; }
        public string BankPreference { get; set; }
        //實際託運人
        public string ShipperName2 { get; set; }
        public string Gentlemen { get; set; }
        public string GentlemenNameAddress { get; set; }
        public string EncloseDate { get; set; }
        public string EncloseDraftNo { get; set; }
        public string EncloseForCollection { get; set; }
        public string EncloseForOther { get; set; }
        public string EncloseForOtherInput { get; set; }
        public string EncloseForPayment { get; set; }
        public string ExtraBl { get; set; }
        public string ExtraBlCopy { get; set; }
        public string ExtraCommInv { get; set; }
        public string ExtraInsCtf { get; set; }
        public string ExtraCtfOrig { get; set; }
        public string ExtraConsInv { get; set; }
        public string ExtraPkngList { get; set; }
        public string ExtraWgtCtf { get; set; }
        public string DeliverInOneMailing { get; set; }
        public string DeliverInTwoMailing { get; set; }
        public string DeliverIfDraft { get; set; }
        public string AllChargesForAccount { get; set; }
        public string DoNotWaiveCharges { get; set; }
        public string ProtestForNonPayment { get; set; }
        public string DoNotProtest { get; set; }
        public string PresentOnArrival { get; set; }
        public string AdviseNonPaymentBy { get; set; }
        public string AdvisePaymentBy { get; set; }
        //操作者所屬分站名稱
        public string ReferToName { get; set; }
        public string ReferToAddress { get; set; }
        public string ToActFullyOnOurBehalf { get; set; }
        public string ToAssistNotToAlter { get; set; }
        public string OtherInstructions { get; set; }
        public string ReferQuestionTo { get; set; }
        public string ReferQuestionName { get; set; }
        public string ReferQuestionPhone { get; set; }
        //Shipper
        public string ReferQuestionShipperName { get; set; }
        public string ReferQuestionShipperPhone { get; set; }
        //Freight Forwarder
        public string ReferQuestionFreightForwarderName { get; set; }
        public string ReferQuestionFreightForwarderPhone { get; set; }
        //實際託運人
        public string ShipperName3 { get; set; }
        public string AuthorizedSignatureInput { get; set; }
        //操作者名稱+/+所屬分站名稱
        public string UserCompany { get; set; }

        public string AmountToString { get; set; }

        //ReportLog
        public Guid ReportId { get; set; }
    }
}
