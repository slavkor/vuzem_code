package si.ismvuzem.authentication;

import android.accounts.Account;
import android.accounts.AccountAuthenticatorActivity;
import android.accounts.AccountManager;
import android.annotation.SuppressLint;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.widget.TextView;
import android.widget.Toast;

import si.ismvuzem.contructor.R;
import si.ismvuzem.model.Token;

import static si.ismvuzem.authentication.AccountGeneral.USERDATA_USER;
import static si.ismvuzem.authentication.AccountGeneral.sServerAuthenticate;

public class AuthenticatorActivity extends AccountAuthenticatorActivity {
    public final static String ARG_ACCOUNT_TYPE = "ACCOUNT_TYPE";
    public final static String ARG_AUTH_TYPE = "AUTH_TYPE";
    public final static String ARG_IS_ADDING_NEW_ACCOUNT = "IS_ADDING_ACCOUNT";
    public static final String KEY_ERROR_MESSAGE = "ERR_MSG";
    public final static String PARAM_USER_PASS = "USER_PASS";

    private final String TAG = this.getClass().getSimpleName();
    private AccountManager mAccountManager;
    private String mAuthTokenType;
    private UserAccessTask accessTask;
    private String userName;
    private String userPass;
    private String accountType;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_authenticator);
        mAccountManager = AccountManager.get(getBaseContext());
        mAuthTokenType = getIntent().getStringExtra(ARG_AUTH_TYPE);
        if (mAuthTokenType == null)
            mAuthTokenType = AccountGeneral.AUTHTOKEN_TYPE_FULL_ACCESS;
        findViewById(R.id.submit).setOnClickListener(v -> submit());
    }

    @Override
    protected void onResume() {
        super.onResume();
    }

    @SuppressLint("StaticFieldLeak")
    public void submit() {
        if (accessTask != null) { return; }
        accessTask = new UserAccessTask();
        accessTask.execute();
    }

    private void finishLogin(Intent intent) {
        Log.d("Ism", TAG + "> finishLogin");

        String accountName = intent.getStringExtra(AccountManager.KEY_ACCOUNT_NAME);
        String accountPassword = intent.getStringExtra(PARAM_USER_PASS);
        final Account account = new Account(accountName, intent.getStringExtra(AccountManager.KEY_ACCOUNT_TYPE));

        if (getIntent().getBooleanExtra(ARG_IS_ADDING_NEW_ACCOUNT, false)) {
            Log.d("Ism", TAG + "> finishLogin > addAccountExplicitly");
            String authToken = intent.getStringExtra(AccountManager.KEY_AUTHTOKEN);
            String authTokenType = mAuthTokenType;

            // Creating the account on the device and setting the auth token we got
            // (Not setting the auth token will cause another call to the server to authenticate the user)
            mAccountManager.addAccountExplicitly(account, accountPassword, intent.getBundleExtra(AccountManager.KEY_USERDATA));
            mAccountManager.setAuthToken(account, authTokenType, authToken);
            mAccountManager.setUserData(account, USERDATA_USER, accountName);
            //mAccountManager.setPassword(account, accountPassword);
        } else {
            Log.d("Ism", TAG + "> finishLogin > setPassword");
            mAccountManager.setPassword(account, accountPassword);
        }

        setAccountAuthenticatorResult(intent.getExtras());
        setResult(RESULT_OK, intent);
        finish();
    }

    /**
     * Represents an asynchronous login/registration task used to authenticate
     * the user.
     */
    public class UserAccessTask extends AsyncTask<Void, Void, Intent> {

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            userName = ((TextView) findViewById(R.id.accountName)).getText().toString();
            userPass = ((TextView) findViewById(R.id.accountPassword)).getText().toString();
            accountType = getIntent().getStringExtra(ARG_ACCOUNT_TYPE);
        }

        @Override
        protected Intent doInBackground(Void... params) {

            Log.d("Ism", TAG + "> Started authenticating");

            Bundle data = new Bundle();
            try {
                Token token = sServerAuthenticate.RequestAccess(userName, userPass, mAuthTokenType);

                if (null == token) {
                    data.putString(KEY_ERROR_MESSAGE, "Invalid credentials");
                    final Intent res = new Intent();
                    res.putExtras(data);
                    return res;
                }

                data.putString(AccountManager.KEY_ACCOUNT_NAME, userName);
                data.putString(PARAM_USER_PASS, userPass);
                data.putString(AccountManager.KEY_ACCOUNT_TYPE, accountType);
                data.putString(AccountManager.KEY_AUTHTOKEN, token.getAccessToken());

            } catch (Exception e) {
                data.putString(KEY_ERROR_MESSAGE, e.getMessage());
            }
            final Intent res = new Intent();
            res.putExtras(data);
            return res;
        }

        @Override
        protected void onPostExecute(Intent intent) {
            accessTask = null;
            if (intent.hasExtra(KEY_ERROR_MESSAGE)) {
                Toast.makeText(getBaseContext(), intent.getStringExtra(KEY_ERROR_MESSAGE), Toast.LENGTH_SHORT).show();
            } else {
                finishLogin(intent);
            }
        }

        @Override
        protected void onCancelled() {
            accessTask = null;
        }
    }
}
