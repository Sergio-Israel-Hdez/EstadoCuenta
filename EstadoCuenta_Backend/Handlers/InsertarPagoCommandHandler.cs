﻿using AutoMapper;
using EstadoCuenta_Backend.Models;
using EstadoCuenta_Backend.Models.DTO;
using Microsoft.Data.SqlClient;

namespace EstadoCuenta_Backend.Handlers
{
    public class InsertarPagoCommandHandler
    {
        private string _connectionString;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public InsertarPagoCommandHandler(IConfiguration configuration,IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task HandleAsync(InsertarPagoCommandDTO commandDto)
        {
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            var command = _mapper.Map<InsertarPagoCommand>(commandDto);
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SP_InsertarPago";
                using (var sqlCommand = new SqlCommand(commandText, connection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Monto", command.Monto);
                    sqlCommand.Parameters.AddWithValue("@TarjetaID", command.TarjetaID);

                    await sqlCommand.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
