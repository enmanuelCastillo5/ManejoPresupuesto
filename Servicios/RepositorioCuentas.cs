using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioCuentas
    {
        Task Actualizar(CuentaCreacionViewModel cuenta);
        Task<IEnumerable<Cuenta>> Buscar(int usuarioId);
        Task Crear(Cuenta cuenta);
        Task<Cuenta> ObtenerPorId(int id, int usuarioId);
    }
    public class RepositorioCuentas : IRepositorioCuentas
    {
        private readonly string connectionString;

        public RepositorioCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");    
        }

        public async Task Crear(Cuenta cuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"insert into CuentasTabla 
                                                       (Nombre, TipoCuentaId, Descripcion, Balance)
                                                        values (@Nombre, @TipoCuentaId, @Descripcion, @Balance);
                                                        SELECT scope_identity();", cuenta);

            cuenta.Id = id;
        }

        public async Task<IEnumerable<Cuenta>> Buscar(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Cuenta>(@"SELECT CuentasTabla.Id, CuentasTabla.Nombre, Balance, tc.Nombre As TipoCuenta
                                                        from CuentasTabla
                                                        INNER JOIN TiposCuentasTabla tc
                                                        ON tc.Id = CuentasTabla.TipoCuentaId
                                                        WHERE tc.UsuarioId = @UsuarioId
                                                        ORDER By tc.Orden
                                                        ", new {usuarioId});
        }

        public async Task<Cuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Cuenta>(@"SELECT CuentasTabla.Id, CuentasTabla.Nombre, Balance, Descripcion, tc.Id
                                                        from CuentasTabla
                                                        INNER JOIN TiposCuentasTabla tc
                                                        ON tc.Id = CuentasTabla.TipoCuentaId
                                                        WHERE tc.UsuarioId = @UsuarioId AND CuentasTabla.Id = @Id", new { id, usuarioId });
        }

        public async Task Actualizar(CuentaCreacionViewModel cuenta)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE CuentasTabla
                                          Set Nombre = @NOmbre, Balance = @Balance, Descripcion = @Descripcion, TipoCuentaId = @TipoCuentaId
                                          where Id = @Id", cuenta);

        }
    }
}
