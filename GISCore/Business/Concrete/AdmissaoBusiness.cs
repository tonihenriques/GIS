using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class AdmissaoBusiness : BaseBusiness<Admissao>, IAdmissaoBusiness
    {

        public override void Inserir(Admissao admissao)
        {

            admissao.UniqueKey = Guid.NewGuid().ToString();

            base.Inserir(admissao);

        }

        public override void Excluir(Admissao admissao)
        {
            base.Alterar(admissao);
        }

    }
}
