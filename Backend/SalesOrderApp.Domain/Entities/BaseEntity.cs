namespace SalesOrderApp.Domain.Entities
{
    /// <summary>
    /// Common audit fields shared by every persisted entity.
    /// </summary>
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
    }
}
