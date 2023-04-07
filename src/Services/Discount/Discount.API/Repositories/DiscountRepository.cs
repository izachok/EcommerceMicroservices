﻿using Dapper;
using Discount.API.Entities;
using Discount.API.Repositories.Interfaces;
using Npgsql;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetValue<string>("DatabaseSettings:ConnectionString");
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var affected = await connection.ExecuteAsync("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)", coupon);

            return affected != 0;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

            return affected != 0;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });
            if(coupon == null)
            {
                return new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
            }

            return coupon;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var affected = await connection.ExecuteAsync("UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id", coupon);

            return affected != 0;
        }
    }
}
