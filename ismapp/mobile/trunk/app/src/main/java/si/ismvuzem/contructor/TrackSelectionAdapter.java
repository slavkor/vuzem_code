package si.ismvuzem.contructor;

import android.view.KeyEvent;
import android.view.View;

import androidx.recyclerview.widget.RecyclerView;

public abstract class TrackSelectionAdapter<VH extends TrackSelectionAdapter.ViewHolder> extends RecyclerView.Adapter<VH> {
    // Start with first item selected
    private int selectedItem = 0;
    RecyclerView mRecyclerView = null;
    @Override
    public void onAttachedToRecyclerView(final RecyclerView recyclerView) {
        super.onAttachedToRecyclerView(recyclerView);

        mRecyclerView = recyclerView;

        // Handle key up and key down and attempt to move selection
        recyclerView.setOnKeyListener(new View.OnKeyListener() {
            @Override
            public boolean onKey(View v, int keyCode, KeyEvent event) {
                RecyclerView.LayoutManager lm = recyclerView.getLayoutManager();

                // Return false if scrolled to the bounds and allow focus to move off the list
                if (event.getAction() == KeyEvent.ACTION_DOWN) {
                    if (keyCode == KeyEvent.KEYCODE_DPAD_DOWN) {
                        return tryMoveSelection(lm, 1);
                    } else if (keyCode == KeyEvent.KEYCODE_DPAD_UP) {
                        return tryMoveSelection(lm, -1);
                    }
                }

                return false;
            }
        });
    }

    private boolean tryMoveSelection(RecyclerView.LayoutManager lm, int direction) {
        int nextSelectItem = selectedItem + direction;

        // If still within valid bounds, move the selection, notify to redraw, and scroll
        if (nextSelectItem >= 0 && nextSelectItem  < getItemCount()) {
            notifyItemChanged(selectedItem);
            selectedItem = nextSelectItem;
            notifyItemChanged(selectedItem);
            lm.scrollToPosition(selectedItem);
            return true;
        }

        return false;
    }

    @Override
    public void onBindViewHolder(VH viewHolder, int position) {
        // Set selected state; use a state list drawable to style the view
        viewHolder.itemView.setSelected(selectedItem == position);
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        public ViewHolder(View itemView) {
            super(itemView);

            // Handle item click and set the selection
            itemView.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    // Redraw the old selection and the new
                    notifyItemChanged(selectedItem);
                    selectedItem = mRecyclerView.getChildPosition(v);
                    notifyItemChanged(selectedItem);
                }
            });
        }
    }
}