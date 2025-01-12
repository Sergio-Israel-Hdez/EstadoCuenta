using AutoMapper;
using EstadoCuenta_Backend.Models;
using EstadoCuenta_Backend.Models.DTO;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;

namespace EstadoCuenta_Backend.Handlers
{
    public class EstadoCuentaQueryHandler
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;

        public EstadoCuentaQueryHandler(string connectionString,IMapper mapper)
        {
            _connectionString = connectionString;
            _mapper = mapper;
        }

        public async Task<EstadoCuentaResponseDTO> HandleAsync(EstadoCuentaQuery query)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand("SP_GetEstadoCuenta", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@UsuarioID", query.UsuarioID);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var result = new EstadoCuentaResponse
                        {
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            NumeroTarjeta = reader["NumeroTarjeta"].ToString(),
                            SaldoActual = (decimal)reader["SaldoActual"],
                            LimiteCredito = (decimal)reader["LimiteCredito"],
                            SaldoDisponible = (decimal)reader["SaldoDisponible"],
                            ComprasMesActual = (decimal)reader["ComprasMesActual"],
                            ComprasMesAnterior = (decimal)reader["ComprasMesAnterior"],
                            InteresBonificable = (decimal)reader["InteresBonificable"],
                            CuotaMinima = (decimal)reader["CuotaMinima"],
                            MontoTotalPagar = (decimal)reader["MontoTotalPagar"],
                            PagoContadoConIntereses = (decimal)reader["PagoContadoConIntereses"],
                            TarjetaID = (int)reader["TarjetaID"]
                        };
                        return _mapper.Map<EstadoCuentaResponseDTO>(result);
                    }
                    return null;
                }
            }
        }
    }
}
