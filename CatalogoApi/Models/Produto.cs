using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoApi.Models
{
    [Table("Produtos")]
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength (50, ErrorMessage = "O nome deve ter entre 5 e 50 caracteres", MinimumLength =5)]
        public string Nome { get; set; }
        [Required]
        [StringLength(100, ErrorMessage ="A descrição deve ter no máximo {1} caracteres")]
        public string Descricao { get; set; }
        [Required]
        [Range(1, 10000, ErrorMessage ="O preço deve estar entre {1} e {2}")]
        public decimal Preco { get; set; }
        [Required]
        [StringLength(500, MinimumLength =10)]
        public string ImagemUrl { get; set; }
        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }
        public Categoria Categoria { get; set; }
        public int CategoriaId { get; set; }
    }
}
