using FistVR;
using UnityEngine;

namespace H3VRMod.GameModeScripts
{
	[System.Serializable]
	public class CookOffAssault : MonoBehaviour
	{
		/*public CookOffPatrol[] Patrols;*/
		public float delayBetweenPatrols;
		public float startingTimeGrace;
		public float assaultLength;
		public WurstMod.MappingComponents.Generic.SosigSpawner[] spawners;
	}
}