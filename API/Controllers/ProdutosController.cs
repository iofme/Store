using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Interface;
using API.Models;
using API.Pagination;
using AutoMapper;
using Azure;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("produtos/{id}")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosCategoria(int id)
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosPorCategoriaAsync(id);

            if(produtos is null)
            {
                return NotFound();
            }

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDto);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>>Get([FromQuery] ProdutosParameters produtosParameters)
        {
            var produto = await _uof.ProdutoRepository.GetProdutosAsync(produtosParameters);

            return ObterProdutos(produto);
        }

        private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(IPagedList<Produto> produto)
        {
            var metadata = new
            {
                produto.Count,
                produto.PageSize,
                produto.PageCount,
                produto.TotalItemCount,
                produto.HasNextPage,
                produto.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var produtoDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produto);

            return Ok(produtoDTO);
        }

        [HttpGet("filter/preco/pagination")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosFilterPreco([FromQuery] ProdutosFiltrosPreco produtosFilterParams)
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosFiltroPrecoAsync(produtosFilterParams);

            return ObterProdutos(produtos);
        }

        [Authorize(Policy ="UserOnly")]
        [HttpGet]
         public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
         {
            var produtos = await _uof.ProdutoRepository.GetAllAsync();

            if(produtos is null)
            {
                return NotFound("Produtos não encontrados");
            }

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            
            return Ok(produtosDto);
         }
        [HttpGet("{id:int}")]
         public async Task<ActionResult<ProdutoDTO>> Get(int id)
         {
            var produtos = await _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if(produtos is null)
            {
                return NotFound("Produto não encontrado");
            }

            var produtoDTO = _mapper.Map<ProdutoDTO>(produtos);

            return Ok(produtoDTO);
         }

         [HttpPost]
         public async Task<ActionResult<ProdutoDTO>> Post(ProdutoDTO produtoDto)
         {
            if(produtoDto is null)
                return BadRequest(); 

            var produtoDTO = _mapper.Map<Produto>(produtoDto);
            
            var novoProduto = _uof.ProdutoRepository.Create(produtoDTO);
             await _uof.CommitAsync();

            var novoProdutoDTO = _mapper.Map<ProdutoDTO>(novoProduto);

            return new CreatedAtRouteResult("OvjetProduto", new {id = novoProdutoDTO.ProdutoId}, novoProdutoDTO);
         }

         [HttpPatch("{id}/UpdatePartial")]
         public async Task<ActionResult<ProdutoDTOUpdateResponse>> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
         {
          if(patchProdutoDTO is null || id <= 0)
                return BadRequest();

           var produto = await _uof.ProdutoRepository.Get(c => c.ProdutoId == id);

            if(produto is null)
                return NotFound();

            var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);      

            patchProdutoDTO.ApplyTo(produtoUpdateRequest, ModelState);

            if(!ModelState.IsValid || TryValidateModel(produtoUpdateRequest))
                return BadRequest(ModelState);

            _mapper.Map(produtoUpdateRequest, produto);

             _uof.ProdutoRepository.Update(produto);
             await _uof.CommitAsync();

             return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
         }

         [HttpPut("{id:int}")]
         public async Task<ActionResult<ProdutoDTO>> Put(int id, ProdutoDTO produtoDto)
         {
            if(id != produtoDto.ProdutoId)
            {
                return BadRequest();
            }

            var produtoDTO = _mapper.Map<Produto>(produtoDto);

            var produtoAtualizado = _uof.ProdutoRepository.Update(produtoDTO);
            await _uof.CommitAsync();
            
            var produtoAtualizadoDTO = _mapper.Map<ProdutoDTO>(produtoAtualizado);
            return Ok(produtoAtualizadoDTO);
         }

         [HttpDelete("{id:int}")]
         public async Task<ActionResult<ProdutoDTO>>  Delete(int id)
         {
            var produto = await _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if(produto is null)
            {
                return NotFound("Produto não encontrado");
            }

            var produtoDeletado = _uof.ProdutoRepository.Delete(produto);
            await _uof.CommitAsync();

            var produtoDeletadoDTO = _mapper.Map<ProdutoDTO>(produtoDeletado);
             
            return Ok(produtoDeletadoDTO);
         }
    }
} 