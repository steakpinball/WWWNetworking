using UnityEngine;

namespace WWWNetworking
{
	/// <summary>
	/// Provides singleton-like access to a NetworkingEngine instance.
	/// Instance does not automatically persist between scenes.
	/// </summary>
	public class NetworkingEngineSingleton : NetworkingEngine
	{
		static NetworkingEngineSingleton s_Instance;

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance</value>
		static public NetworkingEngineSingleton Instance
		{
			get
			{
				if (!s_Instance) {
					s_Instance = Object.FindObjectOfType<NetworkingEngineSingleton>();

					if (!s_Instance) {
						s_Instance = new GameObject(nameof(NetworkingEngineSingleton)).AddComponent<NetworkingEngineSingleton>();
					}
				}

				return s_Instance;
			}
		}

		#region MonoBehaviour Messages

		/// <summary>
		/// Called by engine.
		/// </summary>
		protected virtual void Awake()
		{
			if (null == s_Instance) {
				s_Instance = this;
			} else {
				Destroy(gameObject);
			}
		}

		#endregion
	}
}
