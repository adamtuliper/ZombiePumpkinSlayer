using Prime31.MetroStore;
using UnityEngine;
using System.Collections;

public class InAppPurchases : MonoBehaviour
{

    private GameController _gameController;
	// Use this for initialization
	void Start () {

        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        // by calling loadTestingLicenseXmlFile you are putting the app into test mode which uses Microsofts CurrentAppSimulator
        // to simulate all calls based on the data in the xml file. Note that the license.xml file must be relative to the install
        // directory. For this example it would be in the root of the Visual Studio project
        Store.loadTestingLicenseXmlFile("license.xml", listingInfo =>
        {
            Debug.Log("loading up listing. printing results");
            Debug.Log("price: " + listingInfo.formattedPrice);
            Debug.Log("description: " + listingInfo.description);

            Debug.Log("dumping productListings: ");
            foreach (var p in listingInfo.productListings)
                Debug.Log(p.Key + ": " + p.Value);
        });
	}
	
	// Update is called once per frame
	void PurchaseResurrection () {
        Store.requestProductPurchase("resurrection", (purchaseResult,anyException) =>
        {
            if (purchaseResult.status == ProductPurchaseStatus.Succeeded ||
                purchaseResult.status == ProductPurchaseStatus.AlreadyPurchased)
            {
                //Enable whatever - we just purchased it (or own it)
                Debug.Log("in-app purchase result. purchaseResult: " + purchaseResult.receiptXml);
            }
            else
            {
                Debug.Log("Did not purchase it, purchaseResult was:" + purchaseResult.status);
            }
        });
        //hide the dialog
	    _gameController.HideResurrectPurchase();
	}
}
