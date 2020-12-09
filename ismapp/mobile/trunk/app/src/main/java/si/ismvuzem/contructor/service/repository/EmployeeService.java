package si.ismvuzem.contructor.service.repository;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Path;
import si.ismvuzem.contructor.service.model.Project;

public interface EmployeeService {

    String HTTP_API_URL = "http://api.github.com/";

    @GET("mobile/user/{user}/project/list")
    Call<List<Project>> GetProjects(@Path("user") String user);

    @GET("mobile/user/{user}/csite/list")
    Call<List<Project>> GetConstructionSites(@Path("user") String user);



}
