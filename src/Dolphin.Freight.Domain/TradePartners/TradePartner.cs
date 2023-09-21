using Dolphin.Freight.TradePartner;
using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dolphin.Freight.TradePartners
{
    public class TradePartner : FullAuditedAggregateRoot<Guid>
    {

        // Basic Setting
        public TPType TPType { get; set; }
        public string TPCode { get; private set; }
        public string TPName { get; private set; }
        public string TPNamePrint { get; set; }
        public string TPNameLocal { get; set; }

        public string TPLocalAddress { get; set; }
        public string CityCode { get; set; } // 城市代碼
        public string StateCode { get; set; }
        public string PostCode { get; set; }
        public string CountryCode { get; set; }

        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string TPPrintAddress { get; set; } // 貿易夥伴列印於報表之地址
        public string Remark { get; set; }

        // Other Setting
        public bool IsActive { get; set; }


        public string TPAliasName { get; set; }
        public string Ceo { get; set; }
        public string CorpNo { get; set; }
        public string IataCode { get; set; }
        public string IataPrefix { get; set; }
        public string ScacCode { get; set; }
        public string FirmsCode { get; set; } // 美國FIRMS編號
        public string CbsaCode { get; set; }
        public string DutyPaymentType { get; set; }
        public string OpenHours { get; set; }
        public BussinessStatusType BussinessStatusType { get; set; }
        public string CsCode { get; set; }
        public string SalesOfficeCode { get; set; }
        public string SalesCode { get; set; }
        public string SalesCodeOe { get; set; }
        public string SalesCodeOi { get; set; }
        public string SalesCodeAe { get; set; }
        public string SalesCodeAi { get; set; }
        public string SalesCodeCuOe { get; set; }
        public string SalesCodeCuAe { get; set; }
        public string SalesCodeCuOi { get; set; }
        public string SalesCodeCuAi { get; set; }


        // Account Setting
        public string AccountAddress { get; set; }
        public string TaxId { get; set; }
        public bool TrackPayment { get; set; }
        public PaymentType PaymentType { get; set; }
        public CreditTermType CreditTermType { get; set; }
        public int CreditTermDays { get; set; }
        public int CreditLimit { get; set; }
        //public string BillToAgentCode { get; set; }
        public bool BillToAgentCode { get; set; }
        public bool Clm { get; set; }
        public string AccountName { get; set; }
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string AccountCurrencyCode { get; set; }
        public string BankName2 { get; set; }
        public string AccountNo2 { get; set; }
        public string AccountCurrencyCode2 { get; set; }
        public string BankName3 { get; set; }
        public string AccountNo3 { get; set; }
        public string AccountCurrencyCode3 { get; set; }


        // ISF Setting
        public string IsfSubmissionName { get; set; }
        public ImporterCodeType ImporterCodeType { get; set; }
        public string ImporterNo { get; set; }


        // PopUp Tips
        public string PopUpTips { get; set; }
        public Guid AccountGroupId { get; set; }
        public Guid CreditLimitGroupId { get; set; }

        private TradePartner()
        { 
        
        }

        internal TradePartner(
            Guid id,
            Guid accountGroupId,
            Guid creditLimitGroupId,

            [NotNull] TPType tpTyp,
            [NotNull] string tpName,
            [NotNull] string tpNamePrint,

            BussinessStatusType bussinessStatusType,
            PaymentType paymentType,
            CreditTermType creditTermType,
            ImporterCodeType importerCodeType,
            int creditTermDays,
            int creditLimit,
            

            bool isActive = true,
            bool trackPayment = true,
            bool clm = false,
            bool billToAgentCode = false,

            [CanBeNull] string tpNameLocal = null,
            [CanBeNull] string tpLocalAddress = null,
            [CanBeNull] string cityCode = null,
            [CanBeNull] string stateCode = null,
            [CanBeNull] string postCode = null,
            [CanBeNull] string countryCode = null,
            [CanBeNull] string telephone = null,
            [CanBeNull] string fax = null,
            [CanBeNull] string website = null,
            [CanBeNull] string tpPrintAddress = null,
            [CanBeNull] string remark = null,
            [CanBeNull] string tpAliasName = null,
            [CanBeNull] string ceo = null,
            [CanBeNull] string corpNo = null,
            [CanBeNull] string iataCode = null,
            [CanBeNull] string iataPrefix = null,
            [CanBeNull] string scacCode = null,
            [CanBeNull] string firmsCode = null,
            [CanBeNull] string cbsaCode = null,
            [CanBeNull] string dutyPaymentType = null,
            [CanBeNull] string openHours = null,
            [CanBeNull] string csCode = null,
            [CanBeNull] string salesOfficeCode = null,
            [CanBeNull] string salesCode = null,
            [CanBeNull] string salesCodeOe = null,
            [CanBeNull] string salesCodeOi = null,
            [CanBeNull] string salesCodeAe = null,
            [CanBeNull] string salesCodeAi = null,
            [CanBeNull] string salesCodeCuOe = null,
            [CanBeNull] string salesCodeCuAe = null,
            [CanBeNull] string salesCodeCuOi = null,
            [CanBeNull] string salesCodeCuAi = null,
            [CanBeNull] string accountAddress = null,
            [CanBeNull] string taxId = null,
            //[CanBeNull] string billToAgentCode = null,
            [CanBeNull] string accountName = null,
            [CanBeNull] string bankName = null,
            [CanBeNull] string accountNo = null,
            [CanBeNull] string accountCurrencyCode = null,
            [CanBeNull] string bankName2 = null,
            [CanBeNull] string accountNo2 = null,
            [CanBeNull] string accountCurrencyCode2 = null,
            [CanBeNull] string bankName3 = null,
            [CanBeNull] string accountNo3 = null,
            [CanBeNull] string accountCurrencyCode3 = null,
            [CanBeNull] string isfSubmissionName = null,
            [CanBeNull] string importerNo = null,
            [CanBeNull] string popUpTips = null
            ) : base(id)
        {
            AccountGroupId = accountGroupId;
            CreditLimitGroupId = creditLimitGroupId;
            TPType = tpTyp;
            SetTPCode();// 呼叫產生編號
            SetTPName(tpName);
            TPNamePrint = tpNamePrint;

            BussinessStatusType = bussinessStatusType;
            PaymentType = paymentType;
            CreditTermType = creditTermType;
            ImporterCodeType = importerCodeType;
            CreditTermDays = creditTermDays;
            CreditLimit = creditLimit;
            
            IsActive = isActive;
            TrackPayment = trackPayment;
            Clm = clm;
            BillToAgentCode = billToAgentCode;

            TPNameLocal = tpNameLocal;
            TPLocalAddress = tpLocalAddress;
            CityCode = cityCode;
            StateCode = stateCode;
            PostCode = postCode;
            CountryCode = countryCode;
            Telephone = telephone;
            Fax = fax;
            Website = website;
            TPPrintAddress = tpPrintAddress;
            Remark = remark;
            TPAliasName = tpAliasName;
            Ceo = ceo;
            CorpNo = corpNo;
            IataCode = iataCode;
            IataPrefix = iataPrefix;
            ScacCode = scacCode;
            FirmsCode = firmsCode;
            CbsaCode = cbsaCode;
            DutyPaymentType = dutyPaymentType;
            OpenHours = openHours;
            CsCode = csCode;
            SalesOfficeCode = salesOfficeCode;
            SalesCode = salesCode;
            SalesCodeOe = salesCodeOe;
            SalesCodeOi = salesCodeOi;
            SalesCodeAe = salesCodeAe;
            SalesCodeAi = salesCodeAi;
            SalesCodeCuOe = salesCodeCuOe;
            SalesCodeCuAe = salesCodeCuAe;
            SalesCodeCuOi = salesCodeCuOi;
            SalesCodeCuAi = salesCodeCuAi;
            AccountAddress = accountAddress;
            TaxId = taxId;
            //BillToAgentCode = billToAgentCode;
            AccountName = accountName;
            BankName = bankName;
            AccountNo = accountNo;
            AccountCurrencyCode = accountCurrencyCode;
            BankName2 = bankName2;
            AccountNo2 = accountNo2;
            AccountCurrencyCode2 = accountCurrencyCode2;
            BankName3 = bankName3;
            AccountNo3 = accountNo3;
            AccountCurrencyCode3 = accountCurrencyCode3;
            IsfSubmissionName = isfSubmissionName;
            ImporterNo = importerNo;
            PopUpTips = popUpTips;
            
        }

        internal TradePartner ChangeTPName([NotNull] string tpName)
        {
            SetTPName(tpName);
            return this;
        }

        private string SetTPCode()
        {
            //TODO: Add the TPCode generation rule
            string today = DateTime.Now.ToString("yyyyMMddhhmmss");
            TPCode = "TP-" + today;
            return TPCode;
        }

        private void SetTPName([NotNull] string tpName)
        {
            TPName = Check.NotNullOrWhiteSpace(
                    tpName,
                    nameof(tpName),
                    maxLength: TradePartnerConsts.MaxTPNameLength
            );
        }

    }
}
