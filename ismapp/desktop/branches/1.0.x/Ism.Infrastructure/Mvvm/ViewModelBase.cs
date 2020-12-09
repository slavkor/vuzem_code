using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Validation;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Mvvm
{
    public abstract class ViewModelBase : ValidatableBindableBase, IRegionMemberLifetime, INavigationAware
    {
        protected IRegionManager _regionManager;
        protected IUnityContainer _container;
        protected IEventAggregator _eventAggregator;
        protected IServiceLocator _serviceLocator;

        protected NavigationInteraction<BaseModel> _navigationInteraction;

        protected NavigationContext _navigationContext;

        protected ViewModelBase()
        {

            _regionManager = ServiceLocator.Current.TryResolve<IRegionManager>();
            _container = ServiceLocator.Current.TryResolve<IUnityContainer>();
            _eventAggregator = ServiceLocator.Current.TryResolve<IEventAggregator>();
            _serviceLocator = ServiceLocator.Current.TryResolve<IServiceLocator>();

        }

        public virtual NavigationInteraction<BaseModel> NavigationInteraction
        {
            get
            {
                return _navigationInteraction;
            }
            set
            {
                SetProperty(ref _navigationInteraction, value);
            }
        }

        #region IRegionMemberLifetime
        public virtual  bool KeepAlive
        {
            get
            {
                return true;
            }
        }
        #endregion

        private string _header;

        public string Header
        {
            get { return _header; }
            set
            {
                SetProperty(ref _header, value);
            }
        }


        #region INavigationAware
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationContext = navigationContext;
            if (!navigationContext.Parameters.Any(k => k.Key == "navigation")) return;
            var navigation = (INavigationInteraction)navigationContext.Parameters["navigation"];
            if (navigation == null) return;
            Header = navigation.Header;

        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
            NavigationInteraction = null;
        }
        #endregion

        public virtual void NavigateBack()
        {
            if (_navigationContext == null) return;
            if (!_navigationContext.NavigationService.Journal.CanGoBack) return;
            _navigationContext.NavigationService.Journal.GoBack();
        }

    }
}
