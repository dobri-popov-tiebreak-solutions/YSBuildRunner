using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace YsBuildRunner.Data.Commands
{
    public class OpenSolutionDirectoryCommand : ICommand
    {
        private readonly Solution _solution;
        public OpenSolutionDirectoryCommand(Solution solution)
        {
            _solution = solution;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var solutionDirectory = Path.GetDirectoryName(_solution.Path);
            Process.Start(solutionDirectory);
        }
    }
}
