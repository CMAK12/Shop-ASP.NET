using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp.Models
{
    public class Products
    {
        public int Id { get; set; }
        [Column("ProductName", TypeName = "Varchar(200)")]
        [Required (ErrorMessage = "Field must be filled")]
        public string ProductName { get; set; }
        public string? Description { get; set; }
        [Column("Company", TypeName = "Varchar(100)")]
        [Required(ErrorMessage = "Field must be filled")]
        public string Company { get; set; }
        [Required (ErrorMessage = "Field must be filled")]
        public float Price { get; set; }
        public string? Photo { get; set; }
        public int Stock { get; set; }
    }
}
