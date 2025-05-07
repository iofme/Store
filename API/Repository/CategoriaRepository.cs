using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interface;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class CategoriaRepository : Repository<Categoria> ,ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}