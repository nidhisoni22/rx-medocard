using System.ComponentModel.DataAnnotations;

namespace RxMedoWeb.Models
{
    public class AdminUserListViewModel
    {
        public List<AdminUser> AdminUsers { get; set; } = new List<AdminUser>();
        
        public int TotalAdminUsers => AdminUsers.Count;
        
        public int ActiveAdminUsers => AdminUsers.Count(u => u.IsActive);
    }
}
