namespace Learning.Domain
{
    public class DeletionEntity : ModificationEntity, IFullAuditedObject
    {
        public DeletionEntity(long id) : base(id)
        {
        }

        public bool IsDeleted { get; set; }
        public long? DeleterId { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
