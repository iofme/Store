using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Pagination;

namespace API.Interface
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<PagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams);
        Task<PagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltrosPreco produtosFiltrosPreco);
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);

    }
}