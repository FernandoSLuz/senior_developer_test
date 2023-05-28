using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityChallenge
{
	public class ShaderLoop : MonoBehaviour
	{
		public Material loopMaterial;
		public string loopParameterName;

		private void OnEnable()
		{
			StartCoroutine(LoopRoutine(2.5f));
		}
		private void OnDisable()
		{
			StopAllCoroutines();
		}

		public IEnumerator LoopRoutine(float fadeDuration) {
			while (true) { 
				float time = 0;
				while (time < fadeDuration) {
					time += Time.deltaTime;
					loopMaterial.SetFloat(loopParameterName, time / fadeDuration);
					yield return null;
				}
				time = 0;
				while (time < fadeDuration) {
					time += Time.deltaTime;
					loopMaterial.SetFloat(loopParameterName, 1 - (time / fadeDuration));
					yield return null;
				}
			}
		}
	}
}