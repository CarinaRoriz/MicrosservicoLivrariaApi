using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicrosservicoLivrariaApi.Modelos;

namespace MicrosservicoLivrariaApi.Controllers
{
    [Route("v1/livros")]
    [ApiController]
    public class LivroController : ControllerBase
    {
        List<Livro> listaLivros;

        List<ComentarioLivro> listaComentarios;

        public LivroController()
        {
            listaLivros = new List<Livro>() {
                new Livro { Id = 1, Nome = "Livro 1", Descricao = "Livro 1", Preco = 10, QuantPaginas = 10, CodCategoria = 1 },
                new Livro { Id = 2, Nome = "Livro 2", Descricao = "Livro 2", Preco = 20, QuantPaginas = 40, CodCategoria = 2 },
                new Livro { Id = 3, Nome = "Livro 3", Descricao = "Livro 3", Preco = 25, QuantPaginas = 350, CodCategoria = 2 }
            };

            listaComentarios = new List<ComentarioLivro>() {
                new ComentarioLivro() { Id = 1, IdLivro = 3, IdUsuario = 2, DataComentario = new System.DateTime(2019, 04, 07), Descricao = "Ótima leitura" }
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Livro>>> GetLivros()
        {
            return listaLivros;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Livro>> GetLivros(int id)
        {
            var livro = listaLivros.Where(l=>l.Id == id).FirstOrDefault();

            if (livro == null)
            {
                return NotFound();
            }

            return livro;
        }

        [HttpGet("categorias/{id}")]
        public async Task<ActionResult<List<Livro>>> GetLivrosPorCategoria(int id)
        {
            List<Livro> livros = listaLivros.Where(l => l.CodCategoria == id).ToList();

            if (livros.Count() == 0)
            {
                return NotFound();
            }

            return livros;
        }
        
        [HttpPost]
        public async Task<ActionResult<List<Livro>>> CadastrarLivro(Livro livro)
        {
            Livro novoLivro = new Livro() { Id = (listaLivros.Max(l=>l.Id) + 1), Nome = livro.Nome, Descricao = livro.Descricao, CodCategoria = livro.CodCategoria, Preco = livro.Preco, QuantPaginas = livro.QuantPaginas };
            listaLivros.Add(novoLivro);

            return listaLivros;
        }

        [HttpPost("comentarios")]
        public async Task<ActionResult<List<ComentarioLivro>>> CadastrarComentarioLivro(ComentarioLivro comentarioLivro)
        {
            ComentarioLivro novoComentario = new ComentarioLivro() { Id = ((listaComentarios.Count() == 0) ? 1 : (listaComentarios.Max(l => l.Id) + 1)), IdLivro = comentarioLivro.IdLivro, IdUsuario = comentarioLivro.IdUsuario, DataComentario = comentarioLivro.DataComentario, Descricao = comentarioLivro.Descricao };
            listaComentarios.Add(novoComentario);

            return listaComentarios;
        }

        [HttpGet("comentarios/{id}")]
        public async Task<ActionResult<List<ComentarioLivro>>> GetComentariosPorLivro(int id)
        {
            List<ComentarioLivro> comentariosLivros = listaComentarios.Where(l => l.IdLivro == id).ToList();

            if (comentariosLivros.Count() == 0)
            {
                return NotFound();
            }

            return comentariosLivros;
        }
    }
}