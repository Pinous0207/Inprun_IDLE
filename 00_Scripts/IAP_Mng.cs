using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAP_Mng : IStoreListener
{
    public readonly string removeADS = "removeads";
    public readonly string gem01 = "dia300"; // 소모품

    private IStoreController storeController; // 구매 과정을 제어하는 함수 제공자
    private IExtensionProvider storeExtensionProvider; // 여러 플랫폼을 위한 확정 처리 제공자

    public void InitUnityIAP()
    {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(gem01, ProductType.Consumable, new IDs() { { gem01, GooglePlay.Name} });
        builder.AddProduct(removeADS, ProductType.NonConsumable, new IDs() { { removeADS, GooglePlay.Name } });

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("초기화에 성공하였습니다.");

        storeController = controller;
        storeExtensionProvider = extensions;
    }

    void IStoreListener.OnInitializeFailed(UnityEngine.Purchasing.InitializationFailureReason error)
    {
        Debug.LogError("초기화에 실패하였습니다.");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError("초기화에 실패하였습니다.");
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError("구매에 실패하였습니다.");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        Debug.Log(purchaseEvent.purchasedProduct.transactionID);
        Debug.Log(purchaseEvent.purchasedProduct.definition.id);

        string purchaseEventName = purchaseEvent.purchasedProduct.definition.id;
        IAP_Holder iap = (IAP_Holder)Enum.Parse(typeof(IAP_Holder), purchaseEventName);

        Base_Canvas.instance.Get_UI("#Reward");
        Utils.UI_Holder.Peek().GetComponent<UI_Reward>().GetIAPReward(iap);
        return PurchaseProcessingResult.Complete;
    }

    public void Purchase(string productId)
    {
        Product product = storeController.products.WithID(productId);
        if(product != null && product.availableToPurchase)
        {
            storeController.InitiatePurchase(product);
        }
        else
        {
            Debug.Log("상품이 없거나 현재 구매가 불가능합니다.");
        }
    }
}
