using AlpataEntities.Dtos.AuthDtos;
using AlpataEntities.Dtos.MeetingDtos;
using AlpataUI.Helpers.ClientHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Security.Claims;

namespace AlpataUI.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        readonly IToastNotification _toastNotification;
        readonly IAlpataClient _alpataClient;
        public DashboardController(IToastNotification toastNotification, IAlpataClient alpataClient)
        {
            _toastNotification = toastNotification;
            _alpataClient = alpataClient;
        }
        public async Task<IActionResult> Index()
        {
            var list = await _alpataClient.GetNoRoot<List<MeetingDto>>("Meeting/GetAll");
            return View(list.Data);
        }
        public IActionResult CreateMeeting() =>View();
    }
}
