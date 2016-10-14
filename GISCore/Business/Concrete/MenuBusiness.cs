﻿using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class MenuBusiness : BaseBusiness<Menu>, IMenuBusiness
    {
        public override void Inserir(Menu Menu)
        {
            Menu.Nome = Menu.Nome.Trim();

            if (Consulta.Any(u => u.Nome.Equals(Menu.Nome) && u.IDMenuSuperior.Equals(Menu.IDMenuSuperior)))
                throw new InvalidOperationException("Não é possível inserir o menu, pois já existe um menu cadastro com este nome.");

            Menu.IDMenu = Guid.NewGuid().ToString();

            base.Inserir(Menu);
        }

        public override void Alterar(Menu Menu)
        {
            
            Menu tempMenu = Consulta.FirstOrDefault(p => p.IDMenu.Equals(Menu.IDMenu));
            if (tempMenu == null)
            {
                throw new Exception("Não foi possível encontra o menu através do ID.");
            }
            else
            {

                tempMenu.Nome = Menu.Nome;
                tempMenu.Ordem = Menu.Ordem;
                tempMenu.Controller = Menu.Controller;
                tempMenu.Action = Menu.Action;
                tempMenu.Icone = Menu.Icone;
                tempMenu.IDMenuSuperior = Menu.IDMenuSuperior;

                base.Alterar(tempMenu);

            }

        }

    }
}