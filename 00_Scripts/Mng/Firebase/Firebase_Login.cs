using Firebase.Auth;
using UnityEngine;

public partial class Firebase_Mng
{
    public void GuestLogin()
    {
        if(auth.CurrentUser != null)
        {
            Debug.Log("��⿡ �α��ε� �����Դϴ�. :" + auth.CurrentUser.UserId);
            ReadData();
            return;
        }
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("�Խ�Ʈ �α��� ����");
                return;
            }
            FirebaseUser user = task.Result.User;
            Debug.Log("�Խ�Ʈ �α��� ����! ����� ID : " + user.UserId);
            ReadData();
        });
    }
}
