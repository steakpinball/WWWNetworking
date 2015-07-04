using System;
using System.Collections;
using UnityEngine;

namespace WWWNetworking
{
	/// <summary>
	/// Loads <code>AssetBundle</code> from cache or downloads if not cached.
	/// Given that asset bundles are typically large, there is no version which doesn't report progress.
	/// </summary>
	public class CachableAssetBundleRequest : ProgressRequest
	{
		public int Version { get; private set; }
		public new Action<AssetBundle> Success { get; private set; }

		public CachableAssetBundleRequest(string url, int version, Action<float> progress, Action<AssetBundle> success, Action<string> error) : base(url, progress, null, error)
		{
			this.Version = version;
			this.Success = success;
		}

		#region Base Overrides
		public override IEnumerator RunRequest()
		{
			using (var www = WWW.LoadFromCacheOrDownload(Url, Version)) {
				while (!www.isDone) {
					OnProgress(www.progress);
					yield return null;
				}
				OnProgress(www.progress);

				if (string.IsNullOrEmpty(www.error)) {
					// No error means success
					Success(www.assetBundle);
				} else {
					Error(www.error);
				}
			}
		}
		#endregion
	}
}
