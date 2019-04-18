using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicrosservicoLivrariaApi.Modelos;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace MicrosservicoLivrariaApi.Controllers
{
    [Route("v1/pedidos")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        List<Pedido> listaPedidos;
        List<ItemPedido> listaItensPedido;
        List<Pagamento> listaPagamento;


        public PedidoController()
        {
            listaItensPedido = new List<ItemPedido>
            {
                new ItemPedido { Id = 1, IdPedido = 1, IdLivro = 1, Quantidade = 2, Valor = 20 },
                new ItemPedido { Id = 2, IdPedido = 1, IdLivro = 2, Quantidade = 1, Valor = 10 }
            };

            listaPedidos = new List<Pedido>() {
                new Pedido { Id = 1, IdUsuario = 1, ValorTotal = 30, listaItensPedido = listaItensPedido },
                new Pedido { Id = 2, IdUsuario = 2 , ValorTotal = 0 }
            };

            listaPagamento = new List<Pagamento>() {
                new Pagamento { IdPagamento = 1, IdPedido = 1, ValorPago = 20, DataPagamento = new System.DateTime(2019, 04, 17) }
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            return listaPedidos;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedidos(int id)
        {
            var pedido = listaPedidos.Where(l => l.Id == id).FirstOrDefault();

            if (pedido == null)
            {
                return NotFound();
            }

            return pedido;
        }

        [HttpGet("itens/{id}")]
        public async Task<ActionResult<List<ItemPedido>>> GetItensPedido(int id)
        {
            List<ItemPedido> itens = listaItensPedido.Where(l => l.IdPedido == id).ToList();

            if (itens.Count() == 0)
            {
                return NotFound();
            }

            List<ItemPedido> lista = (from p in itens
                                      select new ItemPedido
                                      {
                                          Id = p.Id,
                                          IdPedido = p.IdPedido,
                                          IdLivro = p.IdLivro,
                                          Quantidade = p.Quantidade,
                                          Valor = p.Valor
                                      }).ToList();

            return lista;
        }

        [HttpPost]
        public async Task<ActionResult<List<Pedido>>> CadastrarPedido(CarrinhoCompras carrinho)
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
                Pedido novoPedido = new Pedido() { Id = ((listaPedidos.Count() == 0) ? 1 : (listaPedidos.Max(l => l.Id) + 1)), IdUsuario = carrinho.IdUsuario, listaItensPedido = new List<ItemPedido>(), ValorTotal = (carrinho.listaItensCarrinho == null || carrinho.listaItensCarrinho.Count() == 0) ? 0 : carrinho.listaItensCarrinho.Sum(c => c.Valor * c.Quantidade) };

                if (carrinho.listaItensCarrinho != null && carrinho.listaItensCarrinho.Count() > 0)
                {
                    foreach (ItemCarrinhoCompras item in carrinho.listaItensCarrinho)
                    {
                        ItemPedido itemPedido = new ItemPedido() { Id = ((listaItensPedido.Count() == 0) ? 1 : (listaItensPedido.Max(l => l.Id) + 1)), IdLivro = item.IdLivro, Quantidade = item.Quantidade, Valor = item.Valor, IdPedido = novoPedido.Id };
                        novoPedido.listaItensPedido.Add(itemPedido);
                    }
                }
                listaPedidos.Add(novoPedido);

                return listaPedidos;
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("{id}/itens")]
        public async Task<ActionResult<List<ItemPedido>>> AdicionarItensPedido(long id, ItemPedido itemPedido)
        {
            HttpClient client = new HttpClient();
            bool autenticado = false;

            string json = JsonConvert.SerializeObject(new Usuario { Login = itemPedido.LoginUsuario, Senha = itemPedido.SenhaUsuario }, Formatting.Indented);

            var buffer = System.Text.Encoding.UTF8.GetBytes(json);

            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var httpResult = await client.PostAsync("https://localhost:5002/v1/usuarios", byteContent).ConfigureAwait(false);

            //se retornar com sucesso busca os dados
            if (httpResult.IsSuccessStatusCode)
                autenticado = httpResult.Content.ReadAsAsync<bool>().Result;

            if (autenticado)
            {
                ItemPedido novoItemPedido = new ItemPedido() { Id = ((listaItensPedido.Count() == 0) ? 1 : (listaItensPedido.Max(l => l.Id) + 1)), IdLivro = itemPedido.IdLivro, Quantidade = itemPedido.Quantidade, Valor = itemPedido.Valor, IdPedido = id };
                listaItensPedido.Add(novoItemPedido);

                Pedido pedido = listaPedidos.Where(p => p.Id == id).FirstOrDefault();
                pedido.ValorTotal = pedido.ValorTotal + (novoItemPedido.Quantidade * novoItemPedido.Valor);
                
                List<ItemPedido> lista = (from p in listaItensPedido.Where(c => c.IdPedido == id)
                                         select new ItemPedido
                                         {
                                             Id = p.Id,
                                             IdPedido = p.IdPedido,
                                             IdLivro = p.IdLivro,
                                             Quantidade = p.Quantidade,
                                             Valor = p.Valor
                                         }).ToList();

                return lista;

            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("pagamentos")]
        public async Task<ActionResult<List<Pagamento>>> RealizarPagamento(Pagamento pagamento)
        {
            HttpClient client = new HttpClient();
            bool autenticado = false;

            string json = JsonConvert.SerializeObject(new Usuario { Login = pagamento.LoginUsuario, Senha = pagamento.SenhaUsuario }, Formatting.Indented);

            var buffer = System.Text.Encoding.UTF8.GetBytes(json);

            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var httpResult = await client.PostAsync("https://localhost:5002/v1/usuarios", byteContent).ConfigureAwait(false);

            //se retornar com sucesso busca os dados
            if (httpResult.IsSuccessStatusCode)
                autenticado = httpResult.Content.ReadAsAsync<bool>().Result;

            if (autenticado)
            {
                Pagamento novoPagamento = new Pagamento() { IdPagamento = ((listaPagamento.Count() == 0) ? 1 : (listaPagamento.Max(l => l.IdPagamento) + 1)), IdPedido = pagamento.IdPedido, ValorPago = pagamento.ValorPago, DataPagamento = pagamento.DataPagamento, LoginUsuario = pagamento.LoginUsuario, SenhaUsuario = pagamento.SenhaUsuario };
                listaPagamento.Add(novoPagamento);

                List<Pagamento> lista = (from p in listaPagamento
                                         select new Pagamento
                                         {
                                             IdPagamento = p.IdPagamento,
                                             IdPedido = p.IdPedido,
                                             DataPagamento = p.DataPagamento,
                                             ValorPago = p.ValorPago
                                         }).ToList();

                string jsonTransacao = JsonConvert.SerializeObject(new Transacao { IdPagamento = novoPagamento.IdPagamento, Data = pagamento.DataPagamento }, Formatting.Indented);

                var bufferTransacao = System.Text.Encoding.UTF8.GetBytes(jsonTransacao);

                var byteContentTransacao = new ByteArrayContent(bufferTransacao);

                byteContentTransacao.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var httpResultTransacao = await client.PostAsync("https://localhost:5003/v1/transacoes", byteContentTransacao).ConfigureAwait(false);

                long idTransacao = 0;

                //se retornar com sucesso busca os dados
                if (httpResultTransacao.IsSuccessStatusCode)
                    idTransacao = httpResultTransacao.Content.ReadAsAsync<long>().Result;

                if(idTransacao != 0)
                {
                    string jsonLog = JsonConvert.SerializeObject(new LogAuditoria { IdTransacao = idTransacao, IdPedido = pagamento.IdPedido, LoginUsuario = pagamento.LoginUsuario, Data = pagamento.DataPagamento }, Formatting.Indented);

                    var bufferLog = System.Text.Encoding.UTF8.GetBytes(jsonLog);

                    var byteContentLog = new ByteArrayContent(bufferLog);

                    byteContentLog.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var httpResultLog = await client.PostAsync("https://localhost:5004/v1/auditorias", byteContentLog).ConfigureAwait(false);

                    long idLog = 0;

                    //se retornar com sucesso busca os dados
                    if (httpResultLog.IsSuccessStatusCode)
                        idLog = httpResultLog.Content.ReadAsAsync<long>().Result;

                    if (idTransacao != 0)
                        return lista;
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return Unauthorized();
            }
        }

    }
}