using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interface;

namespace API.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IProdutoRepository _produtoRepo;

        private ICategoriaRepository _categoriaRepo;

        public AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProdutoRepository ProdutoRepository{
            get{
                return _produtoRepo = _produtoRepo ?? new ProdutoRepository(_context);
            }
        }
        public ICategoriaRepository CategoriaRepository{
            get{
                return _categoriaRepo = _categoriaRepo ?? new CategoriaRepository(_context);
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}