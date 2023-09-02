using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp.Models
{
    public class Companies
    {
        public int Id { get; set; }
        [Column("Name", TypeName = "Varchar(100)")]
        [Required(ErrorMessage = "Field must be filled")]
        public string Name { get; set; }
    }
}
