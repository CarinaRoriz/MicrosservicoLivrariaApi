using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrosservicoApi.Modelos
{
    public class Livro
    {
        public int? Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string Descricao { get; set; }
        public int QuantPaginas { get; set; }
        public int CodCategoria { get; set; }
    }
}
