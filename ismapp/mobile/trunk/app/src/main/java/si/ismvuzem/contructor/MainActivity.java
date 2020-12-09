package si.ismvuzem.contructor;

import android.accounts.Account;
import android.accounts.AccountManager;
import android.accounts.AccountManagerFuture;
import android.app.Application;
import android.app.FragmentManager;
import android.os.AsyncTask;
import android.os.Bundle;

import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.ProgressBar;

import androidx.appcompat.app.AppCompatActivity;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;
import androidx.navigation.ui.NavigationUI;

import com.auth0.android.jwt.JWT;
import com.google.android.material.navigation.NavigationView;

import java.sql.Struct;
import java.util.ArrayList;

import okhttp3.OkHttpClient;
import retrofit2.Call;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;
import si.ismvuzem.authentication.AuthStatusInterceptor;
import si.ismvuzem.model.Project;
import si.ismvuzem.model.User;
import si.ismvuzem.restapi.EmployeeService;
import si.ismvuzem.restapi.IsmAuthService;

import static si.ismvuzem.authentication.AccountGeneral.ACCOUNT_TYPE;
import static si.ismvuzem.authentication.AccountGeneral.AUTHTOKEN_TYPE_FULL_ACCESS;
import static si.ismvuzem.authentication.AccountGeneral.BASE_URL_DEV;
import static si.ismvuzem.authentication.AccountGeneral.USERDATA_USER;

public class MainActivity extends AppCompatActivity {
    private static final String TAG = "MainActivity";

    private AccountManager accountManager;
    private String authToken = null;
    private Account account;

    private ArrayList<Project> mProjects;
    private ProgressBar pgsBar;

    private FragmentManager fragmentManager;
    MySpinnerDialog myInstance;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.activity_main);
        setSupportActionBar(findViewById(R.id.toolbar));
        accountManager = accountManager.get(this);
        getTokenForAccountCreateIfNeeded(ACCOUNT_TYPE, AUTHTOKEN_TYPE_FULL_ACCESS);
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        accountManager.invalidateAuthToken(ACCOUNT_TYPE, null);
    }

    @Override
    public boolean isFinishing() {
        return super.isFinishing();
    }

    @Override
    public boolean onSupportNavigateUp() {
        return NavigationUI.navigateUp(Navigation.findNavController(this, R.id.fragment), findViewById(R.id.drawer_layout));
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.options_menu, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()){
            case R.id.action_exit:
                this.finish();
                System.exit(0);
                return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }

    private void getTokenForAccountCreateIfNeeded(String accountType, String authTokenType) {
        Log.d(TAG, "getTokenForAccountCreateIfNeeded: called");

        try {
            final AccountManagerFuture<Bundle> future = accountManager.getAuthTokenByFeatures(accountType, authTokenType, null, this, null, null,
                    callback -> {
                        Bundle bnd = null;
                        try {
                            bnd = callback.getResult();
                            authToken = bnd.getString(accountManager.KEY_AUTHTOKEN);
                            if (authToken != null) {
                                account = accountManager.getAccountsByType(ACCOUNT_TYPE)[0];
                            }

                            NavController navController = Navigation.findNavController(this, R.id.fragment);
                            navController.setGraph(R.navigation.nav_graph);
                            NavigationUI.setupWithNavController((NavigationView) findViewById(R.id.nav_vie), navController);
                            NavigationUI.setupActionBarWithNavController(this, navController, findViewById(R.id.drawer_layout));

                        } catch (Exception e) {
                            Log.e(TAG, "run: ",e );
                        }
                    }
                    , null);
        }catch  (Exception e) {
        Log.e(TAG, "run: ",e );
    }
        int a = 0;
    }

}
