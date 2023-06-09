using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

// Placing the Purchaser class in the CompleteProject namespace allows it to interact with ScoreManager,
// one of the existing Survival Shooter scripts.
namespace CompleteProject
{
    // Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
    public class Purchaser : Singleton<Purchaser>, IStoreListener
    {
        private static IStoreController m_StoreController; // The Unity Purchasing system.

        private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
        // Product identifiers for all products capable of being purchased:
        // "convenience" general identifiers for use with Purchasing, and their store-specific identifier
        // counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers
        // also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

        // General product identifiers for the consumable, non-consumable, and subscription products.
        // Use these handles in the code to reference which product to purchase. Also use these values
        // when defining the Product Identifiers on the store. Except, for illustration purposes, the
        // kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
        // specific mapping to Unity Purchasing's AddProduct, below.
        public static string kProductIDConsumable = "consumable";

        public static string kProductIDNonConsumable = "nonconsumable";
        public static string kProductIDSubscription = "subscription";

        // Apple App Store-specific product identifier for the subscription product.
        private static readonly string kProductNameAppleSubscription = "com.unity3d.subscription.new";

        public static string AppleproductId0 = "com.icecube.jewelswitch.appstore.free.puzzle.255000coin";
        public static string AppleproductId1 = "com.icecube.jewelswitch.appstore.free.puzzle.142000coin";
        public static string AppleproductId2 = "com.icecube.jewelswitch.appstore.free.puzzle.66000coin";
        public static string AppleproductId3 = "com.icecube.jewelswitch.appstore.free.puzzle.27500coin";
        public static string AppleproductId4 = "com.icecube.jewelswitch.appstore.free.puzzle.11100coin";
        public static string AppleproductId5 = "com.icecube.jewelswitch.appstore.free.puzzle.2000coin";
        public static string AppleproductId6 = "com.icecube.jewelswitch.appstore.free.puzzle.noad";
        public static string AppleproductId7 = "com.icecube.jewelswitch.appstore.free.puzzle.coinbank";
        public static string AppleproductPackageId1 = "com.icecube.jewelswitch.appstore.free.puzzle.package01";
        public static string AppleproductPackageId2 = "com.icecube.jewelswitch.appstore.free.puzzle.package02";
        public static string AppleproductPackageId3 = "com.icecube.jewelswitch.appstore.free.puzzle.package03";
        public static string AppleproductPackageId4 = "com.icecube.jewelswitch.appstore.free.puzzle.package04";
        public static string AppleproductPackageId5 = "com.icecube.jewelswitch.appstore.free.puzzle.package05";

        //public static string AppleproductPackageId6 = "com.icecube.jewelswitch.appstore.free.puzzle.package6";
        //public static string AppleproductPackageId7 = "com.icecube.jewelswitch.appstore.free.puzzle.package7";
        public static string AppleProductLimitedPackage = "com.icecube.jewelswitch.appstore.free.puzzle.limitedpackage";

        // Google Play Store-specific product identifier subscription product.
        private static readonly string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

        public static string GoogleproductId0 = "com.icecube.jewelswitch.android.free.puzzle.255000coin";
        public static string GoogleproductId1 = "com.icecube.jewelswitch.android.free.puzzle.142000coin";
        public static string GoogleproductId2 = "com.icecube.jewelswitch.android.free.puzzle.61000coin";
        public static string GoogleproductId3 = "com.icecube.jewelswitch.android.free.puzzle.27500coin";
        public static string GoogleproductId4 = "com.icecube.jewelswitch.android.free.puzzle.11100coin";
        public static string GoogleproductId5 = "com.icecube.jewelswitch.android.free.puzzle.2000coin";
        public static string GoogleproductId6 = "com.icecube.jewelswitch.android.free.puzzle.noads";
        public static string GoogleproductId7 = "com.icecube.jewelswitch.android.free.puzzle.coinbank";
        public static string GoogleproductPackageId1 = "com.icecube.jewelswitch.android.free.puzzle.package1";
        public static string GoogleproductPackageId2 = "com.icecube.jewelswitch.android.free.puzzle.package2";
        public static string GoogleproductPackageId3 = "com.icecube.jewelswitch.android.free.puzzle.package3";
        public static string GoogleproductPackageId4 = "com.icecube.jewelswitch.android.free.puzzle.package4";
        public static string GoogleproductPackageId5 = "com.icecube.jewelswitch.android.free.puzzle.package5";
        public static string GoogleproductPackageId6 = "com.icecube.jewelswitch.android.free.puzzle.package6";
        public static string GoogleproductPackageId7 = "com.icecube.jewelswitch.android.free.puzzle.package7";
        public static string GoogleproductLimitedPackage = "com.icecube.jewelswitch.android.free.puzzle.package8";
        public List<PackageInfo> packageInfo = new List<PackageInfo>();
        private bool isCoroutine;
        private bool isLoad;
        public Action actPurchase;
        
