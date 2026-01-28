using GRC.BuildingBlocks.Domain.SeedWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.BuildingBlocks.Infrastructure.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
    private readonly DbContext _dbContext;

    public TransactionBehavior(
        ILogger<TransactionBehavior<TRequest, TResponse>> logger,
        DbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        try
        {
            if (_dbContext.Database.CurrentTransaction != null)
            {
                return await next();
            }

            var strategy = _dbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

                _logger.LogInformation("Begin transaction for {RequestName}", requestName);

                var response = await next();

                _logger.LogInformation("Commit transaction for {RequestName}", requestName);

                await transaction.CommitAsync(cancellationToken);

                return response;
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in transaction for {RequestName}", requestName);
            throw;
        }
    }
}