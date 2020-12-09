using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Prism.Commands;
namespace Ism.Infrastructure
{
    public interface IWindow
    {
        string TitleExtendet { get; set; }

        DelegateCommand<object> MinimizeCommand { get; set; }
        DelegateCommand<object> MaximizeCommand { get; set; }
        DelegateCommand<object> RestoreCommand { get; set; }
        DelegateCommand<object> CloseCommand { get; set; }

        DelegateCommand<object> CloseCommandByEscape { get; set; }

        void OnMinimizeCommand(object window);
        void OnMaximizeCommand(object window);
        void OnRestoreCommand(object window);
        void OnCloseCommand(object window);
        void OnCloseCommandByEscape(object window);

        bool CanExecuteMinimizeCommand(object window);
        bool CanExecuteMaximizeCommand(object window);
        bool CanExecuteRestoreCommand(object window);
        bool CanExecuteCloseCommand(object window);
        bool CanExecuteCloseCommandByEscape(object window);


    }
}
