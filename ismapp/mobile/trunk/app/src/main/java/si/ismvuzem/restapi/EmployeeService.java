package si.ismvuzem.restapi;
import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Path;
import si.ismvuzem.model.Project;

public interface EmployeeService {

    @GET("mobile/user/{user}/project/list")
    Call<ArrayList<Project>> GetProjects(@Path("user") String user);

    @GET("mobile//project/{project}/details")
    Call<si.ismvuzem.contructor.service.model.Project> GetProjectDetails(@Path("project") String project);

    @GET("mobile/user/{user}/csite/list")
    Call<ArrayList<Project>> GetConstructionSites(@Path("user") String user);

}
