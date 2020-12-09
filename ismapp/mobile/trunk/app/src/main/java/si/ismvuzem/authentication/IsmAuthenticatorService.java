package si.ismvuzem.authentication;

import android.app.Service;
import android.content.Intent;
import android.os.IBinder;

public class IsmAuthenticatorService  extends Service {

    private static IsmAuthenticator authenticator;
    @Override
    public IBinder onBind(Intent intent) {

        IBinder binder = null;
        if (intent.getAction().equals(android.accounts.AccountManager.ACTION_AUTHENTICATOR_INTENT)) {
            binder = getAuthenticator().getIBinder();
        }
        return binder;
    }

    private IsmAuthenticator getAuthenticator(){
        if(null == authenticator){
            authenticator = new IsmAuthenticator(this);
        }
        return authenticator;
    }
}