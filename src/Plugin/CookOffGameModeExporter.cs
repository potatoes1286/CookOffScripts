using UnityEngine;
using WurstMod.Runtime;

namespace CookOff
{
	public class CookOffGameModeExporter : CustomSceneLoader
	{
		public override string GamemodeId { get; } = "potatoes.cookoff";
		public override string BaseScene { get; } = "ProvingGround";

		public override void PostLoad()
		{
			Debug.Log("Started Cookoff Gamemode");
		}
		
	}
}