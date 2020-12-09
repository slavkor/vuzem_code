using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace Ism.Infrastructure
{
    public class WindowAwareConfirmation : Confirmation, IWindow

    {
        protected WindowAwareConfirmation()
        {
            MinimizeCommand = new DelegateCommand<object>(OnMinimizeCommand, CanExecuteMinimizeCommand);
            MaximizeCommand = new DelegateCommand<object>(OnMaximizeCommand, CanExecuteMaximizeCommand);
            RestoreCommand = new DelegateCommand<object>(OnRestoreCommand, CanExecuteRestoreCommand);
            CloseCommand = new DelegateCommand<object>(OnCloseCommand, CanExecuteCloseCommand);
            CloseCommandByEscape = new DelegateCommand<object>(OnCloseCommandByEscape, CanExecuteCloseCommandByEscape);
        }

        public string TitleExtendet { get; set; }
        public DelegateCommand<object> MinimizeCommand { get; set; }
        public DelegateCommand<object> MaximizeCommand { get; set; }
        public DelegateCommand<object> RestoreCommand { get; set; }
        public DelegateCommand<object> CloseCommand { get; set; }
        public DelegateCommand<object> CloseCommandByEscape { get; set; }

        public virtual void OnMinimizeCommand(object window)
        {
            if (window == null) return;
            var wnd = (Window) window;
            SystemCommands.MinimizeWindow(wnd);
        }

        public virtual void OnMaximizeCommand(object window)
        {
            if (window == null) return;
            var wnd = (Window) window;
            SystemCommands.MaximizeWindow(wnd);
        }

        public virtual void OnRestoreCommand(object window)
        {
            if (window == null) return;
            var wnd = (Window) window;
            SystemCommands.RestoreWindow(wnd);
        }

        public virtual void OnCloseCommand(object window)
        {
            if (window == null) return;
            var wnd = (Window) window;
            SystemCommands.CloseWindow(wnd);
        }

        public virtual void OnCloseCommandByEscape(object window)
        {
            OnCloseCommand(window);
        }

        public virtual bool CanExecuteMinimizeCommand(object window)
        {
            return true;
        }

        public virtual bool CanExecuteMaximizeCommand(object window)
        {
            return true;
        }

        public virtual bool CanExecuteRestoreCommand(object window)
        {
            return true;
        }

        public virtual bool CanExecuteCloseCommand(object window)
        {
            return true;
        }

        public virtual bool CanExecuteCloseCommandByEscape(object window)
        {
            return true;
        }
    }
}
