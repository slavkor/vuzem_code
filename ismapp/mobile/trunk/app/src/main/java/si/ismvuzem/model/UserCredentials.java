package si.ismvuzem.model;

import androidx.databinding.ObservableField;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class UserCredentials {

    @SerializedName("grant_type")
    @Expose
    public String GrantType;

    @SerializedName("client_id")
    @Expose
    public String ClientId;

    @SerializedName("client_secret")
    @Expose
    public String ClientSecret;

    @SerializedName("username")
    @Expose
    public String Username;

    @SerializedName("password")
    @Expose
    public String Password;


    public UserCredentials(String grantType, String clientId, String clientSecret, String username, String password) {
        GrantType = grantType;
        ClientId = clientId;
        ClientSecret = clientSecret;
        Username = username;
        Password = password;
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

    public String getUsername() {
        return Username;
    }

    public void setUsername(String username) {
        Username = username;
    }

    public String getPassword() {
        return Password;
    }

    public void setPassword(String password) {
        Password = password;
    }
}
