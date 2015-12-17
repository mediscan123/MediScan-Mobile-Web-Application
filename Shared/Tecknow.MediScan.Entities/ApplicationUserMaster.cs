using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tecknow.MediScan.Entities
{
    public class ApplicationUserMaster : BaseEntity
    {
        [Key]
        public string UserId { get; set; }

        public string TenenatId { get; set; }
        public string LoginId { get; set; }
        public string LoginPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime SubscriptionValidFrom { get; set; }
        public DateTime SubscriptionValidTo { get; set; }
        public string Status { get; set; }
        public virtual TenantMaster TenantMaster { get; set; }
        public virtual ICollection<AddressMaster> Address { get; set; }
        public virtual ICollection<RoleMaster> Rolls { get; set; }
    }
}