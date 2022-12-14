using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{

    [Table("OBJArquivo")]
    public class Arquivo : EntidadeBase
    {

        public string UKObjeto { get; set; }

        public string UKTipoDeDocumento { get; set; }



        public string NomeLocal { get; set; }

        public string NomeRemoto { get; set; }

        public string Complemento { get; set; }

        public string Comentario { get; set; }

        public DateTime DataArquivo { get; set; }

    }
}
