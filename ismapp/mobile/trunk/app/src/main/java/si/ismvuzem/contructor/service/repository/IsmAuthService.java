package si.ismvuzem.contructor.service.repository;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.POST;

import si.ismvuzem.contructor.service.model.Token;
import si.ismvuzem.model.User;
import si.ismvuzem.contructor.service.model.UserCredentials;

public interface IsmAuthService {

    public static final String AUTH_SERVICE_URL="";

    @POST("access")
    Call<Token> RequestAccess(@Body UserCredentials credentials);

    @GET("access/get/user")
    Call<User> TokenData();

}
