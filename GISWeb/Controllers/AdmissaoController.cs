﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Ninject;
using GISCore.Business.Abstract;
using GISModel.Entidades;
using GISModel.DTO.Shared;
using GISWeb.Infraestrutura.Provider.Abstract;

namespace GISWeb.Controllers
{
    public class AdmissaoController : Controller
    {
        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        [Inject]
        public IAdmissaoBusiness AdmissaoBusiness { get; set; }
        
        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CarregarEmpresas()
        {
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(new SelectList(EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDEmpresa", "NomeFantasia"));

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CarregarDepartamentos(string IDEmpresa)
        {
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(new SelectList(DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.IDEmpresa.Equals(IDEmpresa)).ToList(), "IDDepartamento", "Sigla"));

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Cadastrar()
        {
            try
            {
                ViewBag.Empresas = new SelectList(EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDEmpresa", "NomeFantasia");
                ViewBag.Departamentos = new SelectList(DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDDepartamento", "Sigla");
                return PartialView("Novo");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
        }

        [HttpPost]
        public ActionResult Cadastrar(string IDEmpresa, string IDDepartamento)
        {
            try
            {
                Admissao oAdmissao = new Admissao();
                oAdmissao.DataAdmissao = DateTime.Now;
                oAdmissao.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                oAdmissao.IDEmpresa = IDEmpresa;
                oAdmissao.IDDepartamentos = IDDepartamento;
                AdmissaoBusiness.Inserir(oAdmissao);
                    
                TempData["MensagemSucesso"] = "Admissao foi cadastrada com sucesso.";

                return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Empregado") } });
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