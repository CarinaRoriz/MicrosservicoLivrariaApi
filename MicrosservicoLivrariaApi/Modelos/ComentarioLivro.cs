using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrosservicoLivrariaApi.Modelos
{
    public class ComentarioLivro
    {
        public int? Id { get; set; }
        public int IdLivro { get; set; }
        public int IdUsuario { get; set; }
        public string Descricao { get; set; }
        public DateTime DataComentario { get; set; }
    }
}
