
using System;
using System.ComponentModel;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LiteRipper.UI.Controls
{
    public partial class IsoRipper : UserControl
    {
        public IsoRipper()
        {
            InitializeComponent();
            DataContext = new IsoRipperViewModel();
        }

        public static readonly AvaloniaProperty<string> SelectedDriveLabelProperty = AvaloniaProperty.Register<DrivePicker, string>(nameof(SelectedDriveLabel));

        public string? SelectedDriveLabel
        {
            get => (DataContext != null ? ((IsoRipperViewModel)DataContext)?.SelectedDriveLabel : null);
            set
            {
                if (DataContext != null)
                {
                    ((IsoRipperViewModel)DataContext).SelectedDriveLabel = value ?? "";
                }
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }

    public class IsoRipperViewModel : INotifyPropertyChanged
    {
        private string path = "";

        public string Path
        {
            get => path;
            set
            {
                path = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Path)));
            }
        }

        public string SelectedDriveLabel { get; set; } = "";

        private double progress = 0;

        public double Progress
        {
            get => progress;
            set
            {
                progress = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Progress)));
            }
        }

        public override string ToString() => $"Path: {this.Path}\nProgress: {this.Progress}";

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}