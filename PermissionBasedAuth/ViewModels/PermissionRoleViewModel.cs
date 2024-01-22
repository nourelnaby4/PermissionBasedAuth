namespace PermissionBasedAuth.ViewModels
{
    public class PermissionRoleViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<SelectedViewModel> Claims { get; set; }=new List<SelectedViewModel>();
    }
}
