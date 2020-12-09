package si.ismvuzem.contructor;

import android.accounts.Account;
import android.accounts.AccountManager;
import android.accounts.AccountManagerFuture;
import android.accounts.AuthenticatorException;
import android.accounts.OperationCanceledException;
import android.os.Bundle;

import com.auth0.android.jwt.JWT;

import java.io.IOException;

import okhttp3.HttpUrl;
import okhttp3.Interceptor;
import okhttp3.Request;
import okhttp3.Response;

import static si.ismvuzem.authentication.AccountGeneral.AUTHTOKEN_TYPE_FULL_ACCESS;
import static si.ismvuzem.authentication.AccountGeneral.USERDATA_USER;

public class HeaderInterceptor  implements Interceptor {

    private AccountManager accountManager;
    private Account account;
    private String token;
    private String tokenId;
    public HeaderInterceptor(AccountManager accountManager, Account account) throws AuthenticatorException, OperationCanceledException, IOException {
        this.accountManager = accountManager;
        this.account = account;

        AccountManagerFuture<Bundle> f =  this.accountManager.getAuthToken(account, AUTHTOKEN_TYPE_FULL_ACCESS, null, null,null,null);
        Bundle b = f.getResult();
        this.token =  b.getString(AccountManager.KEY_AUTHTOKEN);

        JWT jwt = new JWT(this.token);
        tokenId = jwt.getId();
        accountManager.setUserData(account, USERDATA_USER, jwt.getSubject());
    }

    @Override
    public Response intercept(Chain chain)
            throws IOException {
        Request request = chain.request();
        request = request.newBuilder()
                .addHeader("Authorization", "Bearer " + this.token)
                .addHeader("appid", "ism.constructor")
                .addHeader("deviceplatform", "android")
                .removeHeader("User-Agent")
                .addHeader("User-Agent", "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:38.0) Gecko/20100101 Firefox/38.0")
                .build();

        HttpUrl url = request.url().newBuilder().addQueryParameter("token",this.tokenId).build();
        request = request.newBuilder().url(url).build();

        Response response = chain.proceed(request);

        return response;
    }
}
