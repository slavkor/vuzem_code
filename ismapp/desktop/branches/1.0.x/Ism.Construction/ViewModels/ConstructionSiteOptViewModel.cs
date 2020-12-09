using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using Ism.Infrastructure.Mvvm;

namespace Ism.Construction.ViewModels
{
    public class ConstructionSiteOptViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly IAppCommands _appCommands;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;

        private ConstructionSite _currentConstructionSite;
        private bool _canSave;
        private InteractionRequest<EditInteraction<ConstructionSite>> _constructionSiteEditRequest;

        public ConstructionSiteOptViewModel(ISettingsService settingsService, IAppCommands appCommands, ISecurityService securityService, IExceptionService exceptionService)
        {

            _appCommands = appCommands;
            _securityService = securityService;
            _settingsService = settingsService;
            _exceptionService = exceptionService;
            try
            {
                ProjectEditInteractionRequest = new InteractionRequest<EditInteraction<Project>>();

                _eventAggregator.GetEvent<SelectedEvent<ConstructionSite>>().Subscribe(OnConstructionSiteSelected);
                _eventAggregator.GetEvent<EditEvent<ConstructionSite>>().Subscribe(OnConstructionSiteEditEvent);
                ConstructionSiteEditRequest = new InteractionRequest<EditInteraction<ConstructionSite>>();
                ListCommand = new DelegateCommand(OnListCommand);
                EditCommand = new DelegateCommand<ConstructionSite>(OnEditCommand, CanExecuteEditCommand);

                AddCommand = new DelegateCommand(OnAddCommand, () => _securityService.HasPermission("csite"));
                _eventAggregator.GetEvent<EditChildEvent<ConstructionSite, Project>>().Subscribe(OnProjectEditEvent);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public DelegateCommand<ConstructionSite> EditCommand { get; }



        private void OnProjectEditEvent(EditChildEventArgs<ConstructionSite, Project> args)
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<Project>() { Header = args.EditChildMode == EditMode.New ? "Dodajanje projekta " : "Urejanje projekta ", EditInteraction = new EditInteraction<Project>() { Title = "Urejanje projekta ", InteractionObject = args.EditChildObject, EditMode = args.EditChildMode, SaveAction = args.SaveChildAction } });
                parameters.Add("sitedata", args.EditObject);
                _regionManager.RequestNavigate(Infrastructure.RegionNames.CSiteRegion, "ProjectEditView", parameters);

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void ProjectEditInteractionRequestCallback(EditInteraction<Project> obj)
        {
            throw new NotImplementedException();
        }

        public InteractionRequest<EditInteraction<Project>> ProjectEditInteractionRequest { get; }
        public InteractionRequest<EditInteraction<ConstructionSite>> ConstructionSiteEditRequest
        {
            get { return _constructionSiteEditRequest; }
            set {
                SetProperty(ref _constructionSiteEditRequest, value);
            }
        }

        public DelegateCommand ListCommand { get; }
        public DelegateCommand AddCommand { get; }

        public DelegateCommand<ConstructionSite> SaveCommand { get; }



        public ConstructionSite CurrentConstructionSite
        {
            get { return _currentConstructionSite; }
            set
            {
                SetProperty(ref _currentConstructionSite, value);
                RaiseCanExecuteChanged();
            }
        }

        public bool CanSave
        {
            get { return _canSave; }
            set { SetProperty(ref _canSave, value); }
        }

        

        private void OnListCommand()
        {
            try
            {

                var par = new NavigationParameters();
                par.Add("navigation", new NavigationInteraction<BaseModel>()
                {
                    Header = "Seznam gradbišč",
                });

                _regionManager.RequestNavigate(Infrastructure.RegionNames.CSiteRegion, "ConstructionSitesList", par);

                var parameters = new NavigationParameters();
                parameters.Add("context", "ConstructionSite.List");
                parameters.Add("metaprovider", new Action<string, Action<string>>(ReportMetaDataProvider));
                _regionManager.RequestNavigate(RegionNames.SiteReportsRegion, "ReportsContext", NavigaionCallback, parameters);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void NavigaionCallback(NavigationResult navigationResult)
        {
            try
            {
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void ReportMetaDataProvider(string meta, Action<string> callback)
        {
            try
            {

                callback?.Invoke(meta);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

        }

        private void OnAddCommand()
        {
            try
            {
                try
                {

                    NavigationParameters parameters = new NavigationParameters();
                    parameters.Add("navigation", new NavigationInteraction<ConstructionSite>() { Header = "Dodajanje novega gradbišča", EditInteraction = new EditInteraction<ConstructionSite>() { Title = "Dodajanje novega gradbišča", InteractionObject = null, EditMode = EditMode.New, SaveAction = OnAddSiteSaveCallback} });
                    _regionManager.RequestNavigate(Ism.Infrastructure.RegionNames.CSiteRegion, "ConstructionSiteEdit", parameters);
                }
                catch (Exception exc)
                {
                    _exceptionService.RaiseException(exc);
                } 
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnAddSiteSaveCallback(ConstructionSite site, EditMode editMode)
        {
            try
            {
                if (editMode != EditMode.New) return;
                //EditConstructionSiteCommand.Execute(site);
            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        void ConstructionSiteEditRequestCallback(EditInteraction<ConstructionSite> request)
        {
            try
            {
                //if (request.EditMode == EditMode.Edit)
                //    _eventAggregator.GetEvent<EmployeeEdited>().Publish(request.InteractionObject);
                //else
                //    _eventAggregator.GetEvent<EmployeeAdded>().Publish(request.InteractionObject);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnConstructionSiteSelected(SelectedEventArgs<ConstructionSite> args)
        {
            try
            {
                CurrentConstructionSite = null;
                CurrentConstructionSite = args?.SelectedData;

                
                
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }


        private void OnConstructionSiteEditEvent(EditEventArgs<ConstructionSite> args)
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<ConstructionSite>() { Header = "Urejanje gradbišča", EditInteraction = new EditInteraction<ConstructionSite>() { Title = "Urejanje gradbišča", TitleExtendet = $"{args.EditObject.Name}", InteractionObject = args.EditObject, EditMode = EditMode.Edit, RefreshAction = args.RefreshAction } });
                //foreach (var item in _regionManager.Regions[Infrastructure.RegionNames.CSiteRegion].ActiveViews)
                //{
                //    _regionManager.Regions[Infrastructure.RegionNames.CSiteRegion].Remove(item);
                //} 
                _regionManager.RequestNavigate(Infrastructure.RegionNames.CSiteRegion, "ConstructionSiteEdit", onc, parameters);

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void onc(NavigationResult obj)
        {
            int a = 0;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {

            base.OnNavigatedTo(navigationContext);
            CurrentConstructionSite = null;
        }

        private void OnCallBack(NavigationResult obj)
        {
            int a = 0;
        }

        private bool CanExecuteEditCommand(ConstructionSite site)
        {
            return CurrentConstructionSite != null && CurrentConstructionSite.Company.UuId == _securityService.GetCurrentCompany().UuId;
        }

        private void OnEditCommand(ConstructionSite site)
        {
            try
            {
                _eventAggregator.GetEvent<EditEvent<ConstructionSite>>().Publish(new EditEventArgs<ConstructionSite>() { EditObject = site, EditMode = EditMode.Edit });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }


        private void RaiseCanExecuteChanged()
        {
            try
            {
                EditCommand.RaiseCanExecuteChanged();

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
    }
}
