using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using System.Text.RegularExpressions;
using FI.WebAtividadeEntrevista.Models;
using System.Reflection;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        /// <summary>
        /// Responsavel pela View Index
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Responsavel pela View Incluir
        /// </summary>
        public ActionResult Incluir()
        {
            return View();
        }

        /// <summary>
        /// Responsavel por Incluir um novo Cliente
        /// </summary>
        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                ///Realiza Validacao no CPF
                ///Caso encontre alguma Irregularidade Informa o User na View
                if (ValidarCPF(model.CPF) != true)
                {
                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, "CPF Inválido!"));
                }

                ///Realiza Validacao de Existencia por CPF
                ///Caso encontre alguma Irregularidade Informa o User na View
                if (bo.VerificarExistencia(model.CPF) != false)
                {
                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, "CPF já Existente no Banco de Dados!"));
                }

                ///Monta Objeto para Inseir no Banco
                model.Id = bo.Incluir(new Cliente()
                {
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    CPF = model.CPF,
                    Telefone = model.Telefone
                });

                return Json("Cadastro efetuado com sucesso");
            }
        }

        /// <summary>
        /// Responsavel pela View de Alteracao do dados do Cliente
        /// </summary>
        [HttpGet]
        public ActionResult Alterar(long Id)
        {
            ///Obter lista de Beneficiarios por IdCliente
            BoBeneficiarios boBen = new BoBeneficiarios();
            var listaBeneficiarios = boBen.Listar(Id);

            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(Id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    CPF = cliente.CPF,
                    Telefone = cliente.Telefone,
                    Beneficiarios = listaBeneficiarios
                };
            }

            ///Gerencia a Exibicao do btn Beneficiarios
            ///Lembrando que para Tela de Cadastro nao ´e possivel adicionar os Beneficiarios
            ///Somente apos o Cadastro o Clinte acessando atravez da View Alterar Cliente
            ViewBag.ViewAlterar = true;

            return View(model);
        }

        /// <summary>
        /// Salva as Alteracoes do Cliente
        /// </summary>
        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                ///Realiza Validacao no CPF
                ///Caso encontre alguma Irregularidade Informa o User na View
                if (ValidarCPF(model.CPF) != true)
                {
                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, "CPF Inválido!"));
                }

                ///Realiza Validacao de Existencia por CPF
                ///Caso encontre alguma Irregularidade Informa o User na View
                if (bo.VerificarDuplicidade(model.CPF, model.Id) != false)
                {
                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, "CPF já Existente no Banco de Dados!"));
                }

                ///Monta Objeto para Alterar no Banco
                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    CPF = model.CPF,
                    Telefone = model.Telefone
                });

                return Json("Cadastro alterado com sucesso");
            }
        }

        /// <summary>
        /// Inclui um novo Beneficiario
        /// </summary>
        [HttpPost]
        public JsonResult IncluirBeneficiario(BeneficiarioModel model)
        {
            BoBeneficiarios ben = new BoBeneficiarios();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                ///Realiza Validacao no CPF
                ///Caso encontre alguma Irregularidade Informa o User na View
                if (ValidarCPF(model.CPF) != true)
                    return Json("CPF Invalido", JsonRequestBehavior.AllowGet);

                ///Realiza Validacao de Existencia por CPF
                ///Caso encontre alguma Irregularidade Informa o User na View
                if (ben.VerificarExistencia(Regex.Replace(model.CPF, @"[^\d]", ""), model.IdCliente) != false)
                    return Json("CPF ja existente no banco de dados", JsonRequestBehavior.AllowGet);

                ///Monta Objeto para Inseir no Banco
                model.Id = ben.Incluir(new Beneficiarios()
                {
                    CPF = Regex.Replace(model.CPF, @"[^\d]", ""),
                    Nome = model.Nome,
                    IdCliente = model.IdCliente
                });
            }

            return Json("Sucesso", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Salva as Alteracoes do Beneficiario
        /// </summary>
        [HttpPost]
        public JsonResult AlterarBeneficiario(BeneficiarioModel model)
        {
            BoBeneficiarios ben = new BoBeneficiarios();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                ///Realiza Validacao no CPF
                ///Caso encontre alguma Irregularidade Informa o User na View
                if (ValidarCPF(model.CPF) != true)
                    return Json("CPF Invalido", JsonRequestBehavior.AllowGet);

                ///Realiza Validacao de Existencia por CPF
                ///Caso encontre alguma Irregularidade Informa o User na View
                if (ben.VerificarDuplicidade(Regex.Replace(model.CPF, @"[^\d]", ""), model.IdCliente, model.Id) != false)
                    return Json("CPF ja existente no banco de dados", JsonRequestBehavior.AllowGet);

                ///Monta Objeto para Inseir no Banco
                ben.Alterar(new Beneficiarios()
                {
                    Id = model.Id,
                    Nome = model.Nome,
                    CPF = Regex.Replace(model.CPF, @"[^\d]", "")
                });

                return Json("Beneficiario alterado com sucesso");
            }
        }

        /// <summary>
        /// Responsavel por Excluir o Beneficiario
        /// </summary>
        [HttpPost]
        public JsonResult ExcluirBeneficiario(long Id)
        {
            BoCliente bo = new BoCliente();

            bo.Excluir(Id);

            return Json("Sucesso", JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Gera lista de Clientes
        /// </summary>
        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        /// <summary>
        /// Validador de CPF
        /// </summary>
        public bool ValidarCPF(string cpf)
        {
            // Remover pontos e traços do CPF
            cpf = Regex.Replace(cpf, @"[^\d]", "");

            // Verifica se o CPF tem 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Verifica se todos os dígitos são iguais (ex: 11111111111) - CPF inválido
            if (cpf.All(c => c == cpf[0]))
                return false;

            // Calcula o primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (cpf[i] - '0') * (10 - i);

            int primeiroDigitoVerificador = (soma * 10) % 11;
            if (primeiroDigitoVerificador == 10)
                primeiroDigitoVerificador = 0;

            // Verifica se o primeiro dígito verificador está correto
            if (primeiroDigitoVerificador != (cpf[9] - '0'))
                return false;

            // Calcula o segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (cpf[i] - '0') * (11 - i);

            int segundoDigitoVerificador = (soma * 10) % 11;
            if (segundoDigitoVerificador == 10)
                segundoDigitoVerificador = 0;

            // Verifica se o segundo dígito verificador está correto
            if (segundoDigitoVerificador != (cpf[10] - '0'))
                return false;

            // CPF é válido
            return true;
        }
    }
}