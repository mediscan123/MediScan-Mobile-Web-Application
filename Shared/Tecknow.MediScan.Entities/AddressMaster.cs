using System.ComponentModel.DataAnnotations;

namespace Tecknow.MediScan.Entities
{
    public class AddressMaster : BaseEntity
    {
        [Key]
        public string AddressId { get; set; }

        public string TenenatId { get; set; }
        public string UserId { get; set; }
        public string AddressCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string PostalIndexNumber { get; set; }
        public string LatLag { get; set; }
        public int LandlineNumber { get; set; }
        public int MobileNumber { get; set; }
        public string Email { get; set; }
        public virtual TenantMaster TenantMaster { get; set; }
        public virtual ApplicationUserMaster ApplicationUserMaster { get; set; }
    }
}