package si.ismvuzem.authentication;

import android.accounts.Account;
import android.accounts.AccountManager;
import android.accounts.AccountManagerFuture;
import android.accounts.AuthenticatorException;
import android.accounts.OperationCanceledException;
import android.app.Application;
import android.os.Bundle;
import android.util.Log;

import java.io.IOException;

import okhttp3.Interceptor;
import okhttp3.Request;
import okhttp3.Response;
import si.ismvuzem.contructor.MainActivity;

import static si.ismvuzem.authentication.AccountGeneral.ACCOUNT_TYPE;
import static si.ismvuzem.authentication.AccountGeneral.AUTHTOKEN_TYPE_FULL_ACCESS;
import static si.ismvuzem.authentication.AccountGeneral.USERDATA_USER;

public class AuthStatusInterceptor implements Interceptor {

    private String token;
    private AccountManager accountManager;
    private Account account;


    public AuthStatusInterceptor(AccountManager accountManager, Account account){
        this.accountManager = accountManager;
        this.account = account;
    }

    @Override
    public Response intercept(Chain chain) throws IOException {
        Request request = chain.request();
        Response response= chain.proceed(request);

        // todo deal with the issues the way you need to
        if (response.code() == 401) {
            response.close();

            this.token = accountManager.peekAuthToken(account, AUTHTOKEN_TYPE_FULL_ACCESS);
            accountManager.invalidateAuthToken(ACCOUNT_TYPE, this.token);
/*
            final AccountManagerFuture<Bundle> future = accountManager.getAuthToken(account, AUTHTOKEN_TYPE_FULL_ACCESS, null, null, future1 -> {
                Bundle bnd = null;
                try {
                    bnd = future1.getResult();
                    this.token = bnd.getString(AccountManager.KEY_AUTHTOKEN);
                } catch (Exception e) {
                    Log.e("TAG", "run: ",e );
                }
            }, null);

            try {
                Bundle bb = future.getResult();
                this.token = bb.getString(AccountManager.KEY_AUTHTOKEN);
                request = chain.call().clone().request();
                request = request.newBuilder()
                        .addHeader("Authorization", "Bearer " + this.token)
                        .addHeader("appid", "ism.constructor")
                        .addHeader("deviceplatform", "android")
                        .removeHeader("User-Agent")
                        .addHeader("User-Agent", "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:38.0) Gecko/20100101 Firefox/38.0")
                        .build();
                response = chain.proceed(request);
                return response;

            } catch (AuthenticatorException e) {
                e.printStackTrace();
            } catch (OperationCanceledException e) {
                e.printStackTrace();
            }
            */


            try {
                this.token = accountManager.blockingGetAuthToken(account, AUTHTOKEN_TYPE_FULL_ACCESS, false );
                request = chain.call().clone().request();
                request = request.newBuilder()
                        .addHeader("Authorization", "Bearer " + this.token)
                        .addHeader("appid", "ism.constructor")
                        .addHeader("deviceplatform", "android")
                        .removeHeader("User-Agent")
                        .addHeader("User-Agent", "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:38.0) Gecko/20100101 Firefox/38.0")
                        .build();
                response = chain.proceed(request);
                return response;

            } catch (AuthenticatorException e) {
                e.printStackTrace();
            } catch (OperationCanceledException e) {
                e.printStackTrace();
            }


        }

        return response;
    }
}
