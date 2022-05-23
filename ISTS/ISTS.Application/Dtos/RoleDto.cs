namespace ISTS.Application.Dtos
{
    public class RoleDto
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public string? RoleDetails { get; set; }
        public bool IsDeletable { get; set; }
    }
}
