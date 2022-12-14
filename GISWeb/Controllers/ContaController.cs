using BotDetect.Web.UI.Mvc;
using GISCore.Business.Abstract;
using GISModel.DTO.Conta;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace GISWeb.Controllers
{
    public class ContaController : BaseController
    {

        #region

            [Inject]
            public ICustomAuthorizationProvider AutorizacaoProvider { get; set; }

            [Inject]
            public IUsuarioBusiness UsuarioBusiness { get; set; }

        #endregion

        public ActionResult Login()
        {
            return View();
        }

        [Autorizador]
        [DadosUsuario]
        public ActionResult Perfil()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AutenticacaoModel usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AutorizacaoProvider.Logar(usuario);
                    return Json(new { url = Url.Action("Index", "Painel") });
                }

                return View(usuario);
            }
            catch (Exception ex)
            {
                return Json(new { alerta = ex.Message, titulo = "Oops! Problema ao realizar login..." });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [CaptchaValidation("CaptchaCode", "LoginCaptcha", "Código do CAPTCHA incorreto.")]
        public ActionResult LoginComCaptcha(AutenticacaoModel usuario)
        {
            MvcCaptcha.ResetCaptcha("LoginCaptcha");
            ViewBag.IncluirCaptcha = Convert.ToBoolean(ConfigurationManager.AppSettings["AD:IncluirCaptchaNoLogin"]);

            try
            {
                if (ModelState.IsValid)
                {
                    AutorizacaoProvider.Logar(usuario);

                    return Json(new { url = Url.Action(ConfigurationManager.AppSettings["Web:DefaultAction"], ConfigurationManager.AppSettings["Web:DefaultController"]) });
                }

                return View("Login", usuario);
            }
            catch (Exception ex)
            {
                return Json(new { alerta = ex.Message, titulo = "Oops! Problema ao realizar login..." });
            }
        }

        public ActionResult Logout()
        {
            AutorizacaoProvider.Deslogar();
            Session.Clear();

            return RedirectToAction("Login", "Conta");
        }

        [OutputCache(Duration = 604800, Location = OutputCacheLocation.Client, VaryByParam = "login")]
        public ActionResult FotoPerfil(string login)
        {
            byte[] avatar = null;

            try
            {
                avatar = UsuarioBusiness.RecuperarFotoPerfil(login);
            }
            catch { }

            if (avatar == null || avatar.Length == 0)
                avatar = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/Ace/avatars/unknown.png"));

            return File(avatar, "image/jpeg");
        }

        public ActionResult DefinirNovaSenha(string id) {

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    TempData["MensagemErro"] = "Não foi possível recuperar a identificação do usuário.";
                }
                else
                {
                    id = GISHelpers.Utils.Criptografador.Descriptografar(WebUtility.UrlDecode(id.Replace("_@", "%")), 1);

                    string numDiasExpiracao = ConfigurationManager.AppSettings["Web:ExpirarLinkAcesso"];
                    if (string.IsNullOrEmpty(numDiasExpiracao))
                        numDiasExpiracao = "1";

                    if (DateTime.Now.Subtract(DateTime.ParseExact(id.Substring(id.IndexOf("#") + 1), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)).Days > int.Parse(numDiasExpiracao))
                    {
                        TempData["MensagemErro"] = "Este link já expirou, solicite um outro link na opção abaixo.";
                    }
                    else
                    {
                        NovaSenhaViewModel oNovaSenhaViewModel = new NovaSenhaViewModel();
                        oNovaSenhaViewModel.IDUsuario = id.Substring(0, id.IndexOf("#"));
                        return View(oNovaSenhaViewModel);
                    }
                }
            }
            catch (Exception ex) {
                if (ex.GetBaseException() == null)
                {
                    TempData["MensagemErro"] = ex.Message;
                }
                else
                {
                    TempData["MensagemErro"] = ex.GetBaseException().Message;
                }
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult DefinirSenha(NovaSenhaViewModel novaSenhaViewModel)
        {
            if (ModelState.IsValid)
            {
                if (novaSenhaViewModel.NovaSenha.Equals(novaSenhaViewModel.ConfirmarNovaSenha))
                {
                    try
                    {
                        if (string.IsNullOrEmpty(novaSenhaViewModel.IDUsuario))
                            return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível localizar o ID do usuário através de sua requisição. Solicite um novo acesso." } });

                        UsuarioBusiness.DefinirSenha(novaSenhaViewModel);
                        TempData["MensagemSucesso"] = "Senha alterada com sucesso.";
                        return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Login", "Conta") } });
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
                else {
                    return Json(new { resultado = new RetornoJSON() { Erro = "As duas senhas devem ser identicas." } });      
                }
            }
            else
            {
                return Json(new { resultado = TratarRetornoValidacaoToJSON() });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SolicitarAcesso(NovaSenhaViewModel novaSenhaViewModel)
        {
            if (!string.IsNullOrEmpty(novaSenhaViewModel.Email))
            {
                try
                {
                    UsuarioBusiness.SolicitarAcesso(novaSenhaViewModel.Email);
                    TempData["MensagemSucesso"] = "Solicitação de acesso realizada com sucesso.";
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Login", "Conta") } });
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
                return Json(new { resultado = new RetornoJSON() { Erro = "Informe o e-mail cadastrado em sua conta." } });
            }
        }

	}
}