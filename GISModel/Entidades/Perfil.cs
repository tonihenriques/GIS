using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("OBJPerfil")]
    public class Perfil : EntidadeBase
    {

        [Required(ErrorMessage = "O perfil é obrigatório")]
        public string Nome { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Action")]
        [Required(ErrorMessage = "Informe a página padrão")]
        public string ActionDefault { get; set; }

        [Display(Name = "Controller")]
        [Required(ErrorMessage = "Informe o controlador padrão")]
        public string ControllerDefault { get; set; }

    }
}
