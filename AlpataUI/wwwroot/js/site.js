

const AddInventoryFile = (InventoryId) => $("#meetId").val(InventoryId);

const DeleteFile = (fileId) => { debugger; $("#fileId").val(fileId) };

const DirectMeetingDetail = (meetId) => window.location.href = '/Dashboard/EditMeeting?meetId=' + meetId;
const DeleteMeeting = (Id) => $("#meetId").val(Id);