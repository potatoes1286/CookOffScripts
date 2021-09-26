using FistVR;
using UnityEngine;

namespace H3VRMod.GameModeScripts
{
	[System.Serializable]
	public class CookOffPatrol : MonoBehaviour
	{
		public int weight;
		public TNH_PatrolChallenge Patrol;
		public Transform[] PatrolSpawnPoints;
	}
}