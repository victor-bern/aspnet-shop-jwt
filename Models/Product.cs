using System.ComponentModel.DataAnnotations;
namespace Shop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é Obrigátorio")]
        [MaxLength(30, ErrorMessage = "Este campo deve conter entre 3 e 30 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 30 caracteres")]
        public string Title { get; set; }

        [MaxLength(1024, ErrorMessage = "Este campo deve ter no máx 1024 caracteres")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Este campo é obrigátorio")]
        [Range(1, int.MaxValue, ErrorMessage = "O preço deve ser maior que 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Este campo é obrigátorio")]
        [Range(1, int.MaxValue, ErrorMessage = "O preço deve ser maior que 0")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}