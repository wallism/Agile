using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Diagnostics.Logging;
using Agile.Mobile.Web.Testing.Model;
using Agile.Shared.IoC;

namespace Agile.Mobile.Web
{
    public interface IIndexHttpService
    {
        Task<ServiceCallResult<string>> TestGetTextAsync();
        Task<ServiceCallResult<TestPersonClass>> TestGetClassAsync();
    }

    public class IndexHttpService : HttpServiceBase, IIndexHttpService
    {
        protected override string GetUrlBase()
        {
            return string.Format("{0}/index/", base.GetUrlBase());
        }


        public Task<ServiceCallResult<string>> TestGetTextAsync()
        {
            Logger.Debug("TestGetTextAsync");
            return GetAsync<string>("test");
        }

        public Task<ServiceCallResult<TestPersonClass>> TestGetClassAsync()
        {
            return GetAsync<TestPersonClass>("testclass");
        }

       

    }
    
} 
