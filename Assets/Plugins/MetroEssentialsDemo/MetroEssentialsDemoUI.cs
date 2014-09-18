using UnityEngine;
using System;
using System.Collections;
using System.IO;
using Prime31.MetroEssentials;
using Prime31;


public class MetroEssentialsDemoUI : MonoBehaviourGUI
{
	void Start()
	{
		// add something for the SettingsPane
		SettingsPane.registerSettingsCommand( "My Magical License", "This app uses the internet. It's 2014 so deal with it" );
		SettingsPane.registerSettingsCommand( "Custom Command that Just Logs Stuff", () => { Debug.Log( "Logged from the settings" ); } );
	}
	
	
	void OnGUI()
	{
		beginColumn();
		
		if( GUILayout.Button( "Set Roaming Setting Value" ) )
		{
			RoamingSettings.setValueForKey( "intKey", 14 );
			Debug.Log( "value of intKey: " + RoamingSettings.valueForKey( "intKey" ) );
		}
		
		
		if( GUILayout.Button( "Update Badge" ) )
		{
			Tiles.updateBadge( "attention" );
		}
		
		
		if( GUILayout.Button( "Show Share UI" ) )
		{
			// setup something to share. note that title and description are mandatory
			Sharing.title = "Share This Cool Stuff!";
			Sharing.description = "This is a link to the prime[31] web site";
			
			// url and text and optional
			Sharing.url = "http://prime31.com";
			Sharing.text = "Check this out!";
			
			Debug.Log( "about to show share with actual api" );
			Sharing.showShareUI();
		}
		
		
		if( GUILayout.Button( "Show Settings Pane" ) )
		{
			SettingsPane.show();
		}


		if( GUILayout.Button( "Show Toast (ToastText04)" ) )
		{
			var text = new string[] { "The Headline", "Some body text", "The second body text here" };
			Toasts.showToast( ToastTemplateType.ToastText04, text );
		}
		
		
		if( GUILayout.Button( "Show Toast (ToastImageAndText03)" ) )
		{
			var text = new string[] { "The Headline", "Some body text" };
			Toasts.showToast( ToastTemplateType.ToastImageAndText03, text, "http://cchronicle.com/wp-content/uploads/2010/06/Stuff-Story-7102832.png" );
		}
		
		
		if( GUILayout.Button( "Show Toast with Events (ToastText04)" ) )
		{
			var text = new string[] { "The Headline", "Some body text", "The second body text here" };
			Toasts.showToast( ToastTemplateType.ToastText04, text, null, null,
				reasonDismissed =>
			{
				Debug.Log( "Toast dismissed: " + reasonDismissed );
			},
			() =>
			{
				Debug.Log( "Toast activated" );
			},
			reasonFailed =>
			{
				Debug.Log( "Toast failed: " + reasonFailed.Message );
			} );
		}
		
		
		if( GUILayout.Button( "Schedule Toast for 30 seconds from now (ToastText03)" ) )
		{
			var text = new string[] { "The Headline Goes Here", "Then the body text goes here" };
			Toasts.scheduleToast( ToastTemplateType.ToastText03, text, DateTime.Now.AddSeconds( 30 ) );
		}


		if( GUILayout.Button( "Create Push Channel" ) )
		{
			Toasts.createPushNotificationChannelForApplication( ( channel, error ) =>
			{
				if( error != null )
				{
					Debug.Log( "error creating push channel: " + error );
				}
				else
				{
					Debug.Log( "push channel created successfully: " + channel );

#if NETFX_CORE
					// NOTE THAT THIS WONT WORK WHEN IN A SCRIPT THAT IS IN THE PLUGINS FOLDER DUE TO A UNITY BUG!
					// we use this native block to access the native channel and add an event listener
					// for when a push is received
					Debug.Log( "adding push event listener" );
					var nativeChannel = (Windows.Networking.PushNotifications.PushNotificationChannel)channel.nativeChannel;
					nativeChannel.PushNotificationReceived += ( sender, args ) =>
					{
						Debug.Log( "push received: " + args.RawNotification.Content );
					};
#endif
				}
			} );
		}

		
		endColumn( true );
		
		if( GUILayout.Button( "Register for Snap Changes" ) )
		{
			Snap.registerForSnapChanges( ( width, height ) =>
			{
				Debug.Log( "snap event fired. width: " + width + ", height: " + height );
			}, true );
		}
		
		
		if( GUILayout.Button( "Get Tile Template Content" ) )
		{
			var content = Tiles.getTemplateContent( TileTemplateType.TileSquareText04 );
			Debug.Log( "tile template content: " + content );
		}
		
		
		if( GUILayout.Button( "Update Live Tile (TileSquareText02)" ) )
		{
			var text = new string[] { "The Title", "Some smaller text for the sub title" };
			Tiles.updateTile( TileTemplateType.TileSquareText02, text );
		}
		
		
		if( GUILayout.Button( "Update Live Tile (TileSquareImage) with Bundle Image" ) )
		{
			// image in the app bundle. Note that you must put an image named "tileText.png" in the Visual Studio
			// assets folder for this to work!
			var images = new string[] { "ms-appx:///assets/tileText.png" };
			Tiles.updateTile( TileTemplateType.TileSquareImage, null, images ); 
		}
		
				
		if( GUILayout.Button( "Update Live Tile (TileSquareImage) with Remote Image" ) )
		{
			// remote images require the internetClient capability in the manifest
			var images = new string[] { "https://prime31.com/media/img/prime31logo.png" };
			Tiles.updateTile( TileTemplateType.TileSquareImage, null, images );
		}
		
		
		if( GUILayout.Button( "Update Live Tile (TileSquareImage) with Local Image" ) )
		{
			Debug.Log( "starting liveTileWithScreenshot" );
			StartCoroutine( liveTileWithScreenshot() );
		}
		
		
		endColumn();
	}
	
	
	private IEnumerator liveTileWithScreenshot()
	{
		yield return new WaitForEndOfFrame();
		
		// get screenshot
		var tex = new Texture2D( 250, 250 );
		tex.ReadPixels( new Rect( 0, Screen.height - 250, 250, 250 ), 0, 0, false );
		tex.Apply();
		var bytes = tex.EncodeToPNG();
		Destroy( tex );
		
		// write to disk
		Prime31.MetroEssentials.Utils.writeAllBytes( "screenshot.png", bytes );
		
		// our source is local storage so we reference it appropriately
		var images = new string[] { "ms-appdata:///local/screenshot.png" };
		Tiles.updateTile( TileTemplateType.TileSquareImage, null, images );
	}

}
