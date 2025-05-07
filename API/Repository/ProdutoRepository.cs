using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interface;
using API.Models;

namespace API.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {

        }

        public IEnumerable<Produto> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(c => c.CategoriaId == id);
        }
    }
}