using Ism.Infrastructure.Events;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Mvvm;
using System.Collections.ObjectModel;
namespace Ism.Common.ViewModels
{
    public class OneContactViewModel : ViewModelBase
    {

        private readonly ISecurityService _securityService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;

        private EditInteraction<Contact> _interaction;

        private Contact _contact;

        public OneContactViewModel(ISecurityService securityService, ISettingsService settingsService, IExceptionService exceptionService)
        {
            if (null == securityService)
                throw new ArgumentNullException(nameof(securityService));
            if (null == settingsService)
                throw new ArgumentNullException(nameof(settingsService));

            _securityService = securityService;
            _settingsService = settingsService;
            _exceptionService = exceptionService;

            try
            {
                ListContactsRequest = new InteractionRequest<ListInteraction<Contact>>();
                SelectCommand = new DelegateCommand(OnSelectCommand);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public InteractionRequest<ListInteraction<Contact>> ListContactsRequest{ get; }

        public Contact Contact
        {
            get
            {
                return _contact;
            }
            set
            {
                SetProperty(ref _contact, value);

            }
        }

        public DelegateCommand SelectCommand { get; }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                base.OnNavigatedTo(navigationContext);

                _interaction = (navigationContext.Parameters["navigation"] as NavigationInteraction<Contact>)?.EditInteraction;
                Contact = _interaction.InteractionObject;

                //_interaction?.DataProvider?.Invoke(DataProviderCallback);

            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }

        private void DataProviderCallback(List<Contact> contacts)
        {
            try
            {

            }
            catch (Exception exception)
            {
                _exceptionService.RaiseException(exception);
            }
        }


        #region Helper methods for Contact edit


        #region adding new contact
        #endregion


        #region updating existing contact

        #endregion

        #region delete contact


        private void OnContactCommandDelete(Contact obj)
        {
            try
            {
                _eventAggregator.GetEvent<ConfirmSaveEvent<BaseModel>>().Publish(new ConfirmSaveEventArgs<BaseModel>() { CallBackAction = OnConfirmContactDelete, Title = "ALO", Content = "Želiš izbrisati kontakt zaposlenega?", PayLoad = obj });
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnConfirmContactDelete(bool confirmed, ConfirmSaveEventArgs<BaseModel> args)
        {
            try
            {
                if (!confirmed)
                    return;

                Contact contact = args.PayLoad as Contact;
                if (null == contact) return;

                _interaction.SaveAction.Invoke(contact, EditMode.Delete);

            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        #endregion


        private void OnSelectCommand()
        {
            try
            {
                ListContactsRequest.Raise(new ListInteraction<Contact>() { Title = "Izberi site managera", ListEventArgs = new ListEventArgs<Contact>() { DataProvider = _interaction.DataProvider }, SelectAction = c => { Contact = c; _interaction?.SaveAction?.Invoke(c, _interaction.EditMode); } });
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);

            }
        }

        private void OnListContactsRequestCallback(ListInteraction<Contact> obj)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
