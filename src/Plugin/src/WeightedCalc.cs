using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace H3VRMod
{
	public class WeightedCalc : MonoBehaviour
	{
		public class WeightedChanceParam
		{
			public Action Func { get; }
			public double Ratio { get; }

			public WeightedChanceParam(Action func, double ratio)
			{
				Func = func;
				Ratio = ratio;
			}
		}

		public class WeightedChanceExecutor
		{
			public WeightedChanceParam[] Parameters { get; set; }
			private Random r;

			public double RatioSum
			{
				get { return Parameters.Sum(p => p.Ratio); }
			}

			public WeightedChanceExecutor(params WeightedChanceParam[] parameters)
			{
				Parameters = parameters;
			}

			public void Execute()
			{
				double numericValue = Random.Range(0f, 1f) * RatioSum;

				foreach (var parameter in Parameters)
				{
					numericValue -= parameter.Ratio;

					if (!(numericValue <= 0))
						continue;

					parameter.Func();
					return;
				}

			}
		}
	}
}