using UnityEngine;

namespace JuiceInternal
{
	public abstract class ModuleBase : MonoBehaviour
	{
		protected virtual void OnSetup()
		{
		}

		protected virtual void OnGameStarted()
		{
		}
	}
}
