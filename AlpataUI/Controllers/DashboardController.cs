using AlpataBLL.BaseResult.Concretes;
using AlpataEntities.Dtos.AuthDtos;
using AlpataEntities.Dtos.InventoryDtos;
using AlpataEntities.Dtos.MeetingDtos;
using AlpataEntities.Entities.Concretes;
using AlpataUI.Helpers.ClientHelper;
using AlpataUI.Helpers.FileManagerHelper;
using AlpataUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NToastNotify;
using System.Collections.Generic;
using System.Security.Claims;

namespace AlpataUI.Controllers
{
    [Authorize]
    [EnableRateLimiting("fixed")]
    public class DashboardController : Controller
    {
        readonly IToastNotification _toastNotification;
        readonly IAlpataClient _alpataClient;
        private readonly IFileManager _fileUploadService;

        public DashboardController(IToastNotification toastNotification, IAlpataClient alpataClient, IFileManager fileUploadService)
        {
            _toastNotification = toastNotification;
            _alpataClient = alpataClient;
            _fileUploadService = fileUploadService;
        }
        public async Task<IActionResult> Index()
        {
            var list = await _alpataClient.GetNoRoot<List<MeetingDto>>("Meeting/GetAll");
            return View(list.Data);
        }

        #region Meeting 
        [HttpPost]
        public async Task<IActionResult> AddFileToMeeting(IFormFile file, string meetId)
        {
            var result = await _fileUploadService.SaveFileAsync(new FileDto() { FormFile = file, MeetingId = Guid.Parse(meetId) }, FileStorageLocation.Database);
            if (result.Success)
            {
                _toastNotification.AddInfoToastMessage(result.Message);
                return RedirectToAction("Index");
            }
            _toastNotification.AddErrorToastMessage(result.Message);
            return RedirectToAction("Index");
        }

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
        public async Task<IActionResult> DeleteUserFromMeeting(string meetId)
        {
            var meet = await _alpataClient.GetNoRoot<bool>("Meeting/DeleteUserFromMeeting?meetId=" + meetId);
            if (!meet.Success)
            {
                _toastNotification.AddErrorToastMessage("Toplantıya iptali sırasında hata oluştu.");
                return RedirectToAction("Index");
            }
            _toastNotification.AddInfoToastMessage("Toplantı kaydınız silinmiştir.");
            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> DeleteInventory(DeleteInventoryVm vm)
        {
            var meet = await _alpataClient.GetNoRoot<bool>("Inventory/DeleteInventory?Id=" + vm.Id);
            if (!meet.Success)
            {
                _toastNotification.AddErrorToastMessage("Dosya silinirken hata oluştu");
                return RedirectToAction("Index");
            }
            _toastNotification.AddInfoToastMessage("Dosya silinmiştir.");
            return RedirectToAction("EditMeeting", new { meetId = vm.meetId });

        }
        public IActionResult CreateMeeting() => View();

        public async Task<IActionResult> OrganizedMeetings()
        {
            var list = await _alpataClient.GetNoRoot<List<MeetingDto>>("Meeting/GetOrganizedMeetings?Id="+ User.Claims.First(z => z.Type == "Id").Value);
            return View(list.Data);
        }

        [HttpGet]
        public async Task<IActionResult> EditMeeting(string meetId)
        {
            var result = await _alpataClient.GetNoRoot<MeetingDto>("Meeting/GetMeetingWithId?meetId=" + meetId);
            if (result.Success) return View(result.Data);

            _toastNotification.AddErrorToastMessage(result.Message);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredMeetings(string q)
        {
            DataResult<List<MeetingDto>> list = new();
            if (string.IsNullOrWhiteSpace(q))
            {
                list = await _alpataClient.GetNoRoot<List<MeetingDto>>("Meeting/GetAll"); 
            }
            else
            {
                list = await _alpataClient.GetNoRoot<List<MeetingDto>>("Meeting/GetFilteredMeetings?q="+q); 
            }
            return PartialView("~/Views/Shared/_Partials/_MeetListPartial.cshtml", list.Data);
        }

        [HttpPost]
        public async Task<IActionResult> EditMeeting(MeetingDto dto)
        {
            var result = await _alpataClient.PostAsync<MeetingDto, bool>(dto,"Meeting/EditMeeting");
            if (result.Success)
            {
                _toastNotification.AddInfoToastMessage("Toplantı güncellendi.");
                return RedirectToAction("EditMeeting",new {meetId = dto.Id});
            }
            _toastNotification.AddInfoToastMessage(result.Message);
            return RedirectToAction("EditMeeting", new { meetId = dto.Id });
        }

        [HttpGet]
        public async Task<IActionResult> MeetingDetail(string meetId)
        {
            var result = await _alpataClient.GetNoRoot<MeetingDto>("Meeting/GetMeetingWithId?meetId=" + meetId);
            if (result.Success) return View(result.Data);

            _toastNotification.AddErrorToastMessage(result.Message);
            return RedirectToAction("Index");
        }
        #endregion
        [HttpGet]
        public async Task<IActionResult> DownloadFile(string InvId)
        {
            var result = await _alpataClient.GetNoRoot<Inventory>("Inventory/GetInventory?Id=" + InvId);
            if (result.Success)
            {
                using (var memoryStream = new MemoryStream(result.Data?.FileData))
                {
                    var byteArray = memoryStream.ToArray();
                    return File(byteArray, "application/octet-stream", result.Data?.fileNameWithZip);
                }
            }
            _toastNotification.AddErrorToastMessage(result.Message);
            return RedirectToAction("Index", "Dashboard"); 
        }
        [HttpPost]
        public async Task<IActionResult> DeleteMeeting(string Id)
        {
            var result = await _alpataClient.GetNoRoot<bool>("Meeting/DeleteMeeting?Id=" + Id);
            if (result.Success)
            {
                _toastNotification.AddInfoToastMessage("Toplantı ve ilgili veriler silinmiştir.");
                return RedirectToAction("OrganizedMeetings");
            }

            _toastNotification.AddErrorToastMessage(result.Message);
            return RedirectToAction("OrganizedMeetings");
        }

    }
}
