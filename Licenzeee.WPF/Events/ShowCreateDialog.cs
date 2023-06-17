using System;

namespace Fateblade.Licenzeee.WPF.Events
{
    internal abstract class ShowDialog
    {
        public string Header { get; }

        protected ShowDialog(string header)
        {
            Header = header;
        }
    }
    internal class ShowCreateDialog<TToCreate> : ShowDialog
    {
        public Action<TToCreate> CreationCompletedCallback { get;  }
        public Action CreationAbortedCallback { get;  }

        public ShowCreateDialog(string header, Action<TToCreate> creationCompletedCallback, Action creationAbortedCallback)
        : base(header) 
        {
            CreationCompletedCallback = creationCompletedCallback;
            CreationAbortedCallback = creationAbortedCallback;
        }
    }
}
