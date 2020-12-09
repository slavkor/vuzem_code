package si.ismvuzem.authentication;


import android.util.Log;

import com.auth0.android.jwt.JWT;
import com.auth0.android.jwt.Claim;
import com.auth0.android.jwt.DecodeException;

import retrofit2.Call;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;
import si.ismvuzem.model.Token;
import si.ismvuzem.model.UserCredentials;
import si.ismvuzem.restapi.IsmAuthService;

import static si.ismvuzem.authentication.AccountGeneral.BASE_URL;
import static si.ismvuzem.authentication.AccountGeneral.BASE_URL_DEV;



public class IsmServerAuthenticator implements IServerAuthenticator {
    @Override
    public Token RequestAccess(String user, String pass, String authType) throws Exception {
        Token token = null;
        try {
            Retrofit retrofit = new Retrofit.Builder().baseUrl(BASE_URL_DEV).addConverterFactory(GsonConverterFactory.create()).build();

            IsmAuthService service = retrofit.create(IsmAuthService.class);
            UserCredentials credentials = new UserCredentials("password", "", "abc123", user, pass);
            Call<Token> call = service.RequestAccess(credentials);
            token = call.execute().body();

            //JWT jwt = new JWT(token.AccessToken);

        }
        catch (Exception error){
            Log.e("IsmServerAuthenticator", error.getMessage());
        }
        return token;
    }
}
