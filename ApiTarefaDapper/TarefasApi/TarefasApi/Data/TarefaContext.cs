using System.Data;

namespace TarefasApi.Data
{
    // Essa Classe Atua como um contexto para a instância do dbconnection do Dapper
    public class TarefaContext
    {
        public delegate Task<IDbConnection> GetConnection();
    }
}
