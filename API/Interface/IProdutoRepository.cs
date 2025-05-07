using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Interface
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosPorCategoria(int id);
    }
}