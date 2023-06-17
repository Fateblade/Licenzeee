using System;

namespace Fateblade.Licenzeee.WPF.Events
{
    internal abstract class ShowDialogBase
    {
        public string Header { get; }

        protected ShowDialogBase(string header)
        {
            Header = header;
        }
    }

    internal abstract class ShowCompletableDialogBase : ShowDialogBase
    {
        public Action CompletedCallback { get; }
        public Action AbortedCallback { get; }

        protected ShowCompletableDialogBase(string header, Action completedCallback, Action abortedCallback)
            : base(header)
        {
            CompletedCallback = completedCallback;
            AbortedCallback = abortedCallback;
        }
    }

    internal abstract class ShowCompletableDialogBase<TPayload> : ShowCompletableDialogBase
    {
        public new Action<TPayload> CompletedCallback { get; set; }

        protected ShowCompletableDialogBase(string header, Action<TPayload> completedCallback, Action abortedCallback)
            : base(header, () => throw new NotImplementedException(), abortedCallback)
        {
            CompletedCallback = completedCallback;
        }
    }

    internal class ShowCreateDialog<TToCreate> : ShowCompletableDialogBase<TToCreate>
    {
        public ShowCreateDialog(string header, Action<TToCreate> creationCompletedCallback, Action creationAbortedCallback)
        : base(header, creationCompletedCallback, creationAbortedCallback) { }
    }

    internal class ShowModifyDialog<TToModify> : ShowCompletableDialogBase<TToModify>
    {
        public TToModify ToModify { get; }

        public ShowModifyDialog(string header, TToModify toModify, Action<TToModify> modifyCompletedCallback, Action modifyAbortedCallback)
            : base(header, modifyCompletedCallback, modifyAbortedCallback)
        {
            ToModify = toModify;
        }
    }
}
