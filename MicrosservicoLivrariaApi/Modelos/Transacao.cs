using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrosservicoLivrariaApi.Modelos
{
    public class Transacao
    {
        public long Id { get; set; }
        public long IdPagamento { get; set; }
        public DateTime Data { get; set; }
        public int IdSituacao { get; set; }
    }
}
