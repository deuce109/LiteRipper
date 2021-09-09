
using System.Collections.Generic;
using System.IO;

using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;

namespace LiteRipper.UI.Controls
{
    public partial class IsoRipper : UserControl
    {
        public async void OpenDialog(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                var filters = new FileDialogFilter()
                {
                    Extensions = new List<string>() { ".iso" },
                    Name = "ISO Files"
                };
                var dialog = new SaveFileDialog()
                {
                    Filters = new List<FileDialogFilter>() { filters }
                };

                if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    ((IsoRipperViewModel)DataContext).Path = await dialog.ShowAsync(desktop.MainWindow);
                }
            }


        }


        private void BurnImage(object sender, RoutedEventArgs e)
        {


            if (DataContext != null && this.SelectedDriveLabel != null)
            {
                DiscUtilsWrapper.DiscUtilsWrapper.CreateIsoImage(this.SelectedDriveLabel, ((IsoRipperViewModel)DataContext).Path, this.SelectedDriveLabel);

            }
        }
    }
}