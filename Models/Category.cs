using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{

    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é Obrigátorio")]
        [MaxLength(30, ErrorMessage = "Este campo deve conter entre 3 e 30 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 30 caracteres")]
        public string Title { get; set; }
    }
}