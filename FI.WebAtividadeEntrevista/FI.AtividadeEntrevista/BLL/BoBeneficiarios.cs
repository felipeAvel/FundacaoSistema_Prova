using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiarios
    {
        /// <summary>
        /// Inclui um novo Beneficiarios
        /// </summary>
        /// <param name="beneficiario">Objeto de Beneficiarios</param>
        public long Incluir(DML.Beneficiarios beneficiarios)
        {
            DAL.DaoBeneficiarios ben = new DAL.DaoBeneficiarios();
            return ben.Incluir(beneficiarios);
        }

        ///// <summary>
        ///// Lista os Beneficiarios
        ///// </summary>
        public List<DML.Beneficiarios> Listar(long IdCliente)
        {
            DAL.DaoBeneficiarios ben = new DAL.DaoBeneficiarios();
            return ben.Listar(IdCliente);
        }


        /// <summary>
        /// Altera um Beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de cliente</param>
        public void Alterar(DML.Beneficiarios beneficiarios)
        {
            DAL.DaoBeneficiarios ben = new DAL.DaoBeneficiarios();
            ben.Alterar(beneficiarios);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string Cpf, long IdCliente)
        {
            DAL.DaoBeneficiarios ben = new DAL.DaoBeneficiarios();
            return ben.VerificarExistencia(Cpf, IdCliente);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarDuplicidade(string Cpf, long IdCliente, long Id)
        {
            DAL.DaoBeneficiarios ben = new DAL.DaoBeneficiarios();
            return ben.VerificarDuplicidade(Cpf, IdCliente, Id);
        }
    }
}
