
using AlpataEntities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataEntities.Entities.Concretes
{
    public class Inventory : BaseEntity
    {
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        [NotMapped]
        public string fileExtantion
        {
            get { return FileName.Split('.').Last(); }
        }
        [NotMapped]
        public string fileNameWithZip
        {
            get { return FileName.Split(".").First() + ".zip"; }
        }
        public Guid MeetingId { get; set; }
        public Meeting Meeting { get; set; }
    }
}
