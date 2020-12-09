using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ism.Infrastructure;
using Prism.Mvvm;
using System.Globalization;
using Ism.Infrastructure.Repository;
using Ism.Views;
using Prism.Regions;
using Ism.Infrastructure.Services;
using Prism.Unity;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using System.Threading;

namespace Ism
{
    public class BootsTrapper : UnityBootstrapper
    {

        protected override DependencyObject CreateShell()
        {
            StyleManager.ApplicationTheme = new FluentTheme();
            return Container.Resolve<Shell>();
        }
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            Container.RegisterType<IAppCommands, AppCommands>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IExceptionService, ExceptionService>(new ContainerControlledLifetimeManager());

        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            var catalog = new DirectoryModuleCatalog() { ModulePath = @".\Modules" };
            return catalog;
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            //Application.Current.MainWindow = (Shell)this.Shell;

            //Application.Current.MainWindow?.Show();

            ((RadWindow)this.Shell).Show();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
            {
                var viewName = viewType.FullName;
                viewName = viewName?.Replace(".Views.", ".ViewModels.");
                var suffix = viewName != null && viewName.EndsWith("View") ? "Model" : "ViewModel";
                var viewModelName = String.Format(CultureInfo.InvariantCulture, "{0}{1}", viewName, suffix);

                var assembly = viewType.GetTypeInfo().Assembly;
                var type = assembly.GetType(viewModelName, true);

                return type;
            });


            //ViewModelLocationProvider.SetDefaultViewModelFactory(type => Container.Resolve(type));
        }

        //protected override void InitializeModules()
        //{
        //    base.InitializeModules();
        //    Thread.Sleep(50);
        //}
    }
}
