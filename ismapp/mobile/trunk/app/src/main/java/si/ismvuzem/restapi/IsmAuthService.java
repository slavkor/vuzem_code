package si.ismvuzem.restapi;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.POST;
import si.ismvuzem.model.Token;
import si.ismvuzem.model.User;
import si.ismvuzem.model.UserCredentials;

public interface IsmAuthService {
    @POST("access")
    Call<Token> RequestAccess(@Body UserCredentials credentials);

}
