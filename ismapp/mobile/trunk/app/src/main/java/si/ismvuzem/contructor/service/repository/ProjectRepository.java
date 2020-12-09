package si.ismvuzem.contructor.service.repository;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;


import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

import si.ismvuzem.contructor.service.model.Project;

public class ProjectRepository {
    private EmployeeService service;
    private static ProjectRepository projectRepository;

    private ProjectRepository(){
        Retrofit retrofit = new Retrofit.Builder()
                .baseUrl(EmployeeService.HTTP_API_URL)
                .addConverterFactory(GsonConverterFactory.create())
                .build();

        service = retrofit.create(EmployeeService.class);
    }

    public synchronized static ProjectRepository getInstance() {
        //TODO No need to implement this singleton in Part #2 since Dagger will handle it ...
        if (projectRepository == null) {
            if (projectRepository == null) {
                projectRepository = new ProjectRepository();
            }
        }
        return projectRepository;
    }

    public LiveData<List<Project>> getProjectList(String userId) {
        final MutableLiveData<List<Project>> data = new MutableLiveData<>();

        service.GetProjects(userId).enqueue(new Callback<List<Project>>() {
            @Override
            public void onResponse(Call<List<Project>> call, Response<List<Project>> response) {
                data.setValue(response.body());
            }

            @Override
            public void onFailure(Call<List<Project>> call, Throwable t) {
                data.setValue(null);
            }
        });


        return data;
    }

    public LiveData<Project> getProjectDetails(String project) {
        final MutableLiveData<Project> data = new MutableLiveData<>();

        /*
        service.GetProjectDetails(project).enqueue(new Callback<Project>() {
            @Override
            public void onResponse(Call<Project> call, Response<Project> response) {
                simulateDelay();
                data.setValue(response.body());
            }

            @Override
            public void onFailure(Call<Project> call, Throwable t) {
                // TODO better error handling in part #2 ...
                data.setValue(null);
            }
        });
*/
        return data;
    }


    private void simulateDelay() {
        try {
            Thread.sleep(10);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
    }
}
