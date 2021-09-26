using System.Collections.Generic;
using FistVR;
using UnityEngine;

namespace H3VRMod.GameModeScripts
{
	[System.Serializable]
	//[CreateAssetMenu(fileName = "New Cookoff Audio", menuName = "CookOff/CookOffAudio", order = 0)]
	public class CookOffAudio : MonoBehaviour
	{
		[Header("Instructions")]
		public AudioEvent instructionSpice;
		public AudioEvent instructionGroundMeat;
		public AudioEvent instructionSausageLink;
		public AudioEvent correctionSpice;
		public AudioEvent correctionGroundMeat;
		public AudioEvent correctionSausageLink;
		[Header("Events")]
		public AudioEvent beginGame;
		public AudioEvent firstAssault;
		public AudioEvent newAssault;
		public AudioEvent assaultEnd;
		public AudioEvent vanArrive;
		public AudioEvent vanLeave;
		public AudioEvent badIngredient;
		public AudioEvent goodIngredient;
		[Header("Announcer Change")]
		public AudioEvent announcerPickUp;
		public AudioEvent announcerLeave;

		public enum CookOffAudioLines
		{
			Instruction_Spice = 0,
			Instruction_GroundMeat = 1,
			Instruction_SausageLink = 2,
			Correction_Spice = 3,
			Correction_GroundMeat = 4,
			Correction_SausageLink = 5,
			Event_BeginGame,
			Event_FirstAssault,
			Event_NewAssault,
			Event_AssaultEnd,
			Event_VanArrive,
			Event_VanLeave,
			Announcer_PickUp,
			Announcer_Leave,
			Ingredient_Bad,
			Ingredient_Good
		}

		public AudioEvent GetAudio(CookOffAudioLines col)
		{
			//fuck you
			//*makes a 700 line long switch statement*
			switch (col)
			{
				case CookOffAudioLines.Instruction_Spice:
					return instructionSpice;
				case CookOffAudioLines.Instruction_GroundMeat:
					return instructionGroundMeat;
				case CookOffAudioLines.Instruction_SausageLink:
					return instructionSausageLink;
				case CookOffAudioLines.Correction_Spice:
					return correctionSpice;
				case CookOffAudioLines.Correction_GroundMeat:
					return correctionGroundMeat;
				case CookOffAudioLines.Correction_SausageLink:
					return correctionSausageLink;
				case CookOffAudioLines.Event_BeginGame:
					return beginGame;
				case CookOffAudioLines.Event_FirstAssault:
					return firstAssault;
				case CookOffAudioLines.Event_NewAssault:
					return newAssault;
				case CookOffAudioLines.Event_AssaultEnd:
					return assaultEnd;
				case CookOffAudioLines.Event_VanArrive:
					return vanArrive;
				case CookOffAudioLines.Event_VanLeave:
					return vanLeave;
				case CookOffAudioLines.Announcer_PickUp:
					return announcerPickUp;
				case CookOffAudioLines.Announcer_Leave:
					return announcerLeave;
				case CookOffAudioLines.Ingredient_Bad:
					return badIngredient;
				case CookOffAudioLines.Ingredient_Good:
					return goodIngredient;
			}
			return null;
		}
	}
}