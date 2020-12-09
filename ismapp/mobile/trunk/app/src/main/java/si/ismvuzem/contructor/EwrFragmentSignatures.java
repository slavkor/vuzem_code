package si.ismvuzem.contructor;

import android.os.Bundle;

import androidx.core.content.ContextCompat;
import androidx.fragment.app.Fragment;

import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import com.kyanogen.signatureview.*;



public class EwrFragmentSignatures extends Fragment {
    private SignatureView signatureView;

    public EwrFragmentSignatures() {

    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        /*
        signatureView = getView().findViewById(R.id.signature_view);
        final TypedValue value = new TypedValue();
        getContext().getTheme ().resolveAttribute (R.attr.colorAccent, value, true);
        signatureView.setPenColor(value.data);
        */

    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        return inflater.inflate(R.layout.fragment_ewr_signatures, container, false);


    }
}