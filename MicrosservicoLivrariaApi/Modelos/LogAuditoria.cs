using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrosservicoLivrariaApi.Modelos
{
    public class LogAuditoria
    {
        public long Id { get; set; }
        public long IdTransacao { get; set; }
        public long IdPedido { get; set; }
        public string LoginUsuario { get; set; }
        public DateTime Data { get; set; }
    }
}
