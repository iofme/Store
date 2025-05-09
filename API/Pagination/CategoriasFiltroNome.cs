using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Pagination
{
    public class CategoriasFiltroNome : QueryStringParameters
    {
        public string? Nome { get; set; }
    }
}