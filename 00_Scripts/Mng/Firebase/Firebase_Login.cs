using Firebase.Auth;
using UnityEngine;

public partial class Firebase_Mng
{
    public void GuestLogin()
    {
        if(auth.CurrentUser != null)
        {
            Debug.Log("기기에 로그인된 상태입니다. :" + auth.CurrentUser.UserId);
            ReadData();
            return;
        }
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("게스트 로그인 실패");
                return;
            }
            FirebaseUser user = task.Result.User;
            Debug.Log("게스트 로그인 성공! 사용자 ID : " + user.UserId);
            ReadData();
        });
    }
}
