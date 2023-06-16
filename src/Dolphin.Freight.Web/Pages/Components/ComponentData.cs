using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.TradePartners;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections;
using System.Collections.Generic;

namespace Dolphin.Freight.Web.Pages.Components
{
    public class ComponentData
    {
        /// <summary>
        /// 元件名稱
        /// </summary>
        public string TagName { get; set; }
        /// <summary>
        /// 輸入的參數
        /// </summary>
        public string DefaultValue { get; set; }
        public string GetDefault() { return DefaultValue==null?"":DefaultValue; }

        /// <summary>
        /// 貿易夥伴用FiledContent輸入的參數
        /// </summary>
        public string ShowFiledContentValue { get; set; }
        public string GetShowFiledContent() { return string.IsNullOrEmpty(ShowFiledContentValue) ? " " : ShowFiledContentValue; }
        /// <summary>
        /// 欄位名稱
        /// </summary>
        public string FieldName { get; set; }
        public string GetFiledContentName() { 
            return FieldName.Replace("Id","")+ "Content";
        }
        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsRequired { get; set; }
        /// <summary>
        /// 是否顯示文字
        /// </summary>
        public bool IsShowLabel { get; set; }
        /// <summary>
        /// 0：一般的，1：貿易夥伴
        /// </summary>
        public int SelectType { get; set; }
        public string Disabled { get; set; }
        public string Selected { get; set; }
        public string Name { get; set; }
        public string Id { get { if (Name != null) return Name.Replace(".", "_"); return ""; } }
        public List<SelectListItem> AspItems { get; set; }

    }
}
