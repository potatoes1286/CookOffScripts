using System;
using System.Collections.Generic;
using FistVR;
using H3VRMod.CookScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace H3VRMod.GameModeScripts
{



	public class CookOffManager : MonoBehaviour
	{
		[Header("Player Info")]
		public int money;
		[Header("Sosig Definitions")]
		public CookOffAssault[] assaults;
		public string healthPickupID;
		public float healthDropChance;
		
		[Header("Item References")]
		public Transform baseLoc;
		
		[Header("Current State")]
		public int currentAssault;
		public bool isInAssault;
		public float timeLeftToNextPhase;
		private float timeSinceLastPatrol;
		private float timeBetweenPatrols;

		[Header("Audio")]
		public CookOffAudio cookOffAudio;
		public float timeTilNextVoice;
		public Queue<AudioEvent> VoiceQueue = new Queue<AudioEvent>();
		
		[HideInInspector]
		public static CookOffManager com;

		public void Start()
		{
			com = this;
			StartNextAssault(0);
			//SosigManager.UpdateSosigTemplateDictionary();
		}

		public void FixedUpdate()
		{
			timeLeftToNextPhase -= Time.fixedDeltaTime;
			if (timeLeftToNextPhase < 0)
			{
				//currently in assualt; go to next assault
				if(isInAssault) StartNextAssault(currentAssault + 1);
				//currenlty in grace period; go to assault
				else StartAssault();
			}
			if (isInAssault)
			{
				UpdateAssault();
			}
		}

		public void Update()
		{
			UpdateVoice();
		}

		public void StartNextAssault(int nextAssault)
		{
			nextAssault = Mathf.Clamp(nextAssault, 0, assaults.Length - 1);
			//this really starts the grace period
			isInAssault = false;
			currentAssault = nextAssault;
			timeLeftToNextPhase = assaults[currentAssault].startingTimeGrace;
			timeBetweenPatrols = assaults[currentAssault].delayBetweenPatrols;
			if (nextAssault == 0) QueueLine(CookOffAudio.CookOffAudioLines.Event_BeginGame);
			else QueueLine(CookOffAudio.CookOffAudioLines.Event_AssaultEnd);
		}

		public void StartAssault()
		{
			//starts assault mode
			isInAssault = true;
			timeLeftToNextPhase = assaults[currentAssault].assaultLength;
			if(currentAssault == 0) QueueLine(CookOffAudio.CookOffAudioLines.Event_FirstAssault);
			else QueueLine(CookOffAudio.CookOffAudioLines.Event_NewAssault);
		}

		public void UpdateAssault()
		{
			CookOffAssault assault = assaults[currentAssault];
			if (timeSinceLastPatrol >= timeBetweenPatrols)
			{
				//spawn new patrol
				timeSinceLastPatrol = 0;
				//get patrol with weighted averages
				//CookOffPatrol patrol = assault.Patrols[0];
				//WeightedCalc.WeightedChanceExecutor wce = new WeightedCalc.WeightedChanceExecutor();
				//wce.Parameters = new WeightedCalc.WeightedChanceParam[assault.Patrols.Length - 1];
				//Debug.Log("Calculating random");
				/*for (int i = 0; i < assault.Patrols.Length; i++)
				{
					wce.Parameters[i] = new WeightedCalc.WeightedChanceParam(() =>
					{
						patrol = assault.Patrols[i];
					}, assault.Patrols[i].weight);
				}
				wce.Execute();*/
				//patrol spawn loc index
				//Debug.Log("Getting spawn point");
				//patrol = assault.Patrols[Random.Range(0, assault.Patrols.Length)];
				//int pSL = Random.Range(0, patrol.PatrolSpawnPoints.Length);
				//Debug.Log("Generating Patrol");
				//SosigManager.GeneratePatrol(patrol.Patrol, baseLoc.position, healthDropChance, healthPickupID, 0, patrol.PatrolSpawnPoints[pSL]);
				foreach (var spawner in assault.spawners)
				{
					spawner.SetActive(true);
				}
			}
			else
			{
				timeSinceLastPatrol += Time.fixedDeltaTime;
			}
		}

		public void UpdateVoice()
		{
			if (timeTilNextVoice >= 0f)
			{
				timeTilNextVoice -= Time.deltaTime;
				return;
			}
			if (VoiceQueue.Count > 0)
			{
				AudioEvent key = VoiceQueue.Dequeue();
				key.PitchRange = new Vector2(1f, 1f);
				key.VolumeRange = new Vector2(1f, 1f);
				float maxTime = 0;
				//get longest clip length in key
				foreach (var clip in key.Clips) if (clip.length > maxTime) maxTime = clip.length;
				timeTilNextVoice = maxTime + 1f;
				SM.PlayCoreSoundDelayed(FVRPooledAudioType.UIChirp, key, base.transform.position, 0.2f);
			}
		}

		public void QueueLine(CookOffAudio.CookOffAudioLines col)
		{
			VoiceQueue.Enqueue(cookOffAudio.GetAudio(col));
		}
	}
}