package si.ismvuzem.contructor;

import android.accounts.Account;
import android.accounts.AccountManager;
import android.accounts.AccountManagerFuture;
import android.os.AsyncTask;
import android.os.Bundle;

import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

import java.util.ArrayList;

import okhttp3.OkHttpClient;
import retrofit2.Call;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;
import si.ismvuzem.authentication.AuthStatusInterceptor;
import si.ismvuzem.model.Project;
import si.ismvuzem.restapi.EmployeeService;

import static si.ismvuzem.authentication.AccountGeneral.ACCOUNT_TYPE;
import static si.ismvuzem.authentication.AccountGeneral.AUTHTOKEN_TYPE_FULL_ACCESS;
import static si.ismvuzem.authentication.AccountGeneral.USERDATA_USER;

/**
 * A simple {@link Fragment} subclass.
 * Use the {@link HomeFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class HomeFragment extends Fragment implements ProjectViewAdapter.ItemClickListener{
    private static final String TAG = "MainActivity";
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private AccountManager accountManager;
    private String authToken = null;
    private Account account;
    private RecyclerView view = null;
    private ProjectViewAdapter adapter = null;

    private ArrayList<Project> projects = null;

    public HomeFragment() {
        // Required empty public constructor
    }


    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment HomeFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static HomeFragment newInstance(String param1, String param2) {
        HomeFragment fragment = new HomeFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }
        accountManager = accountManager.get(getContext());


    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        //new ProjectsTask().execute(accountManager.getUserData(account, USERDATA_USER));
        //getTokenForAccountCreateIfNeeded(ACCOUNT_TYPE, AUTHTOKEN_TYPE_FULL_ACCESS);

        /*
        try {
            Account[] accounts = accountManager.getAccountsByType(ACCOUNT_TYPE);
            if(accounts.length == 0)
                getTokenForAccountCreateIfNeeded(ACCOUNT_TYPE, AUTHTOKEN_TYPE_FULL_ACCESS);
            else
                account = accountManager.getAccountsByType(ACCOUNT_TYPE)[0];
        }catch (Exception exc){

        }
        */
        //new ProjectsTask().execute(accountManager.getUserData(account, USERDATA_USER));
        return inflater.inflate(R.layout.fragment_home, container, false);
    }


    @Override
    public void onResume() {
        super.onResume();
        try {
            account = accountManager.getAccountsByType(ACCOUNT_TYPE)[0];
        } catch (Exception exc){
            int a = 0;
        }
        try {
            new ProjectsTask().execute(accountManager.getUserData(account, USERDATA_USER));
        } catch (Exception exc){

        }


    }

    @Override
    public void onItemClick(View view, int position) {
        int a = 0;
    }

    private class ProjectsTask extends AsyncTask<String, Integer, ArrayList<Project>>  {

        @Override
        protected void onPreExecute() {
            super.onPreExecute();

        }

        @Override
        protected ArrayList<Project> doInBackground(String... strings) {
            try {
                OkHttpClient.Builder oktHttpClient = new OkHttpClient.Builder();
                oktHttpClient.addInterceptor(new HeaderInterceptor(accountManager, account));
                //oktHttpClient.addInterceptor(new AuthStatusInterceptor(accountManager, account));
                Retrofit retrofit = new Retrofit.Builder().baseUrl("http://192.168.137.122:81").addConverterFactory(GsonConverterFactory.create()).client(oktHttpClient.build()).build();
                EmployeeService service = retrofit.create(EmployeeService.class);
                Call<ArrayList<Project>> call = service.GetConstructionSites(strings[0]);// service.GetProjects(strings[0]);

                return call.execute().body();

            }
            catch (RuntimeException e){
                Log.e(TAG, "doInBackground: ",e );
            }
            catch (Exception e){
                Log.e(TAG, "doInBackground: ",e );
            }
            return  null;
        }

        @Override
        protected void onPostExecute(ArrayList<Project> projects) {
            super.onPostExecute(projects);
            try {
                fetchedProjects(projects);
            }
            catch (Exception e){
                Log.e(TAG, "onPostExecute: ",e );
            }
        }


    }

    private ArrayList<Project> fetchedProjects(ArrayList<Project> projects){
        this.projects = projects;
        view = getView().findViewById(R.id.proj_view);
        view.setLayoutManager(new LinearLayoutManager(getContext()));
        adapter = new ProjectViewAdapter(projects, getContext());
        adapter.setClickListener(this);
        view.setAdapter(adapter);
        return projects;
    }

    private void getTokenForAccountCreateIfNeeded(String accountType, String authTokenType) {
        Log.d(TAG, "getTokenForAccountCreateIfNeeded: called");

        try {
            final AccountManagerFuture<Bundle> future = accountManager.getAuthTokenByFeatures(accountType, authTokenType, null, getActivity(), null, null,
                    callback -> {
                        Bundle bnd = null;
                        try {
                            bnd = callback.getResult();
                            authToken = bnd.getString(accountManager.KEY_AUTHTOKEN);
                            if (authToken != null) {
                                account = accountManager.getAccountsByType(ACCOUNT_TYPE)[0];
                            }
                            new ProjectsTask().execute(accountManager.getUserData(account, USERDATA_USER));



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