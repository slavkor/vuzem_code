package si.ismvuzem.contructor.service.repository;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;
import si.ismvuzem.contructor.service.model.Token;
import si.ismvuzem.contructor.service.model.User;
import si.ismvuzem.contructor.service.model.UserCredentials;

public class AuthRepository {

    private IsmAuthService service;
    private static AuthRepository authRepository;

    public AuthRepository(){
        //TODO this gitHubService instance will be injected using Dagger in part #2 ...
        Retrofit retrofit = new Retrofit.Builder()
                .baseUrl(IsmAuthService.AUTH_SERVICE_URL)
                .addConverterFactory(GsonConverterFactory.create())
                .build();

        service = retrofit.create(IsmAuthService.class);
    }
    public synchronized static  AuthRepository getInstance(){
        //TODO No need to implement this singleton in Part #2 since Dagger will handle it ...
        if(authRepository == null)
        {
            authRepository = new AuthRepository();
        }
        return authRepository;
    }

    public LiveData<Token> GetToken(UserCredentials credentials) {
        final MutableLiveData<Token> data = new MutableLiveData<>();
        service.RequestAccess(credentials).enqueue(new Callback<Token>() {
            @Override
            public void onResponse(Call<Token> call, Response<Token> response) {
                data.setValue(response.body());
            }

            @Override
            public void onFailure(Call<Token> call, Throwable t) {
                // TODO better error handling in part #2 ...
                data.setValue(null);
            }
        });
        return data;
    }

}
