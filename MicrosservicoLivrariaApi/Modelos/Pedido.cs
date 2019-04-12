using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrosservicoApi.Modelos
{
    public class Pedido
    {
        public long Id { get; set; }
        public long IdUsuario { get; set; }
        public decimal? ValorTotal { get; set; }
        public List<ItemPedido> listaItensPedido { get; set; }
    }
}
