using System.Collections.Generic;
using System.Threading.Tasks;
using Agile.Framework;
using Agile.Mobile.DAL;

namespace Agile.Mobile.Web
{
    public interface IHttpServiceBase<T> where T : class
    {
        Task<ServiceCallResult<T>> GetAsync(long id);
        Task<ServiceCallResult<T>> GetAsync(long id, IList<DeepLoader> loaders);

        /// <summary>
        /// Post simple objects
        /// </summary>
        Task<ServiceCallResult<T>> PostAsync(T instance, string url = "", int timeout = 10);

        /// <summary>
        /// Auto maps to the TP object (which is also returned)
        /// Definitely use this rather the PostAsync for objects with all but the simplest object maps.
        /// </summary>
        Task<ServiceCallResult<TR>> PostDtoAsync<TR, TP>(TR instance, string url = "", int timeout = 10) 
            where TR : class 
            where TP : class;

        Task<ServiceCallResult<TR>> PostFromSendQueueAsync<TR>(SendQueue queueRecord, int timeout = 10)
            where TR : class;

        /// <summary>
        /// Returns true if the resource exists, null is returned if the check could not be made or an error occurred.
        /// </summary>
        Task<bool?> CheckResourceExists(string uri);
    }
}