using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Services;
using Ism.Infrastructure.Repository;
using Prism.Commands;
using System.Xml.Linq;

namespace Ism.Construction.ViewModels
{
    public class EwrOptionsViewModel : ViewModelBase
    {
        private readonly ISettingsService _settings;
        private readonly ISecurityService _security;
        private readonly IExceptionService _exceptionService;
        private ForemanConstructionSite _siteData;
        private Ewr _erw;

        public EwrOptionsViewModel(ISettingsService settings, ISecurityService security, IExceptionService exceptionService)
        {
            _settings = settings;
            _security = security;
            _exceptionService = exceptionService;

            try
            {
                ListCommand = new DelegateCommand(OnListCommand);
                AddCommand = new DelegateCommand(OnAddCommand);
                EditCommand = new DelegateCommand(OnEditCommand, CanExecuteEditCommand);

                _eventAggregator.GetEvent<SelectedEvent<Ewr>>().Subscribe(OnEwrSelectedEvent);
                _eventAggregator.GetEvent<EditEvent<Ewr>>().Subscribe(OnEwrEditEvetn);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }



        public Ewr Ewr { get; set; }
        public Project Project { get; set; }
        public DelegateCommand ListCommand { get; }
        public DelegateCommand AddCommand { get; }
        public DelegateCommand EditCommand { get; }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);
                Project = navigationContext.Parameters["project"] as Project;

            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
        public override bool KeepAlive => false;

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
        }

        private void NavigaionCallback(NavigationResult navigationResult)
        {
            try
            {
                var b = !navigationResult.Result;
                if (b != null && (bool)b)
                {
                    _exceptionService.RaiseException(navigationResult.Error);
                }
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
                XDocument xdoc = XDocument.Parse(meta);
                XElement element = xdoc.Element("root").Element("contextparams").Element("projectId");
                if (element != null) element.Value = Project.UuId;
                element = xdoc.Element("root").Element("contextparams").Element("ewrId");
                if (element != null) element.Value = Ewr.UuId;

                callback?.Invoke(xdoc.ToString());
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

        }

        private void ReportDocumentAction(Report report, Document document)
        {
            try
            {
                if (null == Ewr) return;

                AddDocument<Ewr> addDocument = new AddDocument<Ewr>(Ewr, document);
                using (var rep = _serviceLocator.GetInstance<IRestRepository<Ewr, AddDocument<Ewr>>>())
                {
                    rep.PostRequestAsync(new Uri(_settings.GetApiServer(), "ewr/addDocument").ToString(), addDocument, (e) => {});
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }

        }
        private void OnListCommand()
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<BaseModel>() { Header = "Seznam dodatnih del" });
                parameters.Add("project", Project);
                _regionManager.RequestNavigate(Infrastructure.RegionNames.EwrRegion, "EwrList", NavigaionCallback, parameters);

                parameters = new NavigationParameters();
                parameters.Add("context", "Foreman.EwrList");
                parameters.Add("metaprovider", new Action<string, Action<string>>(ReportMetaDataProvider));
                parameters.Add("documentaction", new Action<Report, Document>(ReportDocumentAction));
                _regionManager.RequestNavigate(RegionNames.EwrReportsRegion, "ReportsContext", NavigaionCallback, parameters);

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
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<Ewr>() { Header = "Dodajanje dodatnega dela", EditInteraction = new EditInteraction<Ewr>() { EditMode = EditMode.New, InteractionObject = null } });
                parameters.Add("project", Project);
                _regionManager.RequestNavigate(Ism.Infrastructure.RegionNames.EwrRegion, "EwrEdit", parameters);
                _regionManager.Regions[RegionNames.EwrReportsRegion].RemoveAll();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void OnEditCommand()
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("navigation", new NavigationInteraction<Ewr>() { Header = "Urejanje dodatnega dela", EditInteraction = new EditInteraction<Ewr>() { EditMode =  EditMode.Edit, InteractionObject = Ewr} });
                parameters.Add("project", Project);
                _regionManager.RequestNavigate(Ism.Infrastructure.RegionNames.EwrRegion, "EwrEdit", parameters);


                _regionManager.Regions[RegionNames.EwrReportsRegion].RemoveAll();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private void RaiseCanExecuteChanged()
        {
            try
            {
                EditCommand.RaiseCanExecuteChanged();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }
        private void OnEwrSelectedEvent(SelectedEventArgs<Ewr> obj)
        {
            try
            {
                
                Ewr = obj.SelectedData;
                RaiseCanExecuteChanged();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        private bool CanExecuteEditCommand()
        {
            return Ewr != null;
        }

        private void OnEwrEditEvetn(EditEventArgs<Ewr> obj)
        {
            try
            {
                Ewr = obj.EditObject;
                if (EditCommand.CanExecute())
                    EditCommand.Execute();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
    }
}
