using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{

    [Table("RELEstabelecimentoContrato")]
    public class EstabelecimentoContrato : EntidadeBase
    {

        public virtual Estabelecimento Estabelecimento { get; set; }

        [Display(Name = "Estabelecimento")]
        [Required(ErrorMessage = "Selecione um Estabelecimento")]
        public string UKEstabelecimento { get; set; }




        public virtual Contrato Contrato { get; set; }

        [Display(Name = "Contrato")]
        [Required(ErrorMessage = "Selecione um Contrato")]
        public string UKContrato { get; set; }

    }
}
