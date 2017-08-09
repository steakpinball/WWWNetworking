using System;
using UnityEngine;

namespace WWWNetworking
{
	public static class NetworkingEngineRequestExtensions
	{
		static public void Download(this NetworkingEngine ne, string url, Action<WWW> success)
		{
			ne.Add(new Request(url, success));
		}

		static public void DownloadCheckError(this NetworkingEngine ne, string url, Action<WWW> success, Action<string> error)
		{
			ne.Add(new RequestWithError(url, success, error));
		}

		static public void DownloadProgress(this NetworkingEngine ne, string url, Action<float> progress, Action<WWW> success, Action<string> error)
		{
			ne.Add(new ProgressRequest(url, progress, success, error));
		}

		static public void DownloadOrCacheAssetBundle(this NetworkingEngine ne, string url, int version, Action<float> progress, Action<AssetBundle> success, Action<string> error)
		{
			ne.Add(new CachableAssetBundleRequest(url, version, progress, success, error));
		}

		static public void UploadCheckError(this NetworkingEngine ne, string url, WWWForm form, Action<WWW> success, Action<string> error)
		{
			ne.Add(new FormRequest(url, form, success, error));
		}
		
		static public void UploadProgress(this NetworkingEngine ne, string url, WWWForm form, Action<float> uploadProgress, Action<float> downloadProgress, Action<WWW> success, Action<string> error)
		{
			ne.Add(new FormProgressRequest(url, form, uploadProgress, downloadProgress, success, error));
		}
	}
}
