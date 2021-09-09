using System.ComponentModel;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LiteRipper.UI.Controls;

namespace LiteRipper.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new MainWindowViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            var about = new Avalonia.Dialogs.AboutAvaloniaDialog();
            about.ShowDialog(this);
        }

        public void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? drivePicker = sender as ComboBox;
            if (DataContext != null && drivePicker != null && drivePicker.SelectedItem != null)
            {
                ((MainWindowViewModel)DataContext).SelectedDriveLabel = ((DriveInfo)drivePicker.SelectedItem).VolumeLabel;
            }

            IsoRipper? ripper = this.Find<IsoRipper>("ripper");
            if (DataContext != null && ripper != null)
            {
                ripper.SelectedDriveLabel = ((MainWindowViewModel)DataContext).SelectedDriveLabel;
            }
        }
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        string selectedDriveLabel = "";

        public string SelectedDriveLabel
        {
            get => selectedDriveLabel;
            set
            {
                selectedDriveLabel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedDriveLabel)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;



    }
}