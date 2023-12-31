﻿using Fateblade.Licenzee.Db;
using Fateblade.Licenzee.Db.Models;
using Fateblade.Licenzeee.WPF.Db;
using Fateblade.Licenzeee.WPF.Events;
using Prism.Commands;
using Prism.Events;

namespace Fateblade.Licenzeee.WPF.Dialogs;

internal class ModifyProductDialogViewModel : ProductDialogBaseViewModel
{
    private readonly ShowModifyDialog<Product> _dialogInfo;


    public ModifyProductDialogViewModel(IEventAggregator eventAggregator, ShowModifyDialog<Product> dialogInfo, IDb db)
        : base(eventAggregator, dialogInfo, db)
    {
        _dialogInfo = dialogInfo;

        Name = dialogInfo.ToModify.Name;
        Version = dialogInfo.ToModify.Version;
        Licenser = dialogInfo.ToModify.Licenser;
        Comment = dialogInfo.ToModify.Comment;

        Confirm = new DelegateCommand(
                modifyAndClose,
                () => !string.IsNullOrWhiteSpace(Name))
            .ObservesProperty(() => Name);

        Abort = new DelegateCommand(close);
    }


    private void modifyAndClose()
    {
        var createdProduct = Db.UpdateProduct(_dialogInfo.ToModify.Id, Name, Version, Licenser, Comment);

        CloseDialog();
        _dialogInfo.CompletedCallback(createdProduct);
    }

    private void close()
    {
        CloseDialog();
        _dialogInfo.AbortedCallback();
    }
}