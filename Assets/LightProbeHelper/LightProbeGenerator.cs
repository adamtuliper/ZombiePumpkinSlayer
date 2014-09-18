using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Light Probe Helper/Light Probe Generator")]
public class LightProbeGenerator : MonoBehaviour
{
	[System.Serializable]
	public class LightProbeArea
	{
		public Bounds ProbeVolume;
		public Vector3 Subdivisions = Vector3.one * 5;
		public int RandomCount = 0;
	}

	public enum LightProbePlacementType
	{
		Grid,
		Random
	}

	public LightProbeArea[] LightProbeVolumes;
	public LightProbePlacementType PlacementAlgorithm;

	public void GenProbes()
	{
		//todo: generate the probes
		LightProbeGroup lprobe = GetComponent<LightProbeGroup>();
		if( lprobe == null )
		{
			Debug.LogError( "LightProbeGenerator: Must have LightProbeGroup attached!" );
			return;
		}

		List<Vector3> probePositions = new List<Vector3>();

		foreach( LightProbeArea area in LightProbeVolumes )
		{
			if( PlacementAlgorithm == LightProbePlacementType.Grid )
			{
				probePositions.AddRange( GetProbesForVolume_Grid( area.ProbeVolume, area.Subdivisions ) );
			}
			else
			{
				probePositions.AddRange( GetProbesForVolume_Random( area.ProbeVolume, area.RandomCount ) );
			}
		}

		lprobe.probePositions = probePositions.ToArray();
	}

	List<Vector3> GetProbesForVolume_Grid( Bounds ProbeVolume, Vector3 Subdivisions )
	{
		List<Vector3> probePositions = new List<Vector3>();

		Vector3 step = new Vector3( ProbeVolume.extents.x / Subdivisions.x, ProbeVolume.extents.y / Subdivisions.y, ProbeVolume.extents.z / Subdivisions.z );

		for( int x = 0; x <= Subdivisions.x; x++ )
		{
			for( int y = 0; y <= Subdivisions.y; y++ )
			{
				for( int z = 0; z <= Subdivisions.z; z++ )
				{
					Vector3 probePos = ( ProbeVolume.center - ( ProbeVolume.extents / 2 ) ) + new Vector3( step.x * x, step.y * y, step.z * z );
					probePositions.Add( probePos );
				}
			}
		}

		return probePositions;
	}

	List<Vector3> GetProbesForVolume_Random( Bounds ProbeVolume, int Count)
	{
		List<Vector3> probePositions = new List<Vector3>();

		for( int c = 0; c <= Count; c++ )
		{
			Vector3 probePos = ProbeVolume.center + new Vector3( Random.Range( -0.5f, 0.5f ) * ProbeVolume.extents.x, Random.Range( -0.5f, 0.5f ) * ProbeVolume.extents.y, Random.Range( -0.5f, 0.5f ) * ProbeVolume.extents.z );
			probePositions.Add( probePos );
		}

		return probePositions;
	}

	void OnDrawGizmos()
	{
		if( LightProbeVolumes != null )
		{
			Gizmos.color = Color.red;
			foreach( LightProbeArea volume in LightProbeVolumes )
			{
				Gizmos.DrawWireCube( volume.ProbeVolume.center, volume.ProbeVolume.extents );
			}
		}
	}
}