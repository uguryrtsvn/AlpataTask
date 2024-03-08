using AlpataEntities.Entities.Concretes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataEntities.Dtos.InventoryDtos
{
    public class InventoryDto
    {
        public Guid Id { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        [NotMapped]
        public string fileExtantion
        {
            get
            {
                return FileName.Split('.').Last();
            }
        }
        public Guid MeetingId { get; set; } 
        public Meeting Meeting { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
