using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure
{
    public interface IAppCommands
    {
        CompositeCommand LogInCommand { get; }
        CompositeCommand LogOutCommand { get; }
        CompositeCommand SaveCommand { get; }
    }
}
