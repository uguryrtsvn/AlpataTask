using AlpataEntities.Entities.Concretes;

namespace AlpataUI.Models
{
    public class FileDto
    {
        public Guid MeetingId { get; set; }
        public string InventoryId { get; set; }
        public Meeting Meeting { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
