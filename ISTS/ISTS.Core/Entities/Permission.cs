namespace ISTS.Core.Entities
{
    public class Permission : BaseEntity<int>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int ApiResourceId { get; set; }
    }
}
