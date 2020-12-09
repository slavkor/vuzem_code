package si.ismvuzem.model;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class Project {
    @SerializedName("name")
    @Expose
    public String Name;

    @SerializedName("$description")
    @Expose
    public String Description;

    @SerializedName("state")
    @Expose
    public String State;

    @SerializedName("externalnumber")
    @Expose
    public String ExternalNumber;

    @SerializedName("projectnumber")
    @Expose
    public String ProjectNumber;

    @SerializedName("estimatedhours")
    @Expose
    public int EstimatedHours;

    @SerializedName("estimatedvalue")
    @Expose
    public float EstimatedValue;

    @SerializedName("estimatedworkers")
    @Expose
    public Integer EstimatedWorkers;

    public Project(String name, String description, String state, String externalNumber, String projectNumber, int estimatedHours, float estimatedValue, Integer estimatedWorkers) {
        Name = name;
        Description = description;
        State = state;
        ExternalNumber = externalNumber;
        ProjectNumber = projectNumber;
        EstimatedHours = estimatedHours;
        EstimatedValue = estimatedValue;
        EstimatedWorkers = estimatedWorkers;
    }

    public String getName() {
        return Name;
    }

    public void setName(String name) {
        Name = name;
    }

    public String getDescription() {
        return Description;
    }

    public void setDescription(String description) {
        Description = description;
    }

    public String getState() {
        return State;
    }

    public void setState(String state) {
        State = state;
    }

    public String getExternalNumber() {
        return ExternalNumber;
    }

    public void setExternalNumber(String externalNumber) {
        ExternalNumber = externalNumber;
    }

    public String getProjectNumber() {
        return ProjectNumber;
    }

    public void setProjectNumber(String projectNumber) {
        ProjectNumber = projectNumber;
    }

    public int getEstimatedHours() {
        return EstimatedHours;
    }

    public void setEstimatedHours(int estimatedHours) {
        EstimatedHours = estimatedHours;
    }

    public float getEstimatedValue() {
        return EstimatedValue;
    }

    public void setEstimatedValue(float estimatedValue) {
        EstimatedValue = estimatedValue;
    }

    public Integer getEstimatedWorkers() {
        return EstimatedWorkers;
    }

    public void setEstimatedWorkers(Integer estimatedWorkers) {
        EstimatedWorkers = estimatedWorkers;
    }
}
