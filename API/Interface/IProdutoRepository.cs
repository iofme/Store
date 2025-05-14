using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Pagination;
using X.PagedList;

namespace API.Interface
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams);
        Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltrosPreco produtosFiltrosPreco);
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);

    }
}