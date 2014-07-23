using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Mobile.Web.Testing.Model
{
    /// <summary>
    /// TESTING use only!
    /// Represents client side model of server side IndexModule.TestClass.
    /// Deliberately given a different name because the name shouldn't matter,
    /// we will have different classes (sometimes) client side that can be mapped
    /// to server side classes.
    /// </summary>
    /// <remarks>needs to be public because the tests are in a diff csproj</remarks>
    public class TestPersonClass
    {
        public override string ToString()
        {
            return string.Format("{0}[{1}]", Name, Age);
        }

        public string Name { get; set; }
        public int Age { get; set; }
        public string NotServerSide { get; set; }

        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
    }
}
