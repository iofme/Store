using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interface;
using API.Models;
using API.Pagination;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace API.Repository
{
    public class CategoriaRepository : Repository<Categoria> ,ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
            
        }

        public async Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters)
        {
            var categorias = await GetAllAsync();

            var categoriasOrdenadas = categorias.OrderBy(p => p.CategoriaId).AsQueryable();

            //var resultado = PagedList<Categoria>.ToPagedList(categoriasOrdenadas, categoriasParameters.PageNumber, categoriasParameters.PageSize);

            var resultado = await categoriasOrdenadas.ToPagedListAsync(categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return resultado;
        }

        public async Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriaParams)
        {
            var categorias = await GetAllAsync();

            if(!string.IsNullOrEmpty(categoriaParams.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriaParams.Nome));
            }

            //var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias.AsQueryable(), categoriaParams.PageNumber, categoriaParams.PageSize);
            
            var categoriasFiltradas = await categorias.ToPagedListAsync(categoriaParams.PageNumber, categoriaParams.PageSize);

            return categoriasFiltradas; 
        }
    }
}