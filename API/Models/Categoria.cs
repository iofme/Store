using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Categoria
    {
        public Categoria(){
            Produtos = new Collection<Produto>();
        }

        [Key]
        public int CategoriaId { get; set; }

        [Required]
        [StringLength(80)]
        public string? Nome { get; set; }
        
        [Required]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }


        public ICollection<Produto>? Produtos { get; set; }
    }
}