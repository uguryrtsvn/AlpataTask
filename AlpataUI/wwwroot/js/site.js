

const AddInventoryFile = (InventoryId) => $("#meetId").val(InventoryId);

const DeleteFile = (fileId) => { debugger; $("#fileId").val(fileId) };

const DirectMeetingDetail = (meetId) => window.location.href = '/Dashboard/EditMeeting?meetId=' + meetId;

const DeleteMeeting = (Id) => $("#meetId").val(Id);

$(document).ready(function () {
    $("#search").on('keyup', async () =>  {
        let q = $("#search").val()
        await $.ajax({
            method: "GET",
            url: "/Dashboard/GetFilteredMeetings",
            async: true,
            dataType: 'html',
            data: { q: q },
            contentType: 'application/json; charset=utf-8',
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response, textStatus, jqXHR) { 
                $("#table").html(response);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("Filtreleme sırasında hata oluştu");
            }
        });
    });
});
