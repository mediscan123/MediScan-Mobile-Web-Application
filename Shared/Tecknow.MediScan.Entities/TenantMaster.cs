using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tecknow.MediScan.Entities
{
    public class TenantMaster : BaseEntity
    {
        [Key]
        public string TenantID { get; set; }

        public string TenantName { get; set; }
        public string LegalName { get; set; }
        public string RegistrationNumber { get; set; }
        public byte[] Logo { get; set; }
        public virtual ICollection<ApplicationUserMaster> Users { get; set; }
        public virtual ICollection<AddressMaster> Address { get; set; }
    }
}