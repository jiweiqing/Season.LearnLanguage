using System.ComponentModel.DataAnnotations;

namespace Learning.Domain
{
    public class EntityBase<TKey>
    {
        public EntityBase(TKey id)
        {
            Id = id;
        }

        [Key]
        public TKey Id { get; set; }
    }
}
