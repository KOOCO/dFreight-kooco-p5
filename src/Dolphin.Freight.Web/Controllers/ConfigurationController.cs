using Dolphin.Freight.ImportExport.Configuration;
using Dolphin.Freight.Web.ViewModels.Configuration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Users;

namespace Dolphin.Freight.Web.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class ConfigurationController : AbpController
    {
        public IGridPreferenceAppService _gridPreferenceAppService { get; set; }
        public ICurrentUser _currentUser { get; set; }
        public ConfigurationController(IGridPreferenceAppService gridPreferenceAppService, ICurrentUser currentUser)
        {
            _gridPreferenceAppService = gridPreferenceAppService;
            _currentUser = currentUser;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetConfig(string src = "")
        {

            var data = await _gridPreferenceAppService.GetGridPreferenceBySrcAsync(src, _currentUser.Id.GetValueOrDefault());

            ConfigurationViewModel viewModel = new();

            if (data != null)
            {
                var gridPreferences = JsonConvert.DeserializeObject<List<Preference>>(data.Preference);

                viewModel = new()
                {
                    Preference = gridPreferences,
                    PreferenceSrc = src,
                    Id = data.Id,
                    UserId = data.UserId
                };
            }

            return PartialView("Pages/Shared/_Configuration.cshtml", viewModel);
        }

        [Route("PostConfig")]
        [HttpPost]
        public async Task PostConfig(Dictionary<string, object> jsonObject)
        {
            if (jsonObject.Count > 0)
            {
                string id = (string)jsonObject["id"];
                string preferenceSrc = (string)jsonObject["preferenceSrc"];
                string userId = (string)jsonObject["userId"];

                // Access preference items
                List<Preference> preferences = new();
                for (int i = 0; i < jsonObject.Count - 3; i++)
                {
                    string preferenceKey = $"preference[{i}]";
                    if (jsonObject.ContainsKey(preferenceKey))
                    {
                        var preference = JsonConvert.DeserializeObject<Preference>(jsonObject[preferenceKey].ToString());

                        preferences.Add(preference);
                    }
                }

                var requestPayload = new CreateUpdateGridPreferenceDto()
                {
                    Id = Guid.Parse(id),
                    Preference = JsonConvert.SerializeObject(preferences),
                    PreferenceSrc = preferenceSrc,
                    UserId = Guid.Parse(userId)
                };

                await _gridPreferenceAppService.UpdateAsync(requestPayload.Id, requestPayload);
            }
        }

        [Route("GetJsonConfig")]
        [HttpGet]
        public async Task<JsonResult> GetJsonConfig(string src = "")
        {
            var data = await _gridPreferenceAppService.GetGridPreferenceBySrcAsync(src, _currentUser.Id.GetValueOrDefault());

            List<Preference> preferences = new();

            if (data != null)
            {
                preferences = JsonConvert.DeserializeObject<List<Preference>>(data.Preference);
            }

            return Json(preferences);
        }
    }
}
