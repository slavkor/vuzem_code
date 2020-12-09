using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using Ism.Infrastructure.Extensions;
using Ism.Infrastructure;
using Ism.Infrastructure.Mvvm;
using Ism.Infrastructure.Interaction;

namespace Ism.Construction.ViewModels
{
    public class EwrListViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;

        private ObservableCollection<Ewr> _ewrs;
        private Ewr _selected;



        public EwrListViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
        {
            if (null == settingsService)
                throw new ArgumentNullException(nameof(settingsService));

            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));


            _settingsService = settingsService;
            _securityService = securityService;
            _exceptionService = exceptionService;

            try
            {
                DoubleClickCommand = new DelegateCommand<Ewr>(OnDoubleClickCommand, CanExecuteDoubleClickCommand);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }





        private Range LastRange { get; set; }

        private DateTime _date;
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                SetProperty(ref _date, value);
                RefreshEwrs(new Range(Date.FirstDayOfYear(), Date.LastDayOfYear()));
            }
        }

        public Project Project { get; set; }

        private bool CanExecuteDoubleClickCommand(Ewr site)
        {
            return true;
        }

        private void OnDoubleClickCommand(Ewr site)
        {
            try
            {
                _eventAggregator.GetEvent<EditEvent<Ewr>>().Publish(new EditEventArgs<Ewr>() { EditObject = site, EditMode = EditMode.Edit, RefreshAction = s => { RefreshEwrs(LastRange); } });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private DateTime _minDate;

        public DateTime MinDate
        {
            get { return _minDate; }
            set
            {
                SetProperty(ref _minDate, value);
            }
        }

        private DateTime _maxDate;

        public DateTime MaxDate
        {
            get { return _maxDate; }
            set
            {
                SetProperty(ref _maxDate, value);
            }
        }

        public DelegateCommand<Ewr> DoubleClickCommand { get; }

        public ObservableCollection<Ewr> Ewrs { get { return _ewrs; } set { SetProperty(ref _ewrs, value); } }

        public Ewr Selected
        {
            get { return _selected; }
            set
            {
                try
                {
                    SetProperty(ref _selected, value);
                    _eventAggregator.GetEvent<SelectedEvent<Ewr>>().Publish(new SelectedEventArgs<Ewr>(_selected));
                }
                catch (Exception exc)
                {
                    _exceptionService.RaiseException(exc);
                }
            }
        }



        private void RefreshEwrs(Range range)
        {
            try
            {

                Ewrs = null;
                using (var repository = _serviceLocator.GetInstance<IRestRepository<List<Ewr>, Range>>())
                {

                    repository.PostRequestAsync(new Uri(_settingsService.GetApiServer(), $"project/{Project.UuId}/ewr/list").ToString(), range, _securityService.GetCurrentToken(),
                        (e) =>
                        {

                            Ewrs = new ObservableCollection<Ewr>(e.OrderBy(c => c.Number));
                            Selected = null;

                        });
                }
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            Project = navigationContext.Parameters["project"] as Project;
            RefreshEwrs(null);
            //RefreshConstructionSites(new Range(Date, Date.LastDayOfYear()));
        }
        public override bool KeepAlive => false;
    }
}
