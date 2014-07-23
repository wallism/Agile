using System.Threading.Tasks;
using Agile.Mobile.DAL;

namespace Agile.Mobile.Web
{
    public interface IHttpServiceBase
    {
        /// <summary>
        /// Post simple objects
        /// </summary>
        Task<ServiceCallResult<T>> PostAsync<T>(T instance, string url = "") where T : class;

        /// <summary>
        /// Auto maps to the TP object (which is also returned)
        /// Definitely use this rather the PostAsync for objects with all but the simplest object maps.
        /// </summary>
        Task<ServiceCallResult<TR>> PostDtoAsync<TR, TP>(TR instance, string url = "") 
            where TR : class 
            where TP : class;

        Task<ServiceCallResult<TR>> PostFromSendQueueAsync<TR>(SendQueue queueRecord)
            where TR : class;
    }
}