using UnityEngine;
using WurstMod.Runtime;

namespace CookOff
{
	public class CookoffGamemodeLoader : CustomSceneLoader
	{
		public override string GamemodeId => "potatoes.cookoff";
		public override string BaseScene => "ProvingGround";

		public override void PostLoad()
		{
			Debug.Log("Started Cookoff Gamemode");
		}
		
	}
}