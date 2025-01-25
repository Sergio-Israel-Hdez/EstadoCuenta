using AutoMapper;
using EstadoCuenta_Backend.Models.DTO;
using EstadoCuenta_Backend.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EstadoCuenta_Backend.Handlers
{
    public class ComprasQueryHandler
    {
        private string _connectionString;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public ComprasQueryHandler(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<List<ComprasResponseDTO>> HandleAsync(ComprasQuery query)
        {
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            var responseList = new List<ComprasResponseDTO>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand("SP_GetCompras", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@TarjetaID", query.TarjetaID);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync()) // Cambiado a un bucle para leer todas las filas
                    {
                        var result = new ComprasResponse
                        {
                            CompraId = (int)reader["CompraId"],
                            Fecha = (DateTime)reader["Fecha"],
                            Descripcion = (string)reader["Descripcion"],
                            Monto = (decimal)reader["Monto"],
                            TarjetaID = (int)reader["TarjetaID"]
                        };

                        // Mapea y añade cada objeto a la lista
                        responseList.Add(_mapper.Map<ComprasResponseDTO>(result));
                    }
                }
            }

            return responseList; // Devuelve la lista completa
        }
    }
}

