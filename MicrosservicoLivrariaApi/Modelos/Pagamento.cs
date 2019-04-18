using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrosservicoLivrariaApi.Modelos
{
    public class Pagamento
    {
        public long IdPagamento { get; set; }
        public long IdPedido { get; set; }
        public decimal ValorPago { get; set; }
        public DateTime DataPagamento { get; set; }
        public string LoginUsuario { get; set; }
        public string SenhaUsuario { get; set; }
    }
}
