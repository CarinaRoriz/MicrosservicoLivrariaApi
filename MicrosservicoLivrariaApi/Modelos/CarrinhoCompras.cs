using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrosservicoLivrariaApi.Modelos
{
    public class CarrinhoCompras
    {
        public long Id { get; set; }
        public long IdUsuario { get; set; }
        public List<ItemCarrinhoCompras> listaItensCarrinho { get; set; }
        public string LoginUsuario { get; set; }
        public string SenhaUsuario { get; set; }
    }
}
