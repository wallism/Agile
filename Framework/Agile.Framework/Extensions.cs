using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Diagnostics.Logging;

namespace Agile.Framework
{
    /// <summary>
    /// Useful extensions
    /// </summary>
    public static class Extensions
    {

        /// <summary>
        /// Convert the stream to a string
        /// </summary>
        public static string StreamToString(this Stream stream)
        {
            try
            {
                stream.Position = 0;
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Logger.Warning(ex.Message);
                throw; // rethrow, want client code to implement case specific handling
            }
        }
    }
}
