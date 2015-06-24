using UnityEngine;

namespace WWWNetworking
{
	/// <summary>
	/// Provides singleton-like access to a NetworkingEngine instance.
	/// Instance does not automatically persist between scenes.
	/// </summary>
	public class NetworkingEngineSingleton : NetworkingEngine
	{
		static private NetworkingEngineSingleton sInstance;

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance</value>
		static public NetworkingEngineSingleton Instance
		{
			get
			{
				if (!sInstance) {
					new GameObject("NetworkingEngineSingleton").AddComponent<NetworkingEngineSingleton>();
				}

				return sInstance;
			}
		}

		#region MonoBehaviour Messages
		protected virtual void Awake()
		{
			if (null == sInstance) {
				sInstance = this;
			} else {
				Destroy(this);
			}
		}
		#endregion
	}
}
