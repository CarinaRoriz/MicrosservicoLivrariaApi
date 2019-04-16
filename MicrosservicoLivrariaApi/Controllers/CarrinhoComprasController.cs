using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicrosservicoApi.Modelos;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace MicrosservicoApi.Controllers
{
    [Route("v1/carrinhos")]
    [ApiController]
    public class CarrinhoComprasController : ControllerBase
    {
        List<CarrinhoCompras> listaCarrinhos;
        List<ItemCarrinhoCompras> listaItensCarrinhos;

        public CarrinhoComprasController()
        {
            listaItensCarrinhos = new List<ItemCarrinhoCompras>
            {
                new ItemCarrinhoCompras { Id = 1, IdCarrinhoCompras = 1, IdLivro = 1, Quantidade = 2, Valor = 20 },
                new ItemCarrinhoCompras { Id = 2, IdCarrinhoCompras = 1, IdLivro = 2, Quantidade = 1, Valor = 10 }
            };

            listaCarrinhos = new List<CarrinhoCompras>() {
                new CarrinhoCompras { Id = 1, IdUsuario = 1, listaItensCarrinho = listaItensCarrinhos },
                new CarrinhoCompras { Id = 2, IdUsuario = 2 }
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarrinhoCompras>>> GetCarrinhos()
        {
            return listaCarrinhos;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarrinhoCompras>> GetCarrinhos(int id)
        {
            var carrinho = listaCarrinhos.Where(l => l.Id == id).FirstOrDefault();

            if (carrinho == null)
            {
                return NotFound();
            }

            return carrinho;
        }

        [HttpGet("itens/{id}")]
        public async Task<ActionResult<List<ItemCarrinhoCompras>>> GetItensCarrinho(int id)
        {
            List<ItemCarrinhoCompras> itens = listaItensCarrinhos.Where(l => l.IdCarrinhoCompras == id).ToList();

            if (itens.Count() == 0)
            {
                return NotFound();
            }

            return itens;
        }

        [HttpPost]
        public async Task<ActionResult<List<CarrinhoCompras>>> CadastrarCarrinho(CarrinhoCompras carrinho)
        {
            HttpClient client = new HttpClient();
            bool autenticado = false;

            string json = JsonConvert.SerializeObject(new Usuario { Login = carrinho.LoginUsuario, Senha = carrinho.SenhaUsuario }, Formatting.Indented);

            var buffer = System.Text.Encoding.UTF8.GetBytes(json);

            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var httpResult = await client.PostAsync("https://localhost:5002/v1/usuarios", byteContent).ConfigureAwait(false);

            //se retornar com sucesso busca os dados
            if (httpResult.IsSuccessStatusCode)
                autenticado = httpResult.Content.ReadAsAsync<bool>().Result;

            if (autenticado)
            {
                CarrinhoCompras novoCarrinho = new CarrinhoCompras() { Id = ((listaCarrinhos.Count() == 0) ? 1 : (listaCarrinhos.Max(l => l.Id) + 1)), IdUsuario = carrinho.IdUsuario, listaItensCarrinho = new List<ItemCarrinhoCompras>() };

                foreach (ItemCarrinhoCompras item in carrinho.listaItensCarrinho)
                {
                    ItemCarrinhoCompras itemCarrinho = new ItemCarrinhoCompras() { Id = ((listaItensCarrinhos.Count() == 0) ? 1 : (listaItensCarrinhos.Max(l => l.Id) + 1)), IdLivro = item.IdLivro, Quantidade = item.Quantidade, Valor = item.Valor, IdCarrinhoCompras = novoCarrinho.Id };
                    novoCarrinho.listaItensCarrinho.Add(itemCarrinho);
                }

                listaCarrinhos.Add(novoCarrinho);

                return listaCarrinhos;
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("{id}/itens")]
        public async Task<ActionResult<List<ItemCarrinhoCompras>>> AdicionarItensCarrinho(long id, ItemCarrinhoCompras itemCarrinhoCompras)
        {
            HttpClient client = new HttpClient();
            bool autenticado = false;

            string json = JsonConvert.SerializeObject(new Usuario { Login = itemCarrinhoCompras.LoginUsuario, Senha = itemCarrinhoCompras.SenhaUsuario }, Formatting.Indented);

            var buffer = System.Text.Encoding.UTF8.GetBytes(json);

            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var httpResult = await client.PostAsync("https://localhost:5002/v1/usuarios", byteContent).ConfigureAwait(false);

            //se retornar com sucesso busca os dados
            if (httpResult.IsSuccessStatusCode)
                autenticado = httpResult.Content.ReadAsAsync<bool>().Result;

            if (autenticado)
            {
                ItemCarrinhoCompras novoItemCarrinho = new ItemCarrinhoCompras() { Id = ((listaItensCarrinhos.Count() == 0) ? 1 : (listaItensCarrinhos.Max(l => l.Id) + 1)), IdLivro = itemCarrinhoCompras.IdLivro, Quantidade = itemCarrinhoCompras.Quantidade, Valor = itemCarrinhoCompras.Valor, IdCarrinhoCompras = id };
                listaItensCarrinhos.Add(novoItemCarrinho);

                return listaItensCarrinhos.Where(c => c.IdCarrinhoCompras == id).ToList();
            }
            else
            {
                return Unauthorized();
            }
        }

    }
}