using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LiteRipper;

namespace LiteRipper.UI.Controls
{
    public partial class DrivePicker : UserControl
    {
        public DrivePicker()
        {
            InitializeComponent();
            DataContext = new DrivePickerViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


        private ComboBox drivePicker { get => this.Find<ComboBox>("drivePicker"); }
        public event EventHandler<SelectionChangedEventArgs> DriveChanged
        {
            add
            {
                this.drivePicker.SelectionChanged += value;
            }
            remove
            {
                this.drivePicker.SelectionChanged -= value;
            }
        }

        public static readonly AvaloniaProperty<DriveInfo?> SelectedDriveProperty = AvaloniaProperty.Register<DrivePicker, DriveInfo?>(nameof(SelectedDrive));

        public DriveInfo? SelectedDrive
        {
            get => DataContext == null ? null : ((DrivePickerViewModel)DataContext).SelectedDrive;
            set
            {
                if (DataContext != null)
                {
                    ((DrivePickerViewModel)DataContext).SelectedDrive = value;
                }
            }
        }
    }

    public class DrivePickerViewModel : INotifyPropertyChanged
    {
        public DrivePickerViewModel()
        {
            this.drives = Utils.getDrivesByType("fixed").ToList<DriveInfo>() ?? new List<DriveInfo>();
            this.driveSize = this.drives.Count > 0 ? Utils.generateFileSize(this.drives.ToList()[0].TotalSize) : "No Drives Detected";
        }

        IReadOnlyCollection<DriveInfo> drives;

        public IReadOnlyCollection<DriveInfo> Drives
        {
            get => drives;
            private set
            {
                drives = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Drives)));

            }
        }

        string driveSize = "0 B";

        public string DriveSize
        {
            get => driveSize;
            set
            {
                driveSize = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DriveSize)));
            }
        }

        DriveInfo? drive;

        public DriveInfo? SelectedDrive
        {
            get => drive ?? drives.ToList()[0];
            set
            {
                drive = value;
                DriveSize = drive == null ? "No Drive Selected" : Utils.generateFileSize(drive.TotalSize - drive.AvailableFreeSpace);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedDrive)));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}