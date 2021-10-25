﻿using CatalogoApi.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoApi.Models
{
    [Table("Produtos")]
    public class Produto : IValidatableObject
    {
        [Key]
        public int ProdutoId { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength (50, ErrorMessage = "O nome deve ter entre 5 e 50 caracteres", MinimumLength =5)]
        //[PrimeiraLetraMaiuscula]
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(this.Nome))
            {
                var primeiraLetra = this.Nome[0].ToString();
                if(primeiraLetra != primeiraLetra.ToUpper())
                {
                    yield return new ValidationResult("A primeira letra deve ser maiúscula", new[] { nameof(this.Nome) });
                }
                if(this.Estoque <= 0)
                {
                    yield return new ValidationResult("O estoque deve ser maior que zero", new[] { nameof(this.Estoque) });
                }
            }
        }
    }
}
