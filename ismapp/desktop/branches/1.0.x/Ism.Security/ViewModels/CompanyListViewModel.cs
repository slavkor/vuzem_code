using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Ism.Infrastructure.Mvvm;

namespace Ism.Security.ViewModels
{
    public class CompanyListViewModel : ViewModelBase, IInteractionRequestAware
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private ListInteraction<Company> _notification;
        private ObservableCollection<Company> _companies;
        private bool _isSelect;
        private bool _isEdit;

        public CompanyListViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
        {

            _settingsService = settingsService;
            _securityService = securityService;
            _exceptionService = exceptionService;

            try
            {
                AddCompanyCommand = new DelegateCommand<Company>(OnAddCompanyCommand);
                CompanyEditRequest = new InteractionRequest<EditInteraction<Company>>();
                SelectCommand = new DelegateCommand<Company>(OnSelectCommand);
                CancelCommand = new DelegateCommand(OnCancelCommand);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public InteractionRequest<EditInteraction<Company>> CompanyEditRequest { get; }
        public DelegateCommand<Company> AddCompanyCommand { get; set; }
        public bool IsSelect
        {
            get { return _isSelect; }
            set
            {
                SetProperty(ref _isSelect, value);
                IsEdit = !value;
            }
        }

        public bool IsEdit
        {
            get { return _isEdit; }
            set
            {
                SetProperty(ref _isEdit, value);
                
            }
        }

        public ObservableCollection<Company> Companies
        {
            get { return _companies; }
            set
            {
                SetProperty(ref _companies, value);
            }
        }

        private void OnCancelCommand()
        {
            try
            {
                _notification.Confirmed = false;
                FinishInteraction?.Invoke();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnSelectCommand(Company obj)
        {
            try
            {
                _notification.Confirmed = true;
                _notification.InteractionObject = obj;
                FinishInteraction?.Invoke();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public DelegateCommand<Company> SelectCommand { get; }
        public DelegateCommand CancelCommand { get; }

        #region IInteractionRequestAware

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                var notification = value as ListInteraction<Company>;
                if (notification != null)
                {
                    _notification = notification;
                    IsSelect = true;
                    RefreshCompanies();
                }
            }
        }

        public Action FinishInteraction { get; set; }

        #endregion

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                IsSelect = false;
                RefreshCompanies();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }


        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            try
            {
                IsSelect = false;
                Companies = null;
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        #endregion

        private void RefreshCompanies()
        {
            try
            {
                var common = _serviceLocator.TryResolve<ICommonService>();
                if (common == null) return;

                Companies = new ObservableCollection<Company>(common.GetCompanies());
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
       
        

        private void OnAddCompanyCommand(Company obj)
        {
            try
            {
                var interaction = new EditInteraction<Company>();
                if (null == obj)
                {
                    interaction.Title = "Dodajanje novega podjetja";
                    interaction.EditMode = EditMode.New;
                }
                else
                {
                    interaction.Title = "Urejanje podjetja";
                    interaction.TitleExtendet = obj.ShortName;
                    interaction.EditMode = EditMode.Edit;
                    interaction.InteractionObject = obj;
                }
                CompanyEditRequest.Raise(interaction);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

    }
}
