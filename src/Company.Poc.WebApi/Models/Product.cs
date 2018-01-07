using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Permissions;

namespace Company.Poc.WebApi.Models
{
    public class Product
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}