using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace YsBuildRunner.Data.Commands
{
    public class OpenSolutionCommand : ICommand
    {
        private readonly Solution _solution;
        public OpenSolutionCommand(Solution solution)
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
            Process.Start(_solution.Path);
        }
    }
}
