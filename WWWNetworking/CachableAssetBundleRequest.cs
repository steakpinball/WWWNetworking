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
		/// <summary>
		/// The version of the asset bundle to request
		/// </summary>
		/// <value>The version</value>
		public int Version { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:WWWNetworking.CachableAssetBundleRequest"/> class.
		/// </summary>
		/// <param name="url">URL for reques</param>
		/// <param name="version">Version of asset bundle</param>
		/// <param name="progress">Progress callback</param>
		/// <param name="success">Success callback</param>
		/// <param name="error">Error callback</param>
		public CachableAssetBundleRequest(string url, int version, Action<float> progress, Action<AssetBundle> success, Action<string> error) : base(url, progress, www => success(www.assetBundle), error)
		{
			Version = version;
		}

		#region Base Overrides

		/// <summary>
		/// Runs the request.
		/// </summary>
		/// <returns>Enumerator for Coroutine</returns>
		public override IEnumerator RunRequest()
		{
			using (var www = WWW.LoadFromCacheOrDownload(Url, Version)) {
				while (!www.isDone) {
					OnProgress(www.progress);
					yield return null;
				}
				OnProgress(www.progress);

				CheckErrorOrSuccess(www);
			}
		}

		#endregion
	}
}
