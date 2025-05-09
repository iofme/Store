using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Pagination
{
    public class ProdutosFiltrosPreco : QueryStringParameters
    {
        public decimal? Preco { get; set; }
        public string? PrecoCriterio { get; set; }
    }
}