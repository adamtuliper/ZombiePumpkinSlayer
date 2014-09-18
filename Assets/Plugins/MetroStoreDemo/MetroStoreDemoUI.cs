using UnityEngine;
using System.Collections;
using Prime31;
using Prime31.MetroStore;


public class MetroStoreDemoUI : MonoBehaviourGUI
{
	void Start()
	{
		// the licenseChangedEvent is fired whenever any app or product licenses change
		Store.licenseChangedEvent += () => { Debug.Log( "License changed event fired" ); };
	}
	
	
	void OnGUI()
	{
		beginColumn();
		
		if( GUILayout.Button( "Load Demo Licensing Information from XML" ) )
		{
			// by calling loadTestingLicenseXmlFile you are putting the app into test mode which uses Microsofts CurrentAppSimulator
			// to simulate all calls based on the data in the xml file. Note that the license.xml file must be relative to the install
			// directory. For this example it would be in the root of the Visual Studio project
			Store.loadTestingLicenseXmlFile( "license.xml", listingInfo =>
			{
				Debug.Log( "loading up listing. printing results" );
				Debug.Log( "price: " + listingInfo.formattedPrice );
				Debug.Log( "description: " + listingInfo.description );
				
				Debug.Log( "dumping productListings: " );
				foreach( var p in listingInfo.productListings )
					Debug.Log( p.Key + ": " + p.Value );
			});
		}
		
		
		if( GUILayout.Button( "Is App in Trial?" ) )
		{
			Debug.Log( "Is in trial mode: " + Store.isInTrialMode() );
		}
		
		
		if( GUILayout.Button( "Request App Purchase" ) )
		{
			Store.requestAppPurchase( didPurchase =>
			{
				Debug.Log( "app purchase result. didPurchase: " + didPurchase );
			});
		}
		
		
		if( GUILayout.Button( "Get App License Information" ) )
		{
			Debug.Log( "App License Info: " + Store.getLicenseInformation() );
		}
		
		
		if( GUILayout.Button( "Request Product Purchase (Durable)" ) )
		{
			Store.requestProductPurchase( "product1", ( purchaseResults, error ) =>
			{
				if( error != null )
					Debug.LogError( "Error purchasing item: " + error );
				else
					Debug.Log( "product purchase result: " + purchaseResults );
			});
		}
		
		
		if( GUILayout.Button( "Get Product License (Durable)" ) )
		{
			Debug.Log( "Product License: " + Store.getProductLicense( "product1" ) );
		}
		
		
		endColumn( true );
		
		
		GUILayout.Label( "Consumable Product Methods" );
		
		if( GUILayout.Button( "Request Product Purchase (Consumable)" ) )
		{
			Store.requestProductPurchase( "ItalianRecipes", ( purchaseResults, error ) =>
			{
				if( error != null )
					Debug.LogError( "Error purchasing item: " + error );
				else
					Debug.Log( "product purchase result: " + purchaseResults );
			});
		}
		
		
		if( GUILayout.Button( "Load Unfulfilled Consumables" ) )
		{
			Store.loadUnfulfilledConsumables( ( product, error ) =>
			{
				if( error != null )
					Debug.LogError( "Error loading unfilfilled consumables: " + error );
				else
					Debug.Log( "found unfulfilled consumable: " + product );
			});
		}
		
		
		if( GUILayout.Button( "Report All Unfulfilled Consumables as Fulfilled" ) )
		{
			Store.loadUnfulfilledConsumables( ( product, error ) =>
			{
				if( error != null )
				{
					Debug.LogError( "Error loading unfilfilled consumables: " + error );
				}
				else
				{
					Debug.Log( "reporting product fulfilled: " + product );
					Store.reportConsumableFulfillment( product.productId, product.transactionId, ( result, exception ) =>
					{
						if( exception != null )
							Debug.LogError( "error reporting product fulfilled: " + exception );
						else
							Debug.Log( "report fulfillment result: " + result );
					});
				}
			});
		}
		
		endColumn();
	}

}
