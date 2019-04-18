using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrosservicoLivrariaApi.Modelos
{
    public class Entrega
    {
        public int Id { get; set; }
        public int IdPedido { get; set; }
        public string Endereco { get; set; }
        public DateTime DataPrevista { get; set; }
        public DateTime? DataEntrega { get; set; }
        public int IdSituacao { get; set; }
    }
}
