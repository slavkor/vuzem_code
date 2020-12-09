package si.ismvuzem.contructor.viewmodel;

import android.app.Application;

import androidx.annotation.NonNull;
import androidx.databinding.BaseObservable;
import androidx.databinding.Bindable;
import androidx.databinding.ObservableField;
import androidx.lifecycle.AndroidViewModel;
import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import si.ismvuzem.contructor.BR;
import si.ismvuzem.contructor.service.model.Token;
import si.ismvuzem.contructor.service.model.UserCredentials;
import si.ismvuzem.contructor.service.repository.AuthRepository;

public class AuthViewModel extends AndroidViewModel {
    private final LiveData<Token> token;

    private String userName;
    private String password;
    private int busy = 8;
    public final ObservableField<String> errorPassword = new ObservableField<>();
    public final ObservableField<String> errorEmail = new ObservableField<>();

    public AuthViewModel(Application application, UserCredentials credentials){
        super(application);
        // If any transformation is needed, this can be simply done by Transformations class ...
        token = AuthRepository.getInstance().GetToken(credentials);
    }

    public LiveData<Token> getToken(){
        return  token;
    }
}

