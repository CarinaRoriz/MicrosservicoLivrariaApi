using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicrosservicoApi.Modelos;

namespace MicrosservicoApi.Controllers
{
    [Route("v1/pedidos")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        List<Pedido> listaPedidos;
        List<ItemPedido> listaItensPedido;

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

            return itens;
        }

        [HttpPost]
        public async Task<ActionResult<List<Pedido>>> CadastrarPedido(CarrinhoCompras carrinho)
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

        [HttpPost("{id}/itens")]
        public async Task<ActionResult<List<ItemPedido>>> AdicionarItensPedido(long id, ItemPedido itemPedido)
        {
            ItemPedido novoItemPedido = new ItemPedido() { Id = ((listaItensPedido.Count() == 0) ? 1 : (listaItensPedido.Max(l => l.Id) + 1)), IdLivro = itemPedido.IdLivro, Quantidade = itemPedido.Quantidade, Valor = itemPedido.Valor, IdPedido = id };
            listaItensPedido.Add(novoItemPedido);

            Pedido pedido = listaPedidos.Where(p => p.Id == id).FirstOrDefault();
            pedido.ValorTotal = pedido.ValorTotal + (novoItemPedido.Quantidade * novoItemPedido.Valor);


            return listaItensPedido.Where(c => c.IdPedido == id).ToList();
        }

    }
}