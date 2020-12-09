package si.ismvuzem.model;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class Token {
    @SerializedName("access_token")
    @Expose
    public String AccessToken;

    @SerializedName("expires_in")
    @Expose
    public int ExpiresIn;

    @SerializedName("token_type")
    @Expose
    public String TokenType;

    @SerializedName("refresh_token")
    @Expose
    public String RefreshToken;


    public String getAccessToken() {
        return AccessToken;
    }

    public void setAccessToken(String accessToken) {
        AccessToken = accessToken;
    }

    public int getExpiresIn() {
        return ExpiresIn;
    }

    public void setExpiresIn(int expiresIn) {
        ExpiresIn = expiresIn;
    }

    public String getTokenType() {
        return TokenType;
    }

    public void setTokenType(String tokenType) {
        TokenType = tokenType;
    }

    public String getRefreshToken() {
        return RefreshToken;
    }

    public void setRefreshToken(String refreshToken) {
        RefreshToken = refreshToken;
    }


}
