using System;
using System.Collections;
using UnityEngine;

namespace WWWNetworking
{
	/// <summary>
	/// Request which requires an upload.
	/// </summary>
	public class FormProgressRequest : FormRequest
	{
		/// <summary>
		/// Upload progress callback.
		/// </summary>
		/// <value>Callback</value>
		Action<float> m_UploadProgress { get; set; }

		/// <summary>
		/// Download progress callback.
		/// </summary>
		/// <value>Callback</value>
		Action<float> m_DownloadProgress { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:WWWNetworking.FormProgressRequest"/> class.
		/// </summary>
		/// <param name="url">URL for request</param>
		/// <param name="form">Form data</param>
		/// <param name="uploadProgress">Upload progress callback</param>
		/// <param name="downloadProgress">Download progress callback</param>
		/// <param name="success">Success callback</param>
		/// <param name="error">Error callback</param>
		public FormProgressRequest(string url, WWWForm form, Action<float> uploadProgress, Action<float> downloadProgress, Action<WWW> success, Action<string> error) : base(url, form, success, error)
		{
			m_UploadProgress = uploadProgress;
			m_DownloadProgress = downloadProgress;
		}

		/// <summary>
		/// Invokes the upload progress callback.
		/// </summary>
		/// <param name="progress">Progress as a percentage from 0 to 1</param>
		protected void OnUploadProgress(float progress)
		{
			m_UploadProgress?.Invoke(progress);
		}

		/// <summary>
		/// Invokes the download progress delegate.
		/// </summary>
		/// <param name="progress">Progress as a percentage from 0 to 1</param>
		protected void OnDownloadProgress(float progress)
		{
			m_DownloadProgress?.Invoke(progress);
		}

		#region Overrides

		/// <summary>
		/// Runs the request.
		/// </summary>
		/// <returns>Enumerator for coroutine</returns>
		public override IEnumerator RunRequest()
		{
			using (var www = new WWW(Url, Form))
			{
				while (!www.isDone && www.uploadProgress < 1)
				{
					OnUploadProgress(www.uploadProgress);
					yield return null;
				}
				OnUploadProgress(www.uploadProgress);

				while (!www.isDone)
				{
					OnDownloadProgress(www.progress);
					yield return null;
				}
				OnDownloadProgress(www.progress);

				CheckErrorOrSuccess(www);
			}
		}

		#endregion
	}
}
