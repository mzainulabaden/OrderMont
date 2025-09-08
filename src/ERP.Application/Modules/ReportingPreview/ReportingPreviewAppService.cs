using Abp.Application.Services;
using Abp.Domain.Uow;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using ERP.EntityFrameworkCore;
using Abp.EntityFrameworkCore;
using ERP.Modules.ReportingPreview.Dtos;
using AutoMapper;
using System.Linq;

namespace ERP.Modules.ReportingPreview
{
    public class ReportingPreviewAppService:ApplicationService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IDbContextProvider<ERPDbContext> _dbContextProvider;
        private readonly IMapper _mapper;

        public ReportingPreviewAppService(IUnitOfWorkManager unitOfWorkManager, IDbContextProvider<ERPDbContext> dbContextProvider, IMapper mapper)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _dbContextProvider = dbContextProvider;
            _mapper = mapper;
        }

        private async Task<List<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName, Action<SqlCommand> configureCommand)
        {
            var dbContext = _dbContextProvider.GetDbContext();
            var connection = dbContext.Database.GetDbConnection();

            var sqlConnection = (SqlConnection)connection;
            await using var sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = storedProcedureName;
            sqlCommand.CommandType = CommandType.StoredProcedure;

            var currentEfTransaction = dbContext.Database.CurrentTransaction;
            if (currentEfTransaction != null)
            {
                sqlCommand.Transaction = (SqlTransaction)currentEfTransaction.GetDbTransaction();
            }

            configureCommand?.Invoke(sqlCommand);

            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            await using var reader = await sqlCommand.ExecuteReaderAsync();
            var mapped = _mapper.Map<IDataReader, IEnumerable<T>>(reader);
            return mapped?.ToList() ?? new List<T>();
        }

        [UnitOfWork]
        public virtual async Task<List<SalesInvoiceDetailsResultDto>> GetSalesInvoiceDetails(SalesInvoiceDetailsRequestDto input)
        {
            return await ExecuteStoredProcedureAsync<SalesInvoiceDetailsResultDto>(
                "dbo.sp_IMS_SalesInvoiceDetails",
                cmd =>
                {
                    void AddParam(string name, object value, SqlDbType type, int? size = null)
                    {
                        var p = new SqlParameter(name, type) { Value = value ?? DBNull.Value };
                        if (size.HasValue) p.Size = size.Value;
                        cmd.Parameters.Add(p);
                    }

                    AddParam("@SalesInvoiceId", input.SalesInvoiceId, SqlDbType.BigInt);
                    AddParam("@ReferenceNumber", input.ReferenceNumber, SqlDbType.NVarChar, 100);
                    AddParam("@PaymentModeId", input.PaymentModeId, SqlDbType.BigInt);
                    AddParam("@CustomerId", input.CustomerId, SqlDbType.BigInt);
                    AddParam("@EmployeeId", input.EmployeeId, SqlDbType.BigInt);
                    AddParam("@ItemId", input.ItemId, SqlDbType.BigInt);
                }
            );
        }

        [UnitOfWork]
        public virtual async Task<List<SalesOrderDetailsResultDto>> GetSalesOrderDetails(SalesOrderDetailsRequestDto input)
        {
            return await ExecuteStoredProcedureAsync<SalesOrderDetailsResultDto>(
                "dbo.GetSalesOrderDetails",
                cmd =>
                {
                    void AddParam(string name, object value, SqlDbType type, int? size = null)
                    {
                        var p = new SqlParameter(name, type) { Value = value ?? DBNull.Value };
                        if (size.HasValue) p.Size = size.Value;
                        cmd.Parameters.Add(p);
                    }

                    AddParam("@SalesOrderId", input.SalesOrderId, SqlDbType.BigInt);
                    AddParam("@PaymentModeId", input.PaymentModeId, SqlDbType.Int);
                    AddParam("@CustomerId", input.CustomerId, SqlDbType.BigInt);
                    AddParam("@ItemId", input.ItemId, SqlDbType.BigInt);
                    AddParam("@UnitId", input.UnitId, SqlDbType.Int);
                    AddParam("@VoucherNumber", input.VoucherNumber, SqlDbType.NVarChar, 255);
                }
            );
        }
    }
}
