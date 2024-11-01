using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAP_Mng : IStoreListener
{
    public readonly string removeADS = "removeads";
    public readonly string gem01 = "dia300"; // �Ҹ�ǰ

    private IStoreController storeController; // ���� ������ �����ϴ� �Լ� ������
    private IExtensionProvider storeExtensionProvider; // ���� �÷����� ���� Ȯ�� ó�� ������

    public void InitUnityIAP()
    {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(gem01, ProductType.Consumable, new IDs() { { gem01, GooglePlay.Name} });
        builder.AddProduct(removeADS, ProductType.NonConsumable, new IDs() { { removeADS, GooglePlay.Name } });

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("�ʱ�ȭ�� �����Ͽ����ϴ�.");

        storeController = controller;
        storeExtensionProvider = extensions;
    }

    void IStoreListener.OnInitializeFailed(UnityEngine.Purchasing.InitializationFailureReason error)
    {
        Debug.LogError("�ʱ�ȭ�� �����Ͽ����ϴ�.");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError("�ʱ�ȭ�� �����Ͽ����ϴ�.");
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError("���ſ� �����Ͽ����ϴ�.");
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
            Debug.Log("��ǰ�� ���ų� ���� ���Ű� �Ұ����մϴ�.");
        }
    }
}
