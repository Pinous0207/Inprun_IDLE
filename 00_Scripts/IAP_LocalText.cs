using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IAP_LocalText : MonoBehaviour
{
    // App Tracking Transparency (ATT) - iOS 14 
    // 고유 광고 식별자(IDFA : identifier for Advertisers)
    // 사용자의 IDFA를 사용 할 때 앱이 사용자로부터 명시적인 추적 동의를 받는 내용.
    public string productID;

    private void Start()
    {
        InitIAPText();
    }

    private void InitIAPText()
    {
        if (GetComponent<TextMeshProUGUI>() != null)
        {
            GetComponent<TextMeshProUGUI>().text = string.Format("{0} {1}",
                Base_Mng.IAP.GetProduct(productID).metadata.localizedPrice, 
                Base_Mng.IAP.GetProduct(productID).metadata.isoCurrencyCode); 
        }
        else
        {
            GetComponent<Text>().text = string.Format("{0} {1}",
                Base_Mng.IAP.GetProduct(productID).metadata.localizedPrice,
                Base_Mng.IAP.GetProduct(productID).metadata.isoCurrencyCode);
        }
    }
}
