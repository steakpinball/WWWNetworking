using System;
using System.Collections;
using UnityEngine;

namespace WWWNetworking
{
	public class FormProgressRequest : FormRequest
	{
		public Action<float> UploadProgress { get; private set; }
		public Action<float> DownloadProgress { get; private set; }

		public FormProgressRequest(string url, WWWForm form, Action<float> uploadProgress, Action<float> downloadProgress, Action<WWW> success, Action<string> error) : base(url, form, success, error)
		{
			this.UploadProgress = uploadProgress;
			this.DownloadProgress = downloadProgress;
		}

		public override IEnumerator RunRequest()
		{
			using (var www = new WWW(Url, Form)) {
				while (!www.isDone && www.uploadProgress < 1) {
					UploadProgress(www.uploadProgress);
					yield return null;
				}
				UploadProgress(www.uploadProgress);

				while (!www.isDone) {
					DownloadProgress(www.progress);
					yield return null;
				}
				DownloadProgress(www.progress);

				FinalErrorCheck(www);
			}
		}
	}
}
