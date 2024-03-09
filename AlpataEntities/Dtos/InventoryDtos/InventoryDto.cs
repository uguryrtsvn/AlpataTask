using AlpataEntities.Entities.Concretes;
namespace AlpataEntities.Dtos.InventoryDtos
{
    public class InventoryDto
    {
        public InventoryDto()
        {
            CreatedTime = DateTime.Now;
        }
        public Guid Id { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public string fileExtantion
        {
            get
            {
                return FileName.Split('.').Last();
            }
        }
        public string fileNameWithZip
        {
            get { return FileName.Split(".").First() + ".zip"; }
        }
        public Guid MeetingId { get; set; }
        public Meeting? Meeting { get; set; } 
    }
}
