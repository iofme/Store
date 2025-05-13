using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Pagination;

namespace API.Interface
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<PagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters);
        Task<PagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriaParams);
    }
} 