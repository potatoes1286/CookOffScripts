using System;
using FistVR;
using H3VRMod.GameModeScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace H3VRMod.CookScripts
{
	public class CookStand : MonoBehaviour
	{
		public Vector2 timeToNextIngredient;
		public float currentTimeToNextIngredient;
		private int wantedIngredient;
		public bool waitingForIngredient;
		private bool wrongIngredientSaid;
		
		
		public string exportID;
		public Transform exportLoc;
		public float secsToExport;
		public float secsWorking;
		
		
		public Animation workingAnim;
		public AudioSource audSource;
		public AudioClip WorkingAudio;

		public void FixedUpdate()
		{
			//play animation and sound if working; stop if not
			if (waitingForIngredient)
			{
				if (workingAnim.isPlaying)	workingAnim.Stop();
				if (audSource.isPlaying)	audSource.Stop();
			}
			else
			{
				if (!workingAnim.isPlaying) workingAnim.Play();
				if (!audSource.isPlaying)	audSource.Play();
			}
			//work if not empty
			if (!waitingForIngredient) { secsWorking += Time.fixedDeltaTime; }

			if (secsWorking >= secsToExport)
			{
				secsWorking = 0;
				Instantiate(IM.OD[exportID].GetGameObject(), exportLoc.position, exportLoc.rotation); //make export
			}
		}

		public void UpdateIngredients()
		{
			currentTimeToNextIngredient -= Time.fixedDeltaTime;
			//if time to next ing ran out + not set to not working, set it to not working
			if (!(currentTimeToNextIngredient > 0) && !waitingForIngredient)
			{
				waitingForIngredient = true;
			}
			//7.5 = correction time; announcer will correct himself if wrong ingredient is said
			if (currentTimeToNextIngredient < 7.5f)
			{
				if (wrongIngredientSaid)
				{
					// + 3 = correction line
					CookOffAudio.CookOffAudioLines cols = (CookOffAudio.CookOffAudioLines)wantedIngredient + 3;
					CookOffManager.com.QueueLine(cols);
					wrongIngredientSaid = false;
				}
			}
		}

		public void RequestIngrendient()
		{
			wantedIngredient = Random.Range(0, 3);
			var ingSaid = wantedIngredient;
			//in practice, because there's a 33% chance that the
			//correct ingredient is mistaken for the correct ingredient
			//its more 16.75% 
			if (Random.Range(0f, 1f) < 0.25f)
			{
				ingSaid = Random.Range(0, 3);
				if (wantedIngredient != ingSaid) wrongIngredientSaid = true;
			}
			//convert ints 0-2 to instructions and queues their lines
			CookOffAudio.CookOffAudioLines cols = (CookOffAudio.CookOffAudioLines)ingSaid;
			CookOffManager.com.QueueLine(cols);
		}
		
		public void AddIngredient(int IngredientType)
		{
			if(IngredientType != wantedIngredient) blowthefuckup();
			else
			{
				waitingForIngredient = false;
				currentTimeToNextIngredient = Random.Range(timeToNextIngredient.x, timeToNextIngredient.y);
				CookOffManager.com.QueueLine(CookOffAudio.CookOffAudioLines.Ingredient_Good);
			}
		}

		public void blowthefuckup()
		{
			CookOffManager.com.QueueLine(CookOffAudio.CookOffAudioLines.Ingredient_Bad);
			
			waitingForIngredient = false;
			currentTimeToNextIngredient = Random.Range(timeToNextIngredient.x, timeToNextIngredient.y);
		}
	}
}