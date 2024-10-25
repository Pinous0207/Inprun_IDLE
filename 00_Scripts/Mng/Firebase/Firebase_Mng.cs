using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

public partial class Firebase_Mng
{
    private FirebaseAuth auth;
    private FirebaseUser currentUser;
    private DatabaseReference reference;

    public void Init()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                currentUser = auth.CurrentUser;
                reference = FirebaseDatabase.DefaultInstance.RootReference;

                GuestLogin();
                Debug.Log("Firebase �ʱ�ȭ ����!");
            }
            else
            {
                Debug.Log("Firebase �ʱ�ȭ ����");
            }
        });
    }
}
