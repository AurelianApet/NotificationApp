package md548d4bc5d86921120f5fafa7f412dfff9;


public class FirebaseIIDService
	extends com.google.firebase.iid.FirebaseInstanceIdService
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTokenRefresh:()V:GetOnTokenRefreshHandler\n" +
			"";
		mono.android.Runtime.register ("notificationApp.Droid.FirebaseIIDService, notificationApp.Android", FirebaseIIDService.class, __md_methods);
	}


	public FirebaseIIDService ()
	{
		super ();
		if (getClass () == FirebaseIIDService.class)
			mono.android.TypeManager.Activate ("notificationApp.Droid.FirebaseIIDService, notificationApp.Android", "", this, new java.lang.Object[] {  });
	}


	public void onTokenRefresh ()
	{
		n_onTokenRefresh ();
	}

	private native void n_onTokenRefresh ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
