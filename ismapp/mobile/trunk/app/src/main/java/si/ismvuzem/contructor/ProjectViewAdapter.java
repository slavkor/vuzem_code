package si.ismvuzem.contructor;

import android.content.Context;
//import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import androidx.cardview.widget.CardView;
import androidx.recyclerview.widget.RecyclerView;

import java.util.ArrayList;

import si.ismvuzem.model.Project;

//RecyclerView.Adapter<ProjectViewAdapter.ViewHolder>
public class ProjectViewAdapter extends RecyclerView.Adapter<ProjectViewAdapter.ViewHolder>  {
    private static final String TAG = "ProjectViewAdapter";

    private ArrayList<Project> mProjects;
    private Context mContext;
    private LayoutInflater mInflater;
    private ItemClickListener mClickListener;

    public ProjectViewAdapter(ArrayList<Project> projects, Context context) {
        this.mProjects = projects;
        this.mContext = context;
        this.mInflater = LayoutInflater.from(context);
    }

    @Override
    public ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

        View view  = LayoutInflater.from(parent.getContext()).inflate(R.layout.layout_listitem, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        Log.d(TAG, "onBindViewHolder: called");

        holder.name.setText(mProjects.get(position).getName());
        holder.description.setText(mProjects.get(position).getDescription());
    }

    @Override
    public int getItemCount() {
        return mProjects == null ? 0 : mProjects.size();
    }


    public class ViewHolder extends RecyclerView.ViewHolder implements View.OnClickListener {

        TextView name;
        TextView description;
        CardView parent_layout;

        public ViewHolder(View itemView) {
            super(itemView);


            name = (TextView) itemView.findViewById(R.id.name);
            description = (TextView) itemView.findViewById(R.id.description);
            parent_layout = (CardView) itemView.findViewById(R.id.parent_layout);


            itemView.setOnClickListener(this);
        }

        @Override
        public void onClick(View v) {
            if (mClickListener != null) mClickListener.onItemClick(v, getAdapterPosition());
        }
    }
    // convenience method for getting data at click position
    Project getItem(int id) {
        return mProjects.get(id);
    }
    // allows clicks events to be caught

    void setClickListener(ItemClickListener itemClickListener) {
        this.mClickListener = itemClickListener;
    }

    // parent activity will implement this method to respond to click events
    public interface ItemClickListener {
        void onItemClick(View view, int position);
    }


}
