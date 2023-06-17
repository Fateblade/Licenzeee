using Fateblade.Licenzeee.WPF.Views;
using System.Windows;
using System.Windows.Controls;
using Fateblade.Licenzeee.WPF.Dialogs;
using Fateblade.Licenzeee.WPF.Inputs;

namespace Fateblade.Licenzeee.WPF
{
    internal class DisplayContentTemplateSelector : DataTemplateSelector
    {
        //views
        public DataTemplate? LicenseView { get; set; }
        public DataTemplate? UsersView { get; set; }
        public DataTemplate? ProductsView { get; set; }
        public DataTemplate? OptionsView { get; set; }

        //inputs
        public DataTemplate? UserConfirmation { get; set; }

        //dialogs
        public DataTemplate? CreateLicenseDialog { get; set; }
        public DataTemplate? CreateProductDialog { get; set; }
        public DataTemplate? CreateUserDialog { get; set; }
        public DataTemplate? ModifyLicenseDialog { get; set; }
        public DataTemplate? ModifyProductDialog { get; set; }
        public DataTemplate? ModifyUserDialog { get; set; }

        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                //views
                LicensesViewModel => LicenseView,
                UsersViewModel => UsersView,
                ProductsViewModel => ProductsView,
                OptionsViewModel => OptionsView,
                
                //inputs
                UserConfirmationInputViewModel => UserConfirmation,
                
                //dialogs
                CreateLicenseDialogViewModel => CreateLicenseDialog,
                CreateProductDialogViewModel => CreateProductDialog,
                CreateUserDialogViewModel => CreateUserDialog,

                _ => base.SelectTemplate(item, container)
            };
        }
    }
}
