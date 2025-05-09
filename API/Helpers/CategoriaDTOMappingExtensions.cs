using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;

namespace API.Helpers
{
    public static class CategoriaDTOMappingExtensions
    {
        public static IEnumerable<CategoriaDTO> ToCategoriaDTOList(this IEnumerable<Categoria> categorias)
        {
            if(categorias is null || !categorias.Any())
            {
                return new List<CategoriaDTO>();
            }

            return categorias.Select(categoria => new CategoriaDTO
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl,
            }).ToList();
        }
    }
}