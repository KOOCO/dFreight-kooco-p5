using System;

namespace Dolphin.Freight.Web.ViewModels.BookingConfirmation
{
    public class BookingConfirmationAirExportMawbModel
    {
        public bool IsPDF { get; set; }
        public string File_No { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public string Flight_No { get; set; }
        public string Office { get; set; }
        //操作者地址
        public string Address { get; set; }
        //操作者電話
        public string Tel { get; set; }
        //操作者傳真
        public string Fax { get; set; }
        //操作者Email
        public string Email { get; set; }
        //操作者姓
        public string FirstName { get; set; }
        //操作者名
        public string LastName { get; set; }
        //當下日期
        public string Date { get; set; }
        //當下日期(含時間)
        public string DateTime { get; set; }
        //表單類別
        public string ReportTitleChoice { get; set; }
        //實際託運人/收貨人 or 本地/海外代理
        public string TradePartnerLayoutChoice { get; set; }
        //船公司訂艙號碼 
        public string CarrierBookingNo { get; set; }
        //實際託運人
        public string ActualShipper { get; set; }
        //收貨人
        public string Consignee { get; set; }
        //本地代理(分站 + MBL C/O)
        public string Shipping { get; set; }
        //海外代理(MBL 海外代理)
        public string OverseaAgent { get; set; }
        //HBL號碼
        public string MawbNo { get; set; }
        //我司訂艙號碼(S/O號碼)
        public string OutBookingNo { get; set; }
        //訂艙日期
        public string BookingDate { get; set; }
        //出口參考號碼(客戶參考編號)
        public string ExportRefNo { get; set; }
        //PO號碼
        public string PoNo { get; set; }
        //ITN號碼
        public string ItnNo { get; set; }
        //代理(HB/L代理-Print Name)
        public string Agent { get; set; }
        //通知人(通知方-Print Name)
        public string Notify { get; set; }
        //船名與航次(MBL 船名 + MBL 航次)
        public string Vessel_Voyage { get; set; }
        //船公司(MBL 船公司)
        public string Carrier { get; set; }
        //收貨地(HBL 收貨地(POR))
        public string PlaceOfReceipt { get; set; }
        //裝貨港(MBL 裝貨港(POL))
        public string PortOfLoading { get; set; }
        //ETD(MBL ETD)
        public string ETD { get; set; }
        public string ETA { get; set; }
        //中轉港(MBL 中轉港)
        public string PortOfTransshipment { get; set; }
        //中轉港ETA(MBL 中轉港ETA)
        public string TsETA { get; set; }
        //卸貨港(HBL 卸貨港)
        public string PortOfDischarge { get; set; }
        //卸貨港ETA(HBL 卸貨港ETA)
        public string PodETA { get; set; }
        //交貨地(HBL 交貨地(DEL))
        public string PlaceOfDelivery { get; set; }
        //交貨地ETA(HBL 交貨地ETA)
        public string DelETA { get; set; }
        //最終目的地(HBL 最終目的地)
        public string FinalDestination { get; set; }
        //最終目的地ETA(HBL 最終目的地ETA)
        public string FinalETA { get; set; }
        //運輸類型(HBL 運輸條款)
        public string MoveType { get; set; }
        //最早收櫃日(HBL 最早收櫃日)
        public string EarlyReturn { get; set; }
        //商品
        public string Commodity { get; set; }
        //集裝箱
        public string Container { get; set; }
        //重量	
        public string Weight { get; set; }
        //危險品	
        public bool Dangerous { get; set; }
        //材積	
        public string Measurement { get; set; }
        //信用狀	
        public bool LC { get; set; }
        //包裝種類	
        public string PKG { get; set; }
        public string Deliver_To { get; set; }
        //可堆放	
        public bool Stackable { get; set; }
        //貨物進倉地/重櫃交還地	
        public string CargoDeliveryLocation_1 { get; set; }
        public string CargoDeliveryLocation_2 { get; set; }
        //港口結關日(MBL港口結關日)
        public string Port_Cutoff_Date { get; set; }
        //鐵路結關日(MBL港口結關日)
        public string Rail_Cutoff_Date { get; set; }
        //倉儲結關日(HBL港口結關日)
        public string Warehouse_Cutoff_Date { get; set; }
        //文件結關日(MBL港口結關日)
        public string Doc_Cutoff_Date { get; set; }
        //提櫃地(HBL 提空櫃地點)
        public string EmptyPickUp { get; set; }
        //提貨地(HBL 卡車收貨地)
        public string CargoPickUp { get; set; }
        //卡車行(HBL 卡車行)
        public string Trucker { get; set; }
        //備註
        public string Remark { get; set; }

        //ReportLog
        public Guid ReportId { get; set; }
        public string BaseUrl { get; set; }
    }
}
