using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interface;
using API.Models;
using API.Pagination;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class CategoriaRepository : Repository<Categoria> ,ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
            
        }

        public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters)
        {
            var categorias = GetAll().OrderBy(p => p.CategoriaId).AsQueryable();

            var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categorias, categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return categoriasOrdenadas;
        }

        public PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriaParams)
        {
            var categorias = GetAll().AsQueryable();

            if(!string.IsNullOrEmpty(categoriaParams.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriaParams.Nome));
            }

            var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias, categoriaParams.PageNumber, categoriaParams.PageSize);

            return categoriasFiltradas; 
        }
    }
}