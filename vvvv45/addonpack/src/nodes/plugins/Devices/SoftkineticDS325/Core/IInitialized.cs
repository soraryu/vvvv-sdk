using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VVVV.Core.Logging;

namespace VVVV.Nodes.DS325
{
    public delegate void OnInitEvent(bool success);
    public delegate void OnDisposeEvent(bool success);

    /// <summary>
    /// Interface for things that have to be initialized and stopped.
    /// </summary>
    interface IInitialized
    {
        void Init();
        void Dispose();
        void Update();

        OnInitEvent OnInit { get; set; }
        OnDisposeEvent OnDispose { get; set; }

        ILogger Logger { get; set; }
    }
}
