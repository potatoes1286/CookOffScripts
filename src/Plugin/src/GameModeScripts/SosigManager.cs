using System.Collections.Generic;
using FistVR;
using UnityEngine;

namespace H3VRMod.GameModeScripts
{
	public class SosigManager : MonoBehaviour
	{
		/*public static Dictionary<SosigEnemyID, SosigEnemyTemplate> enemyTemplates =
			new Dictionary<SosigEnemyID, SosigEnemyTemplate>();
		public static void UpdateSosigTemplateDictionary()
		{
			var sets = Resources.LoadAll("SosigEnemyTemplates", typeof(SosigEnemyTemplate));
			foreach (var obj in sets)
			{
				var set = (SosigEnemyTemplate)@obj;
				if (set != null)
				{
					Debug.Log("Added cat " + set.SosigEnemyID + "to COM!");
					enemyTemplates.Add(set.SosigEnemyID, set);
				}
			}
		}*/
		public static void GeneratePatrol(
			TNH_PatrolChallenge pc,
			Vector3 holdPoint,
			float healthDropChance,
			string healthPickupID,
			int iff,
			Transform spawnPoint)
		{
			//get a random patrol to spawn from the PC
			TNH_PatrolChallenge.Patrol patrol = pc.Patrols[UnityEngine.Random.Range(0, pc.Patrols.Count)];
			TNH_Manager.SosigPatrolSquad sosigPatrolSquad = new TNH_Manager.SosigPatrolSquad();
			List<int> list = new List<int>();
			
			sosigPatrolSquad.PatrolPoints = new List<Vector3>() { holdPoint };
			for (int k = 0; k < patrol.PatrolSize; k++)
			{
				SosigEnemyTemplate enemyTemplate;
				bool allowAllWeapons;
				if (k == 0)
				{
					enemyTemplate = IM.Instance.odicSosigObjsByID[patrol.LType];
					//enemyTemplate = enemyTemplates[patrol.LType];
					allowAllWeapons = true;
				}
				else
				{
					enemyTemplate = IM.Instance.odicSosigObjsByID[patrol.EType];
					//enemyTemplate = enemyTemplates[patrol.EType];
					allowAllWeapons = false;
				}
				Sosig sosig = SpawnEnemy(enemyTemplate, spawnPoint, iff, true, sosigPatrolSquad.PatrolPoints[0]);
				float num2 = UnityEngine.Random.Range(0f, 1f);
				if (num2 < healthDropChance)
				{
					sosig.Links[1].RegisterSpawnOnDestroy(IM.OD[healthPickupID]);
				}
				sosig.SetAssaultSpeed(Sosig.SosigMoveSpeed.Walking);
				sosigPatrolSquad.Squad.Add(sosig);
			}
		}

		public static Sosig SpawnEnemy(SosigEnemyTemplate t, Transform point, int IFF, bool IsAssault, Vector3 pointOfInterest)
		{
			GameObject weaponPrefab = null;
			if (t.WeaponOptions.Count > 0)
			{
				weaponPrefab = t.WeaponOptions[UnityEngine.Random.Range(0, t.WeaponOptions.Count)].GetGameObject();
			}
			GameObject weaponPrefab2 = null;
			if (t.WeaponOptions_Secondary.Count > 0)
			{
				float num = UnityEngine.Random.Range(0f, 1f);
				if (num <= t.SecondaryChance)
				{
					weaponPrefab2 = t.WeaponOptions_Secondary[UnityEngine.Random.Range(0, t.WeaponOptions_Secondary.Count)].GetGameObject();
				}
			}
			GameObject weaponPrefab3 = null;
			if (t.WeaponOptions_Tertiary.Count > 0)
			{
				float num2 = UnityEngine.Random.Range(0f, 1f);
				if (num2 <= t.TertiaryChance)
				{
					weaponPrefab3 = t.WeaponOptions_Tertiary[UnityEngine.Random.Range(0, t.WeaponOptions_Tertiary.Count)].GetGameObject();
				}
			}
			SosigConfigTemplate t2 = t.ConfigTemplates[UnityEngine.Random.Range(0, t.ConfigTemplates.Count)];
			return SpawnEnemySosig(t.SosigPrefabs[UnityEngine.Random.Range(0, t.SosigPrefabs.Count)].GetGameObject(), weaponPrefab, weaponPrefab2, weaponPrefab3, point.position, point.rotation, t2, t.OutfitConfig[UnityEngine.Random.Range(0, t.OutfitConfig.Count)], IFF, IsAssault, pointOfInterest);
		}

