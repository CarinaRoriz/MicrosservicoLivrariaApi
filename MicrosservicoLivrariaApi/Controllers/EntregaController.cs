using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicrosservicoApi.Modelos;

namespace MicrosservicoApi.Controllers
{
    [Route("v1/entregas")]
    [ApiController]
    public class EntregaController : ControllerBase
    {
        List<Entrega> listaEntregas;

        public EntregaController()
        {
            //Situação 1: Aguardando Entrega
            //Situação 2: Entregue

            listaEntregas = new List<Entrega>() {
                new Entrega { Id = 1, IdPedido = 1, Endereco = "Rua A", DataPrevista = new System.DateTime(2019, 04, 07), DataEntrega = new System.DateTime(2019, 04, 07), IdSituacao = 2 },
                new Entrega { Id = 2, IdPedido = 1, Endereco = "Rua A", DataPrevista = new System.DateTime(2019, 04, 07), DataEntrega = null, IdSituacao = 1 }
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entrega>>> GetEntregas()
        {
            return listaEntregas;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Entrega>> GetEntregas(int id)
        {
            var pedido = listaEntregas.Where(l => l.Id == id).FirstOrDefault();

            if (pedido == null)
            {
                return NotFound();
            }

            return pedido;
        }

        [HttpGet("emAberto")]
        public async Task<ActionResult<List<Entrega>>> GetEntregasEmAberto()
        {
            List<Entrega> itens = listaEntregas.Where(l => l.IdSituacao == 1).ToList();

            if (itens.Count() == 0)
            {
                return NotFound();
            }

            return itens;
        }

        [HttpGet("finalizadas")]
        public async Task<ActionResult<List<Entrega>>> GetEntregasFinalizadas()
        {
            List<Entrega> itens = listaEntregas.Where(l => l.IdSituacao == 2).ToList();

            if (itens.Count() == 0)
            {
                return NotFound();
            }

            return itens;
        }

        [HttpPost]
        public async Task<ActionResult<List<Entrega>>> CadastrarEntrega(Entrega entrega)
        {
            Entrega novaEntrega = new Entrega() { Id = ((listaEntregas.Count() == 0) ? 1 : (listaEntregas.Max(l => l.Id) + 1)), IdPedido = entrega.IdPedido, Endereco = entrega.Endereco, DataPrevista = entrega.DataPrevista, DataEntrega = entrega.DataEntrega, IdSituacao = entrega.IdSituacao };

            listaEntregas.Add(novaEntrega);

            return listaEntregas;
        }


    }
}