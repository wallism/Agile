using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Shared.IoC;

namespace Agile.Framework.Server
{
    public abstract class RepositoryBase
    {
		private static ICache cache;

        protected static ICache Cache
		{
		    get { return cache ?? (cache = Container.Resolve<ICache>()); }
		}

    }
}
