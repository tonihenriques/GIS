﻿using GISCore.Business.Abstract;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GISModel.DTO.Shared;

namespace GISWeb.Controllers
{
    public class TipoDeDocumentoController : Controller
    {

        #region Inject

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        [Inject]
        public IBaseBusiness<TipoDeDocumento> TipoDeDocumentoBusiness { get; set; }
        
        #endregion

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Novo(string UKCategoria)
        {
            try
            {
                TipoDeDocumento objTipo = new TipoDeDocumento();
                objTipo.UKCategoriaDeDocumento = UKCategoria;
                ViewBag.Intervalos = TipoDeDocumentoBusiness.GetTodosEnumsIntervalo();

                return PartialView("Novo", objTipo);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(TipoDeDocumento oTipo)
        {
            try
            {
                oTipo.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                TipoDeDocumentoBusiness.Inserir(oTipo);

                return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "CategoriaDeDocumento") } });
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