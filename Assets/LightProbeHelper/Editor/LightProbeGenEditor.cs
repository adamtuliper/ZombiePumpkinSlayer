using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(LightProbeGenerator))]
public class LightProbeGenEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if( GUILayout.Button( "Generate" ) )
		{
			( target as LightProbeGenerator ).GenProbes();
		}
	}
}