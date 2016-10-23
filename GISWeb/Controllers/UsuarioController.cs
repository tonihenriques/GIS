﻿using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GISWeb.Controllers
{

    [DadosUsuario]
    [Autorizador]
    public class UsuarioController : BaseController
    {

        #region Inject

            [Inject]
            public IEmpresaBusiness EmpresaBusiness { get; set; }

            [Inject]
            public IUsuarioBusiness UsuarioBusiness { get; set; }

            [Inject]
            public IDepartamentoBusiness DepartamentoBusiness { get; set; }

            [Inject]
            public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        [MenuAtivo(MenuAtivo = "Administracao/Usuario")]
        public ActionResult Index()
        {
            ViewBag.Usuarios = UsuarioBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).OrderBy(o => o.Nome).ToList();
            return View();
        }

        [MenuAtivo(MenuAtivo = "Administracao/Usuario")]
        public ActionResult Novo()
        {
            ViewBag.Empresas = new SelectList(EmpresaBusiness.Consulta.ToList(), "IDEmpresa", "NomeFantasia");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Usuario Usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Usuario.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    UsuarioBusiness.Inserir(Usuario);

                    TempData["MensagemSucesso"] = "O usuário '" + Usuario.Nome + "' foi cadastrado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Usuario") } });
                }
                catch (Exception ex)
                {
                    if (ex.GetBaseException() == null)
                    {
                        return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
                    }
                    else
                    {
                        return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
                    }
                }

            }
            else
            {
                return Json(new { resultado = TratarRetornoValidacaoToJSON() });
            }
        }

        [MenuAtivo(MenuAtivo = "Administracao/Usuario")]
        public ActionResult Edicao(string id)
        {
            return View(UsuarioBusiness.Consulta.FirstOrDefault(p => p.IDUsuario.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Usuario Usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UsuarioBusiness.Alterar(Usuario);

                    TempData["MensagemSucesso"] = "O usuário '" + Usuario.Nome + "' foi atualizado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Usuario") } });
                }
                catch (Exception ex)
                {
                    if (ex.GetBaseException() == null)
                    {
                        return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
                    }
                    else
                    {
                        return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
                    }
                }

            }
            else
            {
                return Json(new { resultado = TratarRetornoValidacaoToJSON() });
            }
        }

        public ActionResult BuscarUsuarioPorID(string IDUsuario)
        {
            try
            {
                Usuario oUsuario = UsuarioBusiness.Consulta.FirstOrDefault(p => p.IDUsuario.Equals(IDUsuario));
                if (oUsuario == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Usuário com o ID '" + IDUsuario + "' não encontrado." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_Detalhes", oUsuario) });
                }
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException() == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
                }
                else
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
                }
            }
        }

        [HttpPost]
        public ActionResult Terminar(string IDUsuario)
        {

            try
            {
                Usuario oUsuario = UsuarioBusiness.Consulta.FirstOrDefault(p => p.IDUsuario.Equals(IDUsuario));
                if (oUsuario == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o usuário, pois o mesmo não foi localizado." } });
                }
                else
                {
                    oUsuario.DataExclusao = DateTime.Now;
                    oUsuario.UsuarioExclusao = "LoginTeste";
                    UsuarioBusiness.Alterar(oUsuario);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O usuário '" + oUsuario.Nome + "' foi excluído com sucesso." } });
                }
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException() == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
                }
                else
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
                }
            }

        }

	}
}