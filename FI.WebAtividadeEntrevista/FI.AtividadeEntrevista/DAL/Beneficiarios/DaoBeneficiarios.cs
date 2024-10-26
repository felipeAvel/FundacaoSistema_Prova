using FI.AtividadeEntrevista.DML;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace FI.AtividadeEntrevista.DAL
{
    /// <summary>
    /// Classe de acesso a dados de Beneficiarios
    /// </summary>
    internal class DaoBeneficiarios : AcessoDados
    {
        /// <summary>
        /// Inclui um novo Beneficiarios
        /// </summary>
        /// <param name="beneficiarios">Objeto de cliente</param>
        internal long Incluir(DML.Beneficiarios beneficiarios)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", beneficiarios.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", beneficiarios.CPF));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IdCliente", beneficiarios.IdCliente));

            DataSet ds = base.Consultar("FI_SP_IncBeneficiario", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        /// <summary>
        /// Lista todos os beneficiarios pelo  Idcliente
        /// </summary>
        internal List<DML.Beneficiarios> Listar(long IdCliente)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("ID", IdCliente));

            DataSet ds = base.Consultar("FI_SP_ConsBeneficiario", parametros);
            List<DML.Beneficiarios> ben = Converter(ds);

            return ben;
        }

        /// <summary>
        /// Verifica a Existencia do CPF
        /// </summary>
        internal bool VerificarExistencia(string cpf, long IdCliente)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", cpf));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IDCLIENTE", IdCliente));

            DataSet ds = base.Consultar("FI_SP_VerificaBeneficiario", parametros);

            return ds.Tables[0].Rows.Count > 0;
        }

        /// <summary>
        /// Verifica a Duplicidade na Alteracao
        /// </summary>
        internal bool VerificarDuplicidade(string Cpf, long IdCliente, long Id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", Cpf));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IDCLIENTE", IdCliente));

            DataSet ds = base.Consultar("FI_SP_VerificaBeneficiarioCPF", parametros);

            List<DML.Beneficiarios> ben = Converter(ds);

            foreach (var item in ben)
            {
                if (item.CPF != Cpf || item.IdCliente != IdCliente || item.Id != Id)
                    return true;
            }
            
            return false;
        }

        /// <summary>
        /// Excluir Beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de cliente</param>
        internal void Excluir(long Id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", Id));

            base.Executar("FI_SP_DelBeneficiario", parametros);
        }

        /// <summary>
        /// Alterar dados do Beneficiario
        /// </summary>
        /// <param name="beneficiarios">Objeto de cliente</param>
        internal void Alterar(DML.Beneficiarios beneficiarios)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", beneficiarios.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", beneficiarios.CPF));
            parametros.Add(new System.Data.SqlClient.SqlParameter("ID", beneficiarios.Id));

            base.Executar("FI_SP_AltBenef", parametros);
        }

        /// <summary>
        /// Converte os dados da Table para o Objeto
        /// </summary>
        private List<DML.Beneficiarios> Converter(DataSet ds)
        {
            List<DML.Beneficiarios> lista = new List<DML.Beneficiarios>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DML.Beneficiarios cli = new DML.Beneficiarios();
                    cli.Id = row.Field<long>("Id");
                    cli.Nome = row.Field<string>("Nome");
                    cli.CPF = row.Field<string>("CPF");
                    cli.IdCliente = row.Field<long>("IdCliente");
                    lista.Add(cli);
                }
            }

            return lista;
        }
    }
}
