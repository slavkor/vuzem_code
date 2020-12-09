package si.ismvuzem.contructor.view.adapter;

import android.view.LayoutInflater;
import android.view.ViewGroup;

import androidx.annotation.Nullable;
import androidx.databinding.DataBindingUtil;
import androidx.recyclerview.widget.DiffUtil;
import androidx.recyclerview.widget.RecyclerView;

import java.util.List;
import java.util.Objects;

import si.ismvuzem.contructor.R;
import si.ismvuzem.contructor.databinding.FragmentProjectListBinding;
import si.ismvuzem.contructor.service.model.Project;
import si.ismvuzem.contructor.view.callback.ProjectClickCallback;

public class ProjectAdapter extends RecyclerView.Adapter<ProjectAdapter.ProjectViewHolder> {

        List<? extends Project> projectList;

@Nullable
private final ProjectClickCallback projectClickCallback;

public ProjectAdapter(@Nullable ProjectClickCallback projectClickCallback) {
        this.projectClickCallback = projectClickCallback;
        }

public void setProjectList(final List<? extends Project> projectList) {
        if (this.projectList == null) {
        this.projectList = projectList;
        notifyItemRangeInserted(0, projectList.size());
        } else {
        DiffUtil.DiffResult result = DiffUtil.calculateDiff(new DiffUtil.Callback() {
@Override
public int getOldListSize() {
        return ProjectAdapter.this.projectList.size();
        }

@Override
public int getNewListSize() {
        return projectList.size();
        }

@Override
public boolean areItemsTheSame(int oldItemPosition, int newItemPosition) {
        return ProjectAdapter.this.projectList.get(oldItemPosition).ProjectNumber ==
                projectList.get(newItemPosition).ProjectNumber;
        }

@Override
public boolean areContentsTheSame(int oldItemPosition, int newItemPosition) {
        Project project = projectList.get(newItemPosition);
        Project old = projectList.get(oldItemPosition);
        return project.ProjectNumber == old.ProjectNumber
        && Objects.equals(project.ProjectNumber, old.ProjectNumber);
        }
        });
        this.projectList = projectList;
        result.dispatchUpdatesTo(this);
        }
        }

@Override
public ProjectViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

        FragmentProjectListBinding binding = DataBindingUtil
        .inflate(LayoutInflater.from(parent.getContext()), R.layout.fragment_project_list,
        parent, false);

        //binding.setCallback(projectClickCallback);
        return new ProjectViewHolder(binding);

        }

@Override
public void onBindViewHolder(ProjectViewHolder holder, int position) {

        holder.binding.setPorject(projectList.get(position));
        holder.binding.executePendingBindings();
}

@Override
public int getItemCount() {
        return projectList == null ? 0 : projectList.size();
        }

static class ProjectViewHolder extends RecyclerView.ViewHolder {

        final FragmentProjectListBinding binding;

        public ProjectViewHolder(FragmentProjectListBinding binding) {
                super(binding.getRoot());
                this.binding = binding;
        }


}
}