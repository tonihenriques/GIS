using GISModel.CustomAttributes;
using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("OBJUsuario")]
    public class Usuario : EntidadeBase
    {

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "CPF obrigatório")]
        [CustomValidationCPF(ErrorMessage = "CPF inválido")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "Informe o nome do usuário")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe o Login do usuário")]
        public string Login { get; set; }

        public string Senha { get; set; }

        [Required(ErrorMessage = "Informe o e-mail do usuário")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Informe um e-mail válido")]
        public string Email { get; set; }

        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "Selecione uma empresa")]
        public string UKEmpresa { get; set; }

        public virtual Empresa Empresa { get; set; }

        [Display(Name = "Departamento")]
        [Required(ErrorMessage = "Selecione um departamento")]
        public string UKDepartamento { get; set; }

        public virtual Departamento Departamento { get; set; }

        [Display(Name = "Tipo de Acesso")]
        [Required(ErrorMessage = "Selecione como este usuário será validado")]
        public TipoDeAcesso? TipoDeAcesso { get; set; }

    }
}
