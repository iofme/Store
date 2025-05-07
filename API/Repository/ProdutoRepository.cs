using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interface;
using API.Models;

namespace API.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public Produto Create(Produto produto)
        {
            if(produto == null)
            {
                throw new ArgumentNullException("Produto é null");
            }

            _context.Produtos.Add(produto);
            _context.SaveChanges();
            return produto;
        }

        public bool Delete(int id)
        {
            var produto = _context.Produtos.Find(id);

            if(produto != null)
            {
                _context.Produtos.Remove(produto);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public Produto GetProduto(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if (produto is null)
            {
                throw new InvalidOperationException("Produto é null");
            }

            return produto;
        }

        public IQueryable<Produto> GetProdutos()
        {
            return _context.Produtos;
        }

        public bool Update(Produto produto)
        {
            if (produto == null)
            {
                throw new InvalidOperationException("Produto é null");
            }
                if(_context.Produtos.Any(p => p.ProdutoId == produto.ProdutoId))
                {
                    _context.Produtos.Update(produto);
                    _context.SaveChanges();
                    return true;
                }
                return false;
        }
    }
}