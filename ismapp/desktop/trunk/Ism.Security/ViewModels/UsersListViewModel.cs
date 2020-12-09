using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ism.Infrastructure.Events;
using Ism.Infrastructure.Interaction;
using Ism.Infrastructure.Model;
using Ism.Infrastructure.Repository;
using Ism.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using Ism.Infrastructure.Mvvm;

namespace Ism.Security.ViewModels
{
    public class UsersListViewModel: ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IExceptionService _exceptionService;
        private readonly Uri _baseUriAuth;
        private readonly Uri _baseUriApi;
        private User _user;
        private List<User> _users;
        private List<Company> _companies;

        public UsersListViewModel(ISettingsService settingsService, ISecurityService securityService, IExceptionService exceptionService)
        {
            _settingsService = settingsService;
            _securityService = securityService;
            _exceptionService = exceptionService;
            try
            {
                _baseUriAuth = settingsService.GetAuthServer();
                _baseUriApi = settingsService.GetApiServer();
                AddUserCommand = new DelegateCommand(OnAddUserCommand);
                EditUserCommand = new DelegateCommand<User>(OnEditUserCommand);
                //UserAddRequest = new InteractionRequest<LoginConfirmation>();
                UserEditInteractionRequest = new InteractionRequest<EditInteraction<User>>();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        public DelegateCommand AddUserCommand { get; }
        public DelegateCommand<User> EditUserCommand { get; }
        public InteractionRequest<EditInteraction<User>> UserEditInteractionRequest { get; }

        public User SelectedUser
        {
            get { return _user; }
            set
            {
                SetProperty(ref _user, value);
            }
        }

        public List<User> Users
        {
            get { return _users; }
            set
            {
                SetProperty(ref _users, value);

            }
        }

        public List<Company> Companies
        {
            get { return _companies; }
            set
            {
                SetProperty(ref _companies, value);

            }
        }


        private void OnAddCompanyCommand()
        { }

        private void OnEditUserCommand(User user)
        {
            try
            {
                var interaction = new EditInteraction<User>() { EditMode = Infrastructure.EditMode.Edit, InteractionObject = user, Title = "Urejanje uporabnika" };
                UserEditInteractionRequest.Raise(interaction, OnUserEditInteractionCallback);
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }
        private void OnAddUserCommand()
        {
            try
            {
                var interaction = new EditInteraction<User>() { EditMode  = Infrastructure.EditMode.New ,InteractionObject = null, Title =  "Dodajanje uporabnika" };
                UserEditInteractionRequest.Raise(interaction, OnUserEditInteractionCallback);
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }

        private void OnUserEditInteractionCallback(EditInteraction<User> obj)
        {
            try
            {
                RefreshUsers();
            }
            catch (Exception exc)
            {
                _exceptionService.RaiseException(exc);
            }
        }


        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                RefreshUsers();
            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
        }


        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Users = null;
        }

        #endregion

        private void RefreshUsers()
        {
            try
            {
                using (var rep = _serviceLocator.GetInstance<IRestRepository<List<User>, string>>())
                {
                    rep.GetRequestAsync(new Uri(_settingsService.GetAuthServer(), "/users/list").ToString(), _securityService.GetCurrentToken(), (u) =>
                    {
                        Users = u;
                    });
                }


                _regionManager.RequestNavigate("CompaniesRegion", "CompanyList");


            }
            catch (Exception e)
            {
                _exceptionService.RaiseException(e);
            }
            
        }
    }
}
