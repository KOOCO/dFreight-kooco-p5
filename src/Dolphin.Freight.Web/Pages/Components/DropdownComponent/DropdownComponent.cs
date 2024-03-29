﻿using Dolphin.Freight.Settings.SysCodes;
using Dolphin.Freight.Settinngs.SysCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Dolphin.Freight.Web.Pages.Components
{
    [Widget(ScriptFiles = new[] { "/Pages/Components/DropdownComponent/InitSelect.js" })]
    [ViewComponent(Name = "DropdownComponent")]
    public class DropdownComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke(string Name, List<SelectListItem> AspItems, string Selected = "", bool IsShowLabel = true, string fieldName = "", bool IsRequired = false, string Disabled= "", int SelectType = 0, string ShowFiledContentValue = "", string NotShowFiledContentValue = "", string cssClass = "form-control", bool isMultipleSelection = false, string ChangeEvent = "")
        {
            ComponentData componentData = new ComponentData() { IsRequired = IsRequired, IsShowLabel = IsShowLabel, Disabled = Disabled, Name = Name, AspItems = AspItems, Selected = Selected, FieldName = fieldName, SelectType = SelectType, ShowFiledContentValue = ShowFiledContentValue == null ? "" : ShowFiledContentValue, NotShowFiledContentValue = NotShowFiledContentValue == null ? "" : NotShowFiledContentValue, CssClass = cssClass, IsMultipleSelection = isMultipleSelection, ChangeEvent = ChangeEvent == null ? "" : ChangeEvent };

            return View("~/Pages/Components/DropdownComponent/Index.cshtml", componentData);
        }
    }
}