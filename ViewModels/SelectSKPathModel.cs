using System;
using TWatchSKDesigner.Models.SK;
using TWatchSKDesigner.Views;

namespace TWatchSKDesigner.ViewModels
{
    public class SelectSKPathModel : ViewModelBase
    {
        public SelectSKPathModel()
        {
            if (MainWindow.Instance?.Model?.SignalKManager != null)
            {
                _Paths = MainWindow.Instance?.Model.SignalKManager.GetSignalKPaths();
            }
            else
            {
                _Paths = new[]
                {
                    new SKPath("designer.test")
                    {
                        Value = "Sample value\r\nNew line",
                        Description = "Long Description test long test",
                        Source = "Sample",
                        Units = "m/s"
                    }
                };
            }
        }

        private SKPath[] _Paths;

        public SKPath[] Paths
        {
            get { return _Paths; }
            set { _Paths = value; OnPropertyChanged(nameof(Paths)); }
        }

        private SKPath _SelectedPath;

        public SKPath SelectedPath
        {
            get { return _SelectedPath; }
            set { _SelectedPath = value; OnPropertyChanged(nameof(SelectedPath)); }
        }
    }
}
