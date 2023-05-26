using Dolphin.Freight.Web.ViewModels.BookingConfirmation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Wkhtmltopdf.NetCore;

namespace Dolphin.Freight.Web.Pages.Reports
{
    public class BookingConfirmationModel : PageModel
    {
        public class InfoViewModel
        {
            //����
            public string Office { get; set; }
            //�ާ@�̦a�}
            public string Address { get; set; }
            //�ާ@�̹q��
            public string Tel { get; set; }
            //�ާ@�̶ǯu
            public string Fax { get; set; }
            //�ާ@��Email
            public string Email { get; set; }
            //�ާ@�̩m
            public string FirstName { get; set; }
            //�ާ@�̦W
            public string LastName { get; set; }
            //��U���
            public string Date { get; set; }
            //��U���(�t�ɶ�)
            public string DateTime { get; set; }
            //������O
            public string ReportTitleChoice { get; set; }
            //��ڰU�B�H/���f�H or ���a/���~�N�z
            public string TradePartnerLayoutChoice { get; set; }
            //��q�q�����X 
            public string CarrierBookingNo { get; set; }
            //��ڰU�B�H
            public string ActualShipper { get; set; }
            //���f�H
            public string Consignee { get; set; }
            //���a�N�z(���� + MBL C/O)
            public string Shipping { get; set; }
            //���~�N�z(MBL ���~�N�z)
            public string OverseaAgent { get; set; }
            //HBL���X
            public string HblNo { get; set; }
            //�ڥq�q�����X(S/O���X)
            public string OutBookingNo { get; set; }
            //�q�����
            public string BookingDate { get; set; }
            //�X�f�ѦҸ��X(�Ȥ�Ѧҽs��)
            public string ExportRefNo { get; set; }
            //PO���X
            public string PoNo { get; set; }
            //ITN���X
            public string ItnNo { get; set; }
            //�N�z(HB/L�N�z-Print Name)
            public string Agent { get; set; }
            //�q���H(�q����-Print Name)
            public string Notify { get; set; }
            //��W�P�覸(MBL ��W + MBL �覸)
            public string Vessel_Voyage { get; set; }
            //��q(MBL ��q)
            public string Carrier { get; set; }
            //���f�a(HBL ���f�a(POR))
            public string PlaceOfReceipt { get; set; }
            //�˳f��(MBL �˳f��(POL))
            public string PortOfLoading { get; set; }
            //ETD(MBL ETD)
            public string ETD { get; set; }
            //�����(MBL �����)
            public string PortOfTransshipment { get; set; }
            //�����ETA(MBL �����ETA)
            public string TsETA { get; set; }
            //���f��(HBL ���f��)
            public string PortOfDischarge { get; set; }
            //���f��ETA(HBL ���f��ETA)
            public string PodETA { get; set; }
            //��f�a(HBL ��f�a(DEL))
            public string PlaceOfDelivery { get; set; }
            //��f�aETA(HBL ��f�aETA)
            public string DelETA { get; set; }
            //�̲ץت��a(HBL �̲ץت��a)
            public string FinalDestination { get; set; }
            //�̲ץت��aETA(HBL �̲ץت��aETA)
            public string FinalETA { get; set; }
            //�B������(HBL �B�����)
            public string MoveType { get; set; }
            //�̦����d��(HBL �̦����d��)
            public string EarlyReturn { get; set; }
            //�ӫ~
            public string Commodity { get; set; }
            //���˽c
            public string Container { get; set; }
            //���q	
            public string Weight { get; set; }
            //�M�I�~	
            public bool Dangerous { get; set; }
            //���n	
            public string Measurement { get; set; }
            //�H�Ϊ�	
            public bool LC { get; set; }
            //�]�˺���	
            public string PKG { get; set; }
            //�i���	
            public bool Stackable { get; set; }
            //�f���i�ܦa/���d���٦a	
            public string CargoDeliveryLocation_1 { get; set; }
            public string CargoDeliveryLocation_2 { get; set; }
            //��f������(MBL��f������)
            public string Port_Cutoff_Date { get; set; }
            //�K��������(MBL��f������)
            public string Rail_Cutoff_Date { get; set; }
            //���x������(HBL��f������)
            public string Warehouse_Cutoff_Date { get; set; }
            //�������(MBL��f������)
            public string Doc_Cutoff_Date { get; set; }
            //���d�a(HBL �����d�a�I)
            public string EmptyPickUp { get; set; }
            //���f�a(HBL �d�����f�a)
            public string CargoPickUp { get; set; }
            //�d����(HBL �d����)
            public string Trucker { get; set; }
            //�Ƶ�
            public string Remark { get; set; }
        }
        public InfoViewModel InfoModel { get; set; }
        private readonly IGeneratePdf _generatePdf;
        public BookingConfirmationModel(IGeneratePdf generatePdf)
        {
            _generatePdf = generatePdf;
        }
        public void OnGet()
        {
            if (TempData["PrintDataBC"] != null)
            {
                InfoModel = JsonConvert.DeserializeObject<InfoViewModel>(TempData["PrintDataBC"].ToString());
            }
            else
            {
                InfoModel = new InfoViewModel();
            }

            TempData["PrintData"] = JsonConvert.SerializeObject(InfoModel);

            //Test Data
            #region
            //InfoModel.Office = "Dolphin Logistics Taipei Office";
            //InfoModel.Address = "";
            //InfoModel.Tel = "+886-2-2545-9900#8671";
            //InfoModel.Fax = "";
            //InfoModel.Email = "it@dolphin-gp.com";
            //InfoModel.FirstName = "�U��";
            //InfoModel.LastName = "��T��";
            //InfoModel.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //InfoModel.Date = DateTime.Now.ToString("yyyy-MM-dd");
            ////InfoModel.ReportTitleChoice = "BC";
            ////InfoModel.TradePartnerLayoutChoice = "default";
            //InfoModel.CarrierBookingNo = "A1234";
            //InfoModel.ActualShipper = "3891 DELTA PORT 809\r\nTSI DELTAPORT\r\n2 ROBERTS BANK\r\nDELTA, BC, CANADA";
            //InfoModel.Consignee = "A CUSTOMS BROKERAGE, INC.\r\n5400 NW 84TH AVE\r\nDORAL, FL 33166, UNITED STATES";
            //InfoModel.Shipping = "DOLPHIN LOGISTICS TAIPEI OFFICE\r\nC/O AER LINGUS LIMITED P.L.C.";
            //InfoModel.OverseaAgent = "AER LINGUS\r\n6555 W. IMPERIAL HWY\r\nLOS ANGELES, CA 90045, UNITED STATES";
            //InfoModel.HblNo = "OH-23010006";
            //InfoModel.OutBookingNo = "DDD";
            //InfoModel.BookingDate = "";
            //InfoModel.ExportRefNo = "GGG";
            //InfoModel.PoNo = "";
            //InfoModel.ItnNo = "";
            //InfoModel.Agent = "AER LINGUS";
            //InfoModel.Notify = "123552133441";
            //InfoModel.Vessel_Voyage = ". BELO GOA 444";
            //InfoModel.Carrier = "3891 DELTA PORT 809";
            //InfoModel.PlaceOfReceipt = "HERMITAGE, AR (UNITED STATES)";
            //InfoModel.PortOfLoading = "BALLSTON LAKE, NY (UNITED STATES)";
            //InfoModel.ETD = "01-12-2023";
            //InfoModel.PortOfTransshipment = "ADAIR, OK (UNITED STATES)";
            //InfoModel.TsETA = "03-14-2023";
            //InfoModel.PortOfDischarge = "SOUTH OGDEN, UT (UNITED STATES)";
            //InfoModel.PodETA = "02-21-2023";
            //InfoModel.PlaceOfDelivery = "AMBIA, IN (UNITED STATES)";
            //InfoModel.DelETA = "03-30-2023";
            //InfoModel.FinalDestination = "ALDAN, PA (UNITED STATES)";
            //InfoModel.FinalETA = "03-31-2023";
            //InfoModel.MoveType = "BT/BT";
            //InfoModel.EarlyReturn = "03-29-2023 00:00";
            //InfoModel.Commodity = "����";
            //InfoModel.Container = "����";
            //InfoModel.Weight = "1";
            //InfoModel.Dangerous = false;
            //InfoModel.Measurement = "1";
            //InfoModel.LC = true;
            //InfoModel.PKG = "1";
            //InfoModel.Stackable = true;
            //InfoModel.CargoDeliveryLocation_1 = "AERO TRANSCOLOMBIANA DE CARGA";
            //InfoModel.CargoDeliveryLocation_2 = "AERO TRANSCOLOMBIANA DE CARGA";
            //InfoModel.Port_Cutoff_Date = "03-12-2023 00:00";
            //InfoModel.Rail_Cutoff_Date = "03-10-2023 00:00";
            //InfoModel.Warehouse_Cutoff_Date = "03-29-2023 00:00";
            //InfoModel.Doc_Cutoff_Date = "03-11-2023 00:00";
            //InfoModel.EmptyPickUp = "AERO EJECUTIVOS C.A.";
            //InfoModel.CargoPickUp = "AEROCHAGO AIRLINES S.A.";
            //InfoModel.Trucker = "ADDICTED DEALS, LLC";
            //InfoModel.Remark = "";
            #endregion
        }

        public async Task<IActionResult> OnPostAsync(InfoViewModel InfoModel)
        {
            string Input = JsonConvert.SerializeObject(InfoModel);
            var OutModel = new BookingConfirmationIndexViewModel();
            OutModel = JsonConvert.DeserializeObject<BookingConfirmationIndexViewModel>(Input);

            return await _generatePdf.GetPdf("Views/BookingConfirmation/BookingConfirmation.cshtml", OutModel);
        }

    }
}