        public Dictionary<ESubsType, string> SubscriptionProductList = new Dictionary<ESubsType, string>();

        public bool BuyGold_ID6
        {
            get
            {
                var returnValue = false;
                if (PlayerData.GetInstance != null) returnValue = PlayerData.GetInstance.IsAdsFree;
                return returnValue;
            }
        }

        private void Awake()
        {
            //GetInstance = this;
        }

        //
        // --- IStoreListener
        //

        public ShopItemInfo GetPigCoinPrice()
        {
            var itemInfo = new ShopItemInfo();
            Product p = null;
            if (Application.platform == RuntimePlatform.Android)
            {
                p = m_StoreController.products.WithID(GoogleproductId7);
            }
            else
            {
                p = m_StoreController.products.WithID(AppleproductId7);
            }

            itemInfo.PurchaserID = p.definition.id;
            itemInfo.PurchaserPrice = p.metadata.localizedPriceString;

            return itemInfo;
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            // Purchasing has succeeded initializing. Collect our Purchasing references.
            Debug.Log("OnInitialized: PASS");

            // Overall Purchasing system, configured with products for this application.
            m_StoreController = controller;
            // Store specific subsystem, for accessing device-specific store features.
            m_StoreExtensionProvider = extensions;
            isLoad = true;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            Dictionary<string, string> paramater = new Dictionary<string, string>();
            paramater.Add("Purchaser_Result", true.ToString());

            var id = args.purchasedProduct.definition.id;
            var array = id.Split('.');

            FirebaseManager.GetInstance.FirebaseLogEvent(array[array.Length - 1], paramater);

            var P = m_StoreController.products.WithID(args.purchasedProduct.definition.id);
            if (args.purchasedProduct.definition.id != AppleproductId6 &&
                args.purchasedProduct.definition.id != GoogleproductId6)
            {
                SingularSDK.InAppPurchase(P, null);
            }
            else
            {
                SingularSDK.InAppPurchase(P, null, true);
            }

            var obj = GameObject.Find("PopupManager");
            GameObject coin = null;
            // A consumable product has been purchased by this user.
            if (string.Equals(args.purchasedProduct.definition.id, kProductIDConsumable, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

                // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
            }
            // Or ... a non-consumable product has been purchased by this user.
            else if (string.Equals(args.purchasedProduct.definition.id, kProductIDNonConsumable,
                StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

                // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
            }
            // Or ... a subscription product has been purchased by this user.
            else if (string.Equals(args.purchasedProduct.definition.id, kProductIDSubscription,
                StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

                // TODO: The subscription item has been successfully purchased, grant this to the player.
            }
            else if (string.Equals(args.purchasedProduct.definition.id, GoogleproductId0, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    PlayerData.GetInstance.Gold += 255000;
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, GoogleproductId1, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    PlayerData.GetInstance.Gold += 142000;
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, GoogleproductId2, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    PlayerData.GetInstance.Gold += 66000;
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, GoogleproductId3, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    PlayerData.GetInstance.Gold += 27500;
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, GoogleproductId4, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    PlayerData.GetInstance.Gold += 11100;
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, GoogleproductId5, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    PlayerData.GetInstance.Gold += 2000;
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, GoogleproductId6, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null) PlayerData.GetInstance.IsAdsFree = true;
            }
            else if (string.Equals(args.purchasedProduct.definition.id, GoogleproductId7, StringComparison.Ordinal))
            {
                if (actPurchase != null)
                {
                    Debug.LogWarningFormat("KKI {0}", actPurchase);
                    actPurchase.Invoke();
                    actPurchase = null;
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, GoogleproductPackageId1,
                StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    BuyPackage(0);
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, GoogleproductPackageId2,
                StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    BuyPackage(1);
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, GoogleproductPackageId3,
                StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    BuyPackage(2);
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, GoogleproductPackageId4,
                StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    BuyPackage(3);
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, GoogleproductPackageId5,
                StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    BuyPackage(4);
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, GoogleproductPackageId6,
                StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    BuyPackage(5);
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, GoogleproductPackageId7,
                StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    BuyPackage(6);
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, GoogleproductLimitedPackage,
                StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    BuyPackage(7);
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, AppleproductId0, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    PlayerData.GetInstance.Gold += 255000;
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, AppleproductId1, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    PlayerData.GetInstance.Gold += 142000;
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, AppleproductId2, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    PlayerData.GetInstance.Gold += 60000;
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, AppleproductId3, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    PlayerData.GetInstance.Gold += 27500;
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, AppleproductId4, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    PlayerData.GetInstance.Gold += 11100;
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, AppleproductId5, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    PlayerData.GetInstance.Gold += 2000;
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, AppleproductId6, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null) PlayerData.GetInstance.IsAdsFree = true;
            }
            else if (string.Equals(args.purchasedProduct.definition.id, AppleproductId7, StringComparison.Ordinal))
            {
                if (actPurchase != null)
                {
                    Debug.LogWarningFormat("KKI {0}", actPurchase);
                    actPurchase.Invoke();
                    actPurchase = null;
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, AppleproductPackageId1,
                StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    BuyPackage(0);
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, AppleproductPackageId2,
                StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    BuyPackage(1);
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, AppleproductPackageId3,
                StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    BuyPackage(2);
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, AppleproductPackageId4,
                StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    BuyPackage(3);
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            else if (string.Equals(args.purchasedProduct.definition.id, AppleproductPackageId5,
                StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    BuyPackage(4);
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            /*
            else if (String.Equals(args.purchasedProduct.definition.id, AppleproductPackageId6, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    BuyPackage(5);
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            */
            /*
            else if (String.Equals(args.purchasedProduct.definition.id, AppleproductPackageId7, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    BuyPackage(6);
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            */
            else if (string.Equals(args.purchasedProduct.definition.id, AppleProductLimitedPackage,
                StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase : PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (PlayerData.GetInstance != null)
                {
                    BuyPackage(7);
                    coin = obj.GetComponent<PopupManager>().GetCoin();
                    coin.GetComponent<Animator>().SetTrigger("Normal");
                }
            }
            // Or ... an unknown product has been purchased by this user. Fill in additional products here....
            else
            {
                Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'",
                    args.purchasedProduct.definition.id));
            }

            // Return a flag indicating whether this product has completely been received, or if the application needs
            // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still
            // saving purchased products to the cloud, and when that save is delayed.
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing
            // this reason with the user to guide their troubleshooting actions.
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",
                product.definition.storeSpecificId, failureReason));

            var paramater = new Dictionary<string, string>();
            paramater.Add("Purchaser_Result", false.ToString());

            var id = product.definition.storeSpecificId;
            var array = id.Split('.');

            FirebaseManager.GetInstance.FirebaseLogEvent(array[array.Length - 1], paramater);
        }

        public ShopItemInfo GetItems(int num)
        {
            if (!isLoad) return null;
            var itemInfo = new ShopItemInfo();
            Product p = null;
            if (Application.platform == RuntimePlatform.Android)
                switch (num)
                {
                    case 0:
                        p = m_StoreController.products.WithID(GoogleproductId0);
                        break;

                    case 1:
                        p = m_StoreController.products.WithID(GoogleproductId1);
                        break;

                    case 2:
                        p = m_StoreController.products.WithID(GoogleproductId2);
                        break;

                    case 3:
                        p = m_StoreController.products.WithID(GoogleproductId3);
                        break;

                    case 4:
                        p = m_StoreController.products.WithID(GoogleproductId4);
                        break;

                    case 5:
                        p = m_StoreController.products.WithID(GoogleproductId5);
                        break;

                    case 6:
                        p = m_StoreController.products.WithID(GoogleproductId6);
                        break;
                }
            else
                switch (num)
                {
                    case 0:
                        p = m_StoreController.products.WithID(AppleproductId0);
                        break;

                    case 1:
                        p = m_StoreController.products.WithID(AppleproductId1);
                        break;

                    case 2:
                        p = m_StoreController.products.WithID(AppleproductId2);
                        break;

                    case 3:
                        p = m_StoreController.products.WithID(AppleproductId3);
                        break;

                    case 4:
                        p = m_StoreController.products.WithID(AppleproductId4);
                        break;

                    case 5:
                        p = m_StoreController.products.WithID(AppleproductId5);
                        break;

                    case 6:
                        p = m_StoreController.products.WithID(AppleproductId6);
                        break;
                }

            itemInfo.PurchaserID = p.definition.id;
            itemInfo.PurchaserPrice = p.metadata.localizedPriceString;
            return itemInfo;
        }

        public ShopItemInfo GetPackageItems(int num)
        {
            if (!isLoad) return null;
            var itemInfo = new ShopItemInfo();
            Product p = null;
            if (Application.platform == RuntimePlatform.Android)
            {
                switch (num)
                {
                    case 0:
                        p = m_StoreController.products.WithID(GoogleproductPackageId1);
                        break;

                    case 1:
                        p = m_StoreController.products.WithID(GoogleproductPackageId2);
                        break;

                    case 2:
                        p = m_StoreController.products.WithID(GoogleproductPackageId3);
                        break;

                    case 3:
                        p = m_StoreController.products.WithID(GoogleproductPackageId4);
                        break;

                    case 4:
                        p = m_StoreController.products.WithID(GoogleproductPackageId5);
                        break;

                    case 5:
                        p = m_StoreController.products.WithID(GoogleproductPackageId6);
                        break;

                    case 6:
                        p = m_StoreController.products.WithID(GoogleproductPackageId7);
                        break;

                    case 7:
                        p = m_StoreController.products.WithID(GoogleproductLimitedPackage);
                        break;

                    default:
                        p = m_StoreController.products.WithID(GoogleproductPackageId3);
                        break;
                }

                // Debug.Log("1번");
            }
            else
            {
                switch (num)
                {
                    case 0:
                        Debug.Log("0");
                        p = m_StoreController.products.WithID(AppleproductPackageId1);
                        break;

                    case 1:
                        Debug.Log("1");
                        p = m_StoreController.products.WithID(AppleproductPackageId2);
                        break;

                    case 2:
                        Debug.Log("2");
                        p = m_StoreController.products.WithID(AppleproductPackageId3);
                        break;

                    case 3:
                        Debug.Log("3");
                        p = m_StoreController.products.WithID(AppleproductPackageId4);
                        break;

                    case 4:
                        Debug.Log("4");
                        p = m_StoreController.products.WithID(AppleproductPackageId5);
                        break;
                    //case 5:
                    //    Debug.Log("5");
                    //    p = m_StoreController.products.WithID(AppleproductPackageId6);
                    //    break;
                    //case 6:
                    //    Debug.Log("6");
                    //    p = m_StoreController.products.WithID(AppleproductPackageId7);
                    //    break;
                    case 7:
                        Debug.Log("7");
                        p = m_StoreController.products.WithID(AppleProductLimitedPackage);
                        break;

                    default:
                        p = m_StoreController.products.WithID(AppleproductPackageId3);
                        break;
                }

                // Debug.Log("2번");
            }

            itemInfo.PurchaserID = p.definition.id;
            itemInfo.PurchaserPrice = p.metadata.localizedPriceString;
            return itemInfo;
        }

        public void BuyPackage(int num)
        {
            var items = packageInfo[num];
            if (PlayerData.GetInstance != null)
            {
                for (var i = 0; i < items.buyItems.Count; i++)
                {
                    var eitem = items.buyItems[i].item;
                    switch (eitem)
                    {
                        case EUseItem.NONE:
                            PlayerData.GetInstance.Gold += items.buyItems[i].intValue;
                            break;

                        case EUseItem.HAMMER:
                            PlayerData.GetInstance.ItemHammer += items.buyItems[i].intValue;
                            break;

                        case EUseItem.CROSS:
                            PlayerData.GetInstance.ItemCross += items.buyItems[i].intValue;
                            break;

                        case EUseItem.BOMB:
                            PlayerData.GetInstance.ItemBomb += items.buyItems[i].intValue;
                            break;

                        case EUseItem.COLOR:
                            PlayerData.GetInstance.ItemColor += items.buyItems[i].intValue;
                            break;
                    }
                }

                // Limited Edition
                if (num == 7) PlayerData.GetInstance.IsBuyLimitedPackage = true;
            }

            if (StageManager.GetInstance != null) StageManager.GetInstance.SetUI();
        }

        public ShopItemInfo GetShopItemInfoByKey(string key)
        {
            if (!isLoad) return null;
            var itemInfo = new ShopItemInfo();
            Product p = m_StoreController.products.WithID(key);

            if (p != null)
            {
                itemInfo.PurchaserID = p.definition.id;
                itemInfo.PurchaserPrice = p.metadata.localizedPriceString;
                return itemInfo;
            }
            else
            {
                return null;
            }
        }

        public void Init()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                if (!isCoroutine)
                {
                    StartCoroutine(PurchaserInit());
                    isCoroutine = true;
                }

                return;
            }

            // If we haven't set up the Unity Purchasing reference
            if (m_StoreController == null)
                // Begin to configure our connection to Purchasing
                InitializePurchasing();
        }

        public void InitializePurchasing()
        {
            // If we have already connected to Purchasing ...
            if (IsInitialized())
                // ... we are done here.
                return;

            // Create a builder, first passing in a suite of Unity provided stores.
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            // Add a product to sell / restore by way of its identifier, associating the general identifier
            // with its store-specific identifiers.
            builder.AddProduct(kProductIDConsumable, ProductType.Consumable);
            // Continue adding the non-consumable product.
            builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);
            // And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
            // if the Product ID was configured differently between Apple and Google stores. Also note that
            // one uses the general kProductIDSubscription handle inside the game - the store-specific IDs
            // must only be referenced here.
            builder.AddProduct(kProductIDSubscription, ProductType.Subscription, new IDs
            {
                {kProductNameAppleSubscription, AppleAppStore.Name},
                {kProductNameGooglePlaySubscription, GooglePlay.Name}
            });

            builder.AddProduct(GoogleproductId0, ProductType.Consumable);
            builder.AddProduct(GoogleproductId1, ProductType.Consumable);
            builder.AddProduct(GoogleproductId2, ProductType.Consumable);
            builder.AddProduct(GoogleproductId3, ProductType.Consumable);
            builder.AddProduct(GoogleproductId4, ProductType.Consumable);
            builder.AddProduct(GoogleproductId5, ProductType.Consumable);
            builder.AddProduct(GoogleproductPackageId1, ProductType.Consumable);
            builder.AddProduct(GoogleproductPackageId2, ProductType.Consumable);
            builder.AddProduct(GoogleproductPackageId3, ProductType.Consumable);
            builder.AddProduct(GoogleproductPackageId4, ProductType.Consumable);
            builder.AddProduct(GoogleproductPackageId5, ProductType.Consumable);
            builder.AddProduct(GoogleproductPackageId6, ProductType.Consumable);
            builder.AddProduct(GoogleproductPackageId7, ProductType.Consumable);
            builder.AddProduct(GoogleproductLimitedPackage, ProductType.Consumable);
            builder.AddProduct(GoogleproductId6, ProductType.NonConsumable);
            builder.AddProduct(GoogleproductId7, ProductType.Consumable);

            builder.AddProduct(AppleproductId0, ProductType.Consumable);
            builder.AddProduct(AppleproductId1, ProductType.Consumable);
            builder.AddProduct(AppleproductId2, ProductType.Consumable);
            builder.AddProduct(AppleproductId3, ProductType.Consumable);
            builder.AddProduct(AppleproductId4, ProductType.Consumable);
            builder.AddProduct(AppleproductId5, ProductType.Consumable);
            builder.AddProduct(AppleproductPackageId1, ProductType.Consumable);
            builder.AddProduct(AppleproductPackageId2, ProductType.Consumable);
            builder.AddProduct(AppleproductPackageId3, ProductType.Consumable);
            builder.AddProduct(AppleproductPackageId4, ProductType.Consumable);
            builder.AddProduct(AppleproductPackageId5, ProductType.Consumable);
            //builder.AddProduct(AppleproductPackageId6, ProductType.Consumable);
            //builder.AddProduct(AppleproductPackageId7, ProductType.Consumable);
            builder.AddProduct(AppleProductLimitedPackage, ProductType.Consumable);
            builder.AddProduct(AppleproductId6, ProductType.NonConsumable);
            builder.AddProduct(AppleproductId7, ProductType.Consumable);

            // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration
            // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
            UnityPurchasing.Initialize(this, builder);
        }

        private bool IsInitialized()
        {
            // Only say we are initialized if both the Purchasing references are set.
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }

        public void BuyConsumable()
        {
            // Buy the consumable product using its general identifier. Expect a response either
            // through ProcessPurchase or OnPurchaseFailed asynchronously.
            BuyProductID(kProductIDConsumable);
        }

        public void BuyProductId0()
        {
            if (Application.platform == RuntimePlatform.Android)
                BuyProductID(GoogleproductId0);
            else
                BuyProductID(AppleproductId0);
        }

        public void BuyProductId1()
        {
            if (Application.platform == RuntimePlatform.Android)
                BuyProductID(GoogleproductId1);
            else
                BuyProductID(AppleproductId1);
        }

        public void BuyProductId2()
        {
            if (Application.platform == RuntimePlatform.Android)
                BuyProductID(GoogleproductId2);
            else
                BuyProductID(AppleproductId2);
        }

        public void BuyProductId3()
        {
            if (Application.platform == RuntimePlatform.Android)
                BuyProductID(GoogleproductId3);
            else
                BuyProductID(AppleproductId3);
        }

        public void BuyProductId4()
        {
            if (Application.platform == RuntimePlatform.Android)
                BuyProductID(GoogleproductId4);
            else
                BuyProductID(AppleproductId4);
        }

        public void BuyProductId5()
        {
            if (Application.platform == RuntimePlatform.Android)
                BuyProductID(GoogleproductId5);
            else
                BuyProductID(AppleproductId5);
        }

        public void BuyProductId6()
        {
            if (Application.platform == RuntimePlatform.Android)
                BuyProductID(GoogleproductId6);
            else
                BuyProductID(AppleproductId6);
        }

        public void BuyNonConsumable()
        {
            // Buy the non-consumable product using its general identifier. Expect a response either
            // through ProcessPurchase or OnPurchaseFailed asynchronously.
            BuyProductID(kProductIDNonConsumable);
        }

        public void BuySubscription()
        {
            // Buy the subscription product using its the general identifier. Expect a response either
            // through ProcessPurchase or OnPurchaseFailed asynchronously.
            // Notice how we use the general product identifier in spite of this ID being mapped to
            // custom store-specific identifiers above.
            BuyProductID(kProductIDSubscription);
        }

        public void BuyProductID(string productId, Action _action = null)
        {
            // If Purchasing has been initialized ...
            if (IsInitialized())
            {
                // ... look up the Product reference with the general product identifier and the Purchasing
                // system's products collection.
                var product = m_StoreController.products.WithID(productId);
                // If the look up found a product for this device's store and that product is ready to be sold ...
                if (product != null && product.availableToPurchase)
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                    // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed
                    // asynchronously.
                    actPurchase = _action;
                    m_StoreController.InitiatePurchase(product);
                }
                // Otherwise ...
                else
                {
                    // ... report the product look-up failure situation
                    Debug.Log(
                        "BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            // Otherwise ...
            else
            {
                // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or
                // retrying initiailization.
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }

        // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google.
        // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
        public void RestorePurchases()
        {
            // If Purchasing has not yet been set up ...
            if (!IsInitialized())
            {
                // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
                Debug.Log("RestorePurchases FAIL. Not initialized.");
                return;
            }

            // If we are running on an Apple device ...
            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.OSXPlayer)
            {
                // ... begin restoring purchases
                Debug.Log("RestorePurchases started ...");

                // Fetch the Apple store-specific subsystem.
                var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
                // Begin the asynchronous process of restoring purchases. Expect a confirmation response in
                // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
                apple.RestoreTransactions(result =>
                {
                    // The first phase of restoration. If no more responses are received on ProcessPurchase then
                    // no purchases are available to be restored.
                    Debug.Log("RestorePurchases continuing: " + result +
                              ". If no further messages, no purchases available to restore.");
                });
            }
            // Otherwise ...
            else
            {
                // We are not running on an Apple device. No work is necessary to restore purchases.
                Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            }
        }

        public bool PurchaserLoadCheck()
        {
            if (m_StoreController == null) return false;
            var returnValue = true;
            if (Application.internetReachability == NetworkReachability.NotReachable) return true;
#if UNITY_ANDROID
            if (m_StoreController.products.WithID(GoogleproductPackageId1) == null) return false;
            if (m_StoreController.products.WithID(GoogleproductPackageId2) == null) return false;
            if (m_StoreController.products.WithID(GoogleproductPackageId3) == null) return false;
            if (m_StoreController.products.WithID(GoogleproductPackageId4) == null) return false;
            if (m_StoreController.products.WithID(GoogleproductPackageId5) == null) return false;
            if (m_StoreController.products.WithID(GoogleproductPackageId6) == null) return false;
            if (m_StoreController.products.WithID(GoogleproductPackageId7) == null) return false;
            if (m_StoreController.products.WithID(GoogleproductLimitedPackage) == null) return false;
            if (m_StoreController.products.WithID(GoogleproductId0) == null) return false;
            if (m_StoreController.products.WithID(GoogleproductId1) == null) return false;
            if (m_StoreController.products.WithID(GoogleproductId2) == null) return false;
            if (m_StoreController.products.WithID(GoogleproductId3) == null) return false;
            if (m_StoreController.products.WithID(GoogleproductId4) == null) return false;
            if (m_StoreController.products.WithID(GoogleproductId5) == null) return false;
            if (m_StoreController.products.WithID(GoogleproductId6) == null) return false;
            if (m_StoreController.products.WithID(GoogleproductId7) == null) return false;
#elif UNITY_IOS
            if (m_StoreController.products.WithID(AppleproductPackageId1) == null) return false;
            if (m_StoreController.products.WithID(AppleproductPackageId2) == null) return false;
            if (m_StoreController.products.WithID(AppleproductPackageId3) == null) return false;
            if (m_StoreController.products.WithID(AppleproductPackageId4) == null) return false;
            if (m_StoreController.products.WithID(AppleproductPackageId5) == null) return false;
            //if (m_StoreController.products.WithID(AppleproductPackageId6) == null) return false;
            //if (m_StoreController.products.WithID(AppleproductPackageId7) == null) return false;
            if (m_StoreController.products.WithID(AppleProductLimitedPackage) == null) return false;
            if (m_StoreController.products.WithID(AppleproductId0) == null) return false;
            if (m_StoreController.products.WithID(AppleproductId1) == null) return false;
            if (m_StoreController.products.WithID(AppleproductId2) == null) return false;
            if (m_StoreController.products.WithID(AppleproductId3) == null) return false;
            if (m_StoreController.products.WithID(AppleproductId4) == null) return false;
            if (m_StoreController.products.WithID(AppleproductId5) == null) return false;
            if (m_StoreController.products.WithID(AppleproductId6) == null) return false;
            if (m_StoreController.products.WithID(AppleproductId7) == null) return false;
#endif
            isLoad = true;
            return returnValue;
        }

        public IEnumerator PurchaserInit()
        {
            while (!isLoad)
            {
                yield return new WaitForSeconds(5.0f);

                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    isLoad = false;
                }
                else
                {
                    if (!PurchaserLoadCheck()) Init();
                }
            }
        }
        
        public Product GetSubscriptionProduct(ESubsType subsType)
        {
            if (SubscriptionProductList.ContainsKey(subsType) && m_StoreController!=null)
            {
                return m_StoreController.products.WithID(SubscriptionProductList[subsType]);
            }
            else
            {
                return null;
            }
        }
        
        public void Refresh(Action sucess, Action<InitializationFailureReason> failed)
        {
            HashSet<ProductDefinition> additional = new HashSet<ProductDefinition>()
            {
                //new ProductDefinition(SubscriptionProductList[ESubsType.AOSWeekVip], ProductType.Subscription),
                //new ProductDefinition(SubscriptionProductList[ESubsType.AOSMonthVip], ProductType.Subscription),
                //new ProductDefinition(SubscriptionProductList[ESubsType.IOSWeekVip], ProductType.Subscription),
                //new ProductDefinition(SubscriptionProductList[ESubsType.IOSMonthVip], ProductType.Subscription),
            };

            m_StoreController.FetchAdditionalProducts(additional, sucess, failed);
        }
    }
}