		public static Sosig SpawnEnemySosig(GameObject prefab, GameObject weaponPrefab, GameObject weaponPrefab2, GameObject weaponPrefab3, Vector3 pos, Quaternion rot, SosigConfigTemplate t, SosigOutfitConfig o, int IFF, bool IsAssault, Vector3 pointOfInterest)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, pos, rot);
			Sosig componentInChildren = gameObject.GetComponentInChildren<Sosig>();
			componentInChildren.Configure(t);
			componentInChildren.E.IFFCode = IFF;
			if (weaponPrefab != null)
			{
				SosigWeapon component = UnityEngine.Object.Instantiate<GameObject>(weaponPrefab, pos + Vector3.up * 0.1f, rot).GetComponent<SosigWeapon>();
				component.SetAutoDestroy(true);
				component.O.SpawnLockable = false;
				if (component.Type == SosigWeapon.SosigWeaponType.Gun)
				{
					componentInChildren.Inventory.FillAmmoWithType(component.AmmoType);
				}
				componentInChildren.Inventory.Init();
				componentInChildren.Inventory.FillAllAmmo();
				if (component != null)
				{
					componentInChildren.InitHands();
					componentInChildren.ForceEquip(component);
					component.SetAmmoClamping(true);
				}
				if (weaponPrefab2 != null)
				{
					SosigWeapon component2 = UnityEngine.Object.Instantiate<GameObject>(weaponPrefab2, pos + Vector3.up * 0.1f, rot).GetComponent<SosigWeapon>();
					component2.SetAutoDestroy(true);
					component2.O.SpawnLockable = false;
					component2.SetAmmoClamping(true);
					if (component2.Type == SosigWeapon.SosigWeaponType.Gun)
					{
						componentInChildren.Inventory.FillAmmoWithType(component2.AmmoType);
					}
					if (component2 != null)
					{
						componentInChildren.ForceEquip(component2);
					}
				}
				if (weaponPrefab3 != null)
				{
					SosigWeapon component3 = UnityEngine.Object.Instantiate<GameObject>(weaponPrefab3, pos + Vector3.up * 0.1f, rot).GetComponent<SosigWeapon>();
					component3.SetAutoDestroy(true);
					component3.O.SpawnLockable = false;
					component3.SetAmmoClamping(true);
					if (component3.Type == SosigWeapon.SosigWeaponType.Gun)
					{
						componentInChildren.Inventory.FillAmmoWithType(component3.AmmoType);
					}
					if (component3 != null)
					{
						componentInChildren.ForceEquip(component3);
					}
				}
			}
			float num = UnityEngine.Random.Range(0f, 1f);
			if (num < o.Chance_Headwear)
			{
				SpawnAccesoryToLink(o.Headwear, componentInChildren.Links[0]);
			}
			num = UnityEngine.Random.Range(0f, 1f);
			if (num < o.Chance_Facewear)
			{
				SpawnAccesoryToLink(o.Facewear, componentInChildren.Links[0]);
			}
			num = UnityEngine.Random.Range(0f, 1f);
			if (num < o.Chance_Eyewear)
			{
				SpawnAccesoryToLink(o.Eyewear, componentInChildren.Links[0]);
			}
			num = UnityEngine.Random.Range(0f, 1f);
			if (num < o.Chance_Torsowear)
			{
				SpawnAccesoryToLink(o.Torsowear, componentInChildren.Links[1]);
			}
			num = UnityEngine.Random.Range(0f, 1f);
			if (num < o.Chance_Pantswear)
			{
				SpawnAccesoryToLink(o.Pantswear, componentInChildren.Links[2]);
			}
			num = UnityEngine.Random.Range(0f, 1f);
			if (num < o.Chance_Pantswear_Lower)
			{
				SpawnAccesoryToLink(o.Pantswear_Lower, componentInChildren.Links[3]);
			}
			num = UnityEngine.Random.Range(0f, 1f);
			if (num < o.Chance_Backpacks)
			{
				SpawnAccesoryToLink(o.Backpacks, componentInChildren.Links[1]);
			}
			if (t.UsesLinkSpawns)
			{
				for (int i = 0; i < componentInChildren.Links.Count; i++)
				{
					float num2 = UnityEngine.Random.Range(0f, 1f);
					if (num2 < t.LinkSpawnChance[i])
					{
						componentInChildren.Links[i].RegisterSpawnOnDestroy(t.LinkSpawns[i]);
					}
				}
			}
			if (IsAssault)
			{
				componentInChildren.CurrentOrder = Sosig.SosigOrder.Assault;
				componentInChildren.FallbackOrder = Sosig.SosigOrder.Assault;
				componentInChildren.CommandAssaultPoint(pointOfInterest);
			}
			else
			{
				componentInChildren.CurrentOrder = Sosig.SosigOrder.Wander;
				componentInChildren.FallbackOrder = Sosig.SosigOrder.Wander;
				float num3 = UnityEngine.Random.Range(0f, 1f);
				if (num3 > 0.25f)
				{
				}
				componentInChildren.CommandGuardPoint(pointOfInterest, true);
				componentInChildren.SetDominantGuardDirection(UnityEngine.Random.onUnitSphere);
			}
			componentInChildren.SetGuardInvestigateDistanceThreshold(25f);
			return componentInChildren;
		}

		public static void SpawnAccesoryToLink(List<FVRObject> gs, SosigLink l)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(gs[UnityEngine.Random.Range(0, gs.Count)].GetGameObject(), l.transform.position, l.transform.rotation);
			gameObject.transform.SetParent(l.transform);
			SosigWearable component = gameObject.GetComponent<SosigWearable>();
			component.RegisterWearable(l);
		}
	}
}