package si.ismvuzem.contructor.service.model;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class User {
    @SerializedName("username")
    @Expose
    public String Username;

    @SerializedName("employeeId")
    @Expose
    public String EmployeeId;

    @SerializedName("companyId")
    @Expose
    public String CompanyId;

    public User(String username, String employeeId, String companyId) {
        Username = username;
        EmployeeId = employeeId;
        CompanyId = companyId;
    }

    public String getUsername() {return Username;}

    public void setUsername(String username) {
        Username = username;
    }

    public String getEmployeeId() {return EmployeeId;}

    public void setEmployeeId(String employeeId) { EmployeeId = employeeId; }

    public String getCompanyId() { return CompanyId;}

    public void setCompanyId(String companyId) { CompanyId = companyId;}
}
