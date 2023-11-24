using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsMonitor.Internal
{
    internal class OptionsMonitor<T> : IOptionsMonitor<T>
    {
        private readonly T options;

        public OptionsMonitor(T options)
        {
            this.options = options;
        }

        public T CurrentValue => options;

        public T Get(string? name) => options;

        public IDisposable OnChange(Action<T, string> listener) => new NullDisposable();

        private class NullDisposable : IDisposable
        {
            public void Dispose() { }
        }
    }
}
