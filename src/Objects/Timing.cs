using System;
using System.Diagnostics;
using System.Globalization;
using NLog;

namespace LinkInspector.Objects
{
    public class Timing : IDisposable
    {
        #region Fields

        private readonly String description;
        private readonly Stopwatch watch = Stopwatch.StartNew();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructors

        public Timing(string description, params string[] args)
        {
            this.description = string.Format(description, args, CultureInfo.CurrentUICulture);
        }

        #endregion

        #region Interface implementation

        public void Dispose()
        {
            watch.Stop();
            Logger.Info("{0} took {1} ms.", description, watch.ElapsedMilliseconds);
        }

        #endregion
    }
}
