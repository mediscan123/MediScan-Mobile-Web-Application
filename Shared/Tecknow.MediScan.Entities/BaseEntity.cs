using System;

namespace Tecknow.MediScan.Entities
{
    public class BaseEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastUpdateBy { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }
    }
}