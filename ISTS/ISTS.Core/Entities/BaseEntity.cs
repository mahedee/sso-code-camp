namespace ISTS.Core.Entities
{
    public abstract class BaseEntity<TIdType> where TIdType : struct
    {
        public TIdType Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
