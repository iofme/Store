using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interface;
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
        private readonly IUnitOfWork _uof;

        public ProdutosController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        [HttpGet("produtos/{id}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);

            if(produtos is null)
            {
                return NotFound();
            }

            return Ok(produtos);
        }

        [HttpGet]
         public ActionResult<IEnumerable<Produto>> Get()
         {
            var produtos = _uof.ProdutoRepository.GetAll();

            if(produtos is null)
            {
                return NotFound("Produtos não encontrados");
            }
            return Ok(produtos);
         }
        [HttpGet("{id:int}")]
         public ActionResult<Produto> Get(int id)
         {
            var produtos = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if(produtos is null)
            {
                return NotFound("Produto não encontrado");
            }
            return Ok(produtos);
         }

         [HttpPost]
         public ActionResult Post(Produto produto)
         {
            if(produto is null)
                return BadRequest();
            
            var novoProduto = _uof.ProdutoRepository.Create(produto);
            _uof.Commit();

            return new CreatedAtRouteResult("OvjetProduto", new {id = novoProduto.ProdutoId}, novoProduto);
         }

         [HttpPut("{id:int}")]
         public ActionResult Put(int id, Produto produto)
         {
            if(id != produto.ProdutoId)
            {
                return BadRequest();
            }

            var produtoAtualizado = _uof.ProdutoRepository.Update(produto);
            _uof.Commit();
            
            return Ok(produtoAtualizado);
         }

         [HttpDelete("{id:int}")]
         public ActionResult Delete(int id)
         {
            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if(produto is null)
            {
                return NotFound("Produto não encontrado");
            }

            var produtoDeletado = _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();

            return Ok(produtoDeletado);
         }
    }
} 