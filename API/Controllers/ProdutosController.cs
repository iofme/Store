using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Models;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {

        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context){
            _context = context;
        }
        [HttpGet]
         public ActionResult<IEnumerable<Produto>> Get()
         {
            var produtos = _context.Produtos.ToList();

            if(produtos is null)
            {
                return NotFound("Produtos não encontrados");
            }
            return produtos;
         }
        [HttpGet("{id:int}")]
         public ActionResult<Produto> Get(int id)
         {
            var produtos = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if(produtos is null)
            {
                return NotFound("Produto não encontrado");
            }
            return produtos;
         }

         [HttpPost]
         public ActionResult Post(Produto produto)
         {
            if(produto is null)
                return BadRequest();
            
            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("OvjetProduto", new {id = produto.ProdutoId}, produto);
         }

         [HttpPut("{id:int}")]
         public ActionResult Put(int id, Produto produto)
         {
            if(id != produto.ProdutoId)
            {
                return BadRequest();
            }

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
         }

         [HttpDelete("{id:int}")]
         public ActionResult Delete(int id)
         {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if(produto is null)
                return NotFound("Produto não localizado");

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok();
         }
    }
} 