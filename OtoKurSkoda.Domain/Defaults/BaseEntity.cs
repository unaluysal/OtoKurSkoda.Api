using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OtoKurSkoda.Domain.Defaults
{
    public abstract class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column(Order = 1)]
        public Guid Id { get; set; }

        [Column(TypeName = "timestamp", Order = 2)]
        public DateTime CreateTime { get; set; }

        [Column(Order = 3)]
        public Guid CreateUserId { get; set; }

        [Column(TypeName = "timestamp", Order = 4)]
        public DateTime? UpdateTime { get; set; }
        [Column(Order = 5)]
        public Guid? UpdateUserId { get; set; }
        [Column(Order = 6)]
        public Guid TenatId { get; set; }

        [Column(Order = 7)]
        public bool Status { get; set; }

    }
}
