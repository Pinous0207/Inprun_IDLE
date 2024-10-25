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
                Debug.Log("Firebase 초기화 성공!");
            }
            else
            {
                Debug.Log("Firebase 초기화 실패");
            }
        });
    }
}
