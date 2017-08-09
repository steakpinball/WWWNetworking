using UnityEngine;

namespace WWWNetworking
{
	/// <summary>
	/// Provides singleton-like access to a NetworkingEngine instance.
	/// Instance does not automatically persist between scenes.
	/// </summary>
	public class NetworkingEngineSingleton : NetworkingEngine
	{
		static private NetworkingEngineSingleton s_instance;

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance</value>
		static public NetworkingEngineSingleton Instance
		{
			get
			{
				if (!s_instance) {
					new GameObject("NetworkingEngineSingleton").AddComponent<NetworkingEngineSingleton>();
				}

				return s_instance;
			}
		}

		#region MonoBehaviour Messages
		protected virtual void Awake()
		{
			if (null == s_instance) {
				s_instance = this;
			} else {
				Destroy(this);
			}
		}
		#endregion
	}
}
