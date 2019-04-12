using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrosservicoApi.Modelos
{
    public class ItemCarrinhoCompras
    {
        public long Id { get; set; }
        public long IdCarrinhoCompras { get; set; }
        public long IdLivro { get; set; }
        public decimal Quantidade { get; set; }
        public decimal Valor { get; set; }
    }
}
