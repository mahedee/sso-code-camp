using ISTS.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ISTS.Core.Entities
{
    public class RolePermissions : BaseEntity<long>
    {
        public string RoleId { get; set; }
        public int PermissionId { get; set; }
        public int ApiResourceId { get; set; }
        public ApplicationRole Role { get; set; }
        public Permission Permission { get; set; }
    }
}
