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
        public IActionResult CreateMeeting() => View();
        public async Task<IActionResult> EditMeeting(string meetId)
        {
            var result = await _alpataClient.GetNoRoot<MeetingDto>("/Meeting/EditMeeting?meetId=" + meetId); 
            if (result.Success) return View(result.Data);
            
            _toastNotification.AddErrorToastMessage(result.Message);
            return RedirectToAction("Index"); 
        }
        [HttpPost]
        public async Task<IActionResult> EditMeeting(MeetingDto dto)
        { 
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddFileToMeeting(IFormFile file, string meetId)
        {
            return Ok();
        }
        #region Meeting 
        [HttpPost]
        public async Task<IActionResult> CreateMeeting(MeetingDto dto)
        {
            var result = await _alpataClient.Add(dto, "Meeting/CreateMeeting");
            if (!result.Success)
            {
                _toastNotification.AddErrorToastMessage("Toplantı oluşturulurken hata oluştu");
                return View(dto);
            }

            _toastNotification.AddInfoToastMessage("Toplantı oluşturuldu");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> AddUserToMeeting(string meetId)
        {
            var meet = await _alpataClient.GetNoRoot<bool>("Meeting/AddUserToMeeting?meetId=" + meetId);
            if (!meet.Success)
            {
                _toastNotification.AddErrorToastMessage("Toplantıya katılım sırasında hata oluştu.");
                return RedirectToAction("Index");
            }
            _toastNotification.AddInfoToastMessage("Toplantı bilgileri mailinize gönderilmiştir.");
            return RedirectToAction("Index");

        }


        [HttpGet]
        public async Task<IActionResult> DeleteUserToMeeting(string meetId)
        {
            var meet = await _alpataClient.GetNoRoot<bool>("Meeting/DeleteUserToMeeting?meetId=" + meetId);
            if (!meet.Success)
            {
                _toastNotification.AddErrorToastMessage("Toplantıya iptali sırasında hata oluştu.");
                return RedirectToAction("Index");
            }
            _toastNotification.AddInfoToastMessage("Toplantı kaydınız silinmiştir.");
            return RedirectToAction("Index");

        }
        #endregion

    }
}
