using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib
{
    public interface ICosmosDbClient<TDocument> where TDocument : ICosmosDbDocument
    {
        Task<ICollection<TDocument>> GetAsync(string query);
        Task UpsertAsync(params TDocument[] documents);
        Task SetTimeToLiveAsync(string id, string partitionKey, int timeToLive);
    }
}