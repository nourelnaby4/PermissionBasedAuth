namespace PermissionBasedAuth.Constants
{
    public static class Permission
    {
        public static List<string> GeneratePermissionList(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.View",
                $"Permissions.{module}.Create",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };
        }
    }
}
