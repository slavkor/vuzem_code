package si.ismvuzem.contructor.service.model;

import androidx.databinding.BaseObservable;
import androidx.databinding.Bindable;
import androidx.databinding.ObservableField;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import si.ismvuzem.contructor.BR;

public class UserCredentials  extends BaseObservable {


    @SerializedName("grant_type")
    @Expose
    public String GrantType;

    @SerializedName("client_id")
    @Expose
    public String ClientId;

    @SerializedName("client_secret")
    @Expose
    public String ClientSecret;

    public final ObservableField<String> Username = new ObservableField<>();;
    public final ObservableField<String> Password = new ObservableField<>();;

    private  String userName;
    private  String password;


    public UserCredentials(String grantType, String clientId, String clientSecret, String username, String password) {
        GrantType = grantType;
        ClientId = clientId;
        ClientSecret = clientSecret;
        //Username = username;
        //Password = password;
    }

    public String getGrantType() {
        return GrantType;
    }

    public void setGrantType(String grantType) {
        GrantType = grantType;
    }

    public String getClientId() {
        return ClientId;
    }

    public void setClientId(String clientId) {
        ClientId = clientId;
    }

    public String getClientSecret() {
        return ClientSecret;
    }

    public void setClientSecret(String clientSecret) {
        ClientSecret = clientSecret;
    }

    @SerializedName("username")
    @Bindable
    public String getUsername() {
        return this.userName;
    }

    public void setUsername(String username) {
        this.userName = username; notifyPropertyChanged(BR.username);
    }
    @SerializedName("password")
    @Bindable
    public String getPassword() {
        return this.password;
    }

    public void setPassword(String password) {
        this.password =  password;
        notifyPropertyChanged(BR.password);
    }
}
