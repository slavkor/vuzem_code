package si.ismvuzem.contructor;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;

import java.util.ArrayList;

import si.ismvuzem.model.Project;

public class ViewAdapter<T> {

    private ArrayList<T> data;
    private Context context;
    private LayoutInflater layoutInflater;
    private ViewAdapter.ItemClickListener itemClickListener;


    // convenience method for getting data at click position
    T getItem(int id) {
        return data.get(id);
    }
    // allows clicks events to be caught

    void setClickListener(ViewAdapter.ItemClickListener itemClickListener) {
        this.itemClickListener = itemClickListener;
    }

    public interface ItemClickListener {
        void onItemClick(View view, int position);
    }
}
