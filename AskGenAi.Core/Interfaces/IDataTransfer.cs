using AskGenAi.Core.Entities;
using AskGenAi.Core.Models;

namespace AskGenAi.Core.Interfaces;

public interface IDataTransfer<TIn, TOut> where TIn : IEntity where TOut : class, IEntityRoot
{
    Task TransferLocalToCloudAsync();

    Task<IEnumerable<TOut>> GetDestinationEntitiesAsync();

    Task SaveChangesDestinationAsync(IEnumerable<TOut> outs);

    IOnPremisesRepository<TIn> GetSourceRepository();
}