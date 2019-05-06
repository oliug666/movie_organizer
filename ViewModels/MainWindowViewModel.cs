using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Morganizer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Exporter _exp;
        private string _filepath, _filename, _exportresult;
        public string Filepath { get { return _filepath; } set { _filepath = value; OnPropertyChanged(() => Filepath); } }
        public string Filename { get { return _filename; } set { _filename = value; OnPropertyChanged(() => Filename); } }
        public string ExportResult { get { return _exportresult; } set { _exportresult = value; OnPropertyChanged(() => ExportResult); } }
        public ICommand BrowseCommand { get; private set; }
        public ICommand ExportCommand { get; private set; }

        public MainWindowViewModel()
        {
            _exp = new Exporter();
            Filename = "movie_list.xlsx";
            Filepath = "";
            ExportResult = "";

            BrowseCommand = new DelegateCommand(obj => BrowseCommandExecution());
            ExportCommand = new DelegateCommand(obj => ExportCommandExecution(), obj => ExportCommandCanExecute);
        }

        private bool ExportCommandCanExecute
        {
            get
            {
                if (Filename.Count() != 0 && Path.GetExtension(Filename) == ".xlsx" &&
                    Filepath.Count() != 0 && Directory.Exists(Filepath))
                    return true;
                else
                    return false;
            }
        }

        private void ExportCommandExecution()
        {
            ExportResult = "Exporting ...";
            _exp.Export(_filepath, _filename);
            ExportResult = "Done!";
        }

        private void BrowseCommandExecution()
        {        
            // Create SaveFileDialog 
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = Filename; // Default file name            
            // Display 
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                Filepath = Path.GetDirectoryName(dlg.FileName) + "\\";
            }
        }
    }
}
