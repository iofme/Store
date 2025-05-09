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
        PagedList<Produto> GetProdutos(ProdutosParameters produtosParams);
        PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltrosPreco produtosFiltrosPreco);
        IEnumerable<Produto> GetProdutosPorCategoria(int id);

    }
}