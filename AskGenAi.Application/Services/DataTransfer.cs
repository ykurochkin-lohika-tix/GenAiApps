using AutoMapper;
using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;
using AskGenAi.Core.Models;

namespace AskGenAi.Application.Services;

public class DataTransfer<TIn, TOut>(
    IOnPremisesRepository<TIn> sourceRepository,
    IRepository<TOut> destinationData,
    IMapper mapper) : IDataTransfer<TIn, TOut> where TIn : IEntity
    where TOut : class, IEntityRoot
{
    public async Task TransferLocalToCloudAsync()
    {
        var entities = await sourceRepository.GetAllAsync();

        var outs = mapper.Map<IEnumerable<TOut>>(entities);

        await destinationData.AddRangeAsync(CancellationToken.None, outs.ToArray());

        await destinationData.UnitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<TOut>> GetDestinationEntitiesAsync()
    {
        var entities = await sourceRepository.GetAllAsync();

        var outs = mapper.Map<IEnumerable<TOut>>(entities);

        return outs;
    }

    public async Task SaveChangesDestinationAsync(IEnumerable<TOut> outs)
    {
        await destinationData.AddRangeAsync(CancellationToken.None, outs.ToArray());
        await destinationData.UnitOfWork.SaveChangesAsync();
    }

    public IOnPremisesRepository<TIn> GetSourceRepository()
    {
        return sourceRepository;
    }
}