using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace H3VRMod.CookScripts
{
	public class CookStandImport : MonoBehaviour
	{
		public CookStand cookStand;
		public int importType;
		private void OnTriggerEnter(Collider other)
		{
			if (cookStand.waitingForIngredient)
			{
				CookStandIngredient ingredient = other.gameObject.GetComponent<CookStandIngredient>();
				if (ingredient != null)
				{
					if (ingredient.IngredientType == importType)
					{
						cookStand.AddIngredient(ingredient.IngredientType);
						ingredient.obj.ForceBreakInteraction();
						Destroy(ingredient.gameObject);
					}
				}
			}
		}
	}
}