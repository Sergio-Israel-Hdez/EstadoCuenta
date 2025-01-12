using AutoMapper;
using EstadoCuenta_Backend.Models;
using EstadoCuenta_Backend.Models.DTO;
using Microsoft.Data.SqlClient;

namespace EstadoCuenta_Backend.Handlers
{
    public class InsertarCompraCommandHandler
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;

        public InsertarCompraCommandHandler(string connectionString, IMapper mapper)
        {
            _connectionString = connectionString;
            _mapper = mapper;
        }

        public async Task HandleAsync(InsertarCompraCommandDTO commandDTO)
        {
            var command = _mapper.Map<InsertarCompraCommand>(commandDTO);
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SP_InsertarCompra";
                using (var sqlCommand = new SqlCommand(commandText, connection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Descripcion", command.Descripcion);
                    sqlCommand.Parameters.AddWithValue("@Monto", command.Monto);
                    sqlCommand.Parameters.AddWithValue("@TarjetaID", command.TarjetaID);

                    await sqlCommand.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
