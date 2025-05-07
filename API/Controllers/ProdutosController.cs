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

        private readonly IProdutoRepository _repository;

        public ProdutosController(IProdutoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
         public ActionResult<IEnumerable<Produto>> Get()
         {
            var produtos = _repository.GetProdutos();

            if(produtos is null)
            {
                return NotFound("Produtos não encontrados");
            }
            return Ok(produtos);
         }
        [HttpGet("{id:int}")]
         public ActionResult<Produto> Get(int id)
         {
            var produtos = _repository.GetProduto(id);

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
            
            var novoProduto = _repository.Create(produto);

            return new CreatedAtRouteResult("OvjetProduto", new {id = novoProduto.ProdutoId}, novoProduto);
         }

         [HttpPut("{id:int}")]
         public ActionResult Put(int id, Produto produto)
         {
            if(id != produto.ProdutoId)
            {
                return BadRequest();
            }

            bool atualizado = _repository.Update(produto);

            if(atualizado)
            {
                return Ok(produto);
            }
            else
            {
                return StatusCode(500, $"Falha ao atualizar o produtp de id = {id}");
            }
         }

         [HttpDelete("{id:int}")]
         public ActionResult Delete(int id)
         {
            bool deletado = _repository.Delete(id);
            if(deletado)
            {
                return Ok($"Produto de id={id} foi excluido");
            }
            else{
                return StatusCode (500, $" Falaha ao excluir o produto de id={id}");
            }
         }
    }
} 