using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interface
{
    public interface IUnitOfWork
    {
         IProdutoRepository ProdutoRepository { get; }
         ICategoriaRepository CategoriaRepository { get; }
        Task CommitAsync();
    }
}