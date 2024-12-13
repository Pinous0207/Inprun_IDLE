using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IAP_LocalText : MonoBehaviour
{
    // App Tracking Transparency (ATT) - iOS 14 
    // ���� ���� �ĺ���(IDFA : identifier for Advertisers)
    // ������� IDFA�� ��� �� �� ���� ����ڷκ��� ������� ���� ���Ǹ� �޴� ����.
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
