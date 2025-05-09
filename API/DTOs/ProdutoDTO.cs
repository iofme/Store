using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API.Models
    {
        public class ProdutoDTO
        {
            public int ProdutoId { get; set; }

            [Required]
            [StringLength(20, ErrorMessage = "O nome deve ter entre 5 e 20 caracteres", MinimumLength = 5)]
            public string? Nome { get; set; }

            [Required]
            [StringLength(300)]
            public string? Descricao { get; set; }

            [Required]
            public decimal Preco { get; set; }

            [Required]
            [StringLength(300)]
            public string? ImagemUrl { get; set; }

            public float Estoque { get; set; }

            public DateTime DataCadasto { get; set; }

            public int CategoriaId { get; set; }
            [JsonIgnore]
            public Categoria? Categoria { get; set; }
        }
    }