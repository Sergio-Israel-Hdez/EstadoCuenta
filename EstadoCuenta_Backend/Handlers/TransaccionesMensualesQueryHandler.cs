using AutoMapper;
using EstadoCuenta_Backend.Models.DTO;
using EstadoCuenta_Backend.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EstadoCuenta_Backend.Handlers
{
    public class TransaccionesMensualesQueryHandler
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        public TransaccionesMensualesQueryHandler(string connectionString,IMapper mapper)
        {
            _connectionString = connectionString;
            _mapper = mapper;
        }
        public async Task<List<TransaccionesMensualesResponseDTO>> HandleAsync(TransaccionesMensualesQuery query)
        {
            var responseList = new List<TransaccionesMensualesResponseDTO>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand("SP_MostrarTransaccionesMesActual", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@TarjetaID", query.TarjetaID);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync()) // Cambiado a un bucle para leer todas las filas
                    {
                        var result = new TransaccionesMensualesResponse
                        {
                            TipoTransaccion = reader["TipoTransaccion"].ToString(),
                            TransaccionID = (int)reader["TransaccionID"],
                            Fecha = (DateTime)reader["Fecha"],
                            Descripcion = reader["Descripcion"].ToString(),
                            Monto = (decimal)reader["Monto"]
                        };

                        // Mapea y añade cada objeto a la lista
                        responseList.Add(_mapper.Map<TransaccionesMensualesResponseDTO>(result));
                    }
                }
            }

            return responseList; // Devuelve la lista completa
        }

    }
}
