using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LearnApi.Entity
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; } = null!;

        [StringLength(50)]
        [Unicode(false)]
        public string Email { get; set; } = null!;

        [StringLength(50)]
        [Unicode(false)]
        public string? Phone { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Creditlimit { get; set; }

        public bool? IsActive { get; set; }

        public int? Taxcode { get; set; }
    }
}
