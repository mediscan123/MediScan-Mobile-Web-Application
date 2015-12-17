using System.ComponentModel.DataAnnotations;

namespace Tecknow.MediScan.Entities
{
    public class RoleMaster : BaseEntity
    {
        [Key]
        public string RoleId { get; set; }

        public string UserId { get; set; }
        public string RoleName { get; set; }
        public virtual ApplicationUserMaster ApplicationUserMaster { get; set; }
    }
}