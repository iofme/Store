using API.Data;
using API.Filtros;
using API.Interface;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;

        public CategoriasController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _uof.CategoriaRepository.GetAll();

            return Ok(categorias);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Categoria> Get(int id)
        {

            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound("Categoria não encontrada");
            }

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if(categoria is null)
                return BadRequest("Dados inválidos");

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaCriada );
        }

        [HttpPut]
        public ActionResult Put(int id, Categoria categoria)
        {
            if(id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);
            if(categoria is null)
            {
                return NotFound();
            }

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();

            return Ok(categoriaExcluida);
        }
    }
}