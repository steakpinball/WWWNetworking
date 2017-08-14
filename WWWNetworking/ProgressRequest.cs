using System;
using System.Collections;
using UnityEngine;

namespace WWWNetworking
{
	/// <summary>
	/// Reports download progress.
	/// </summary>
	public class ProgressRequest : RequestWithError
	{
		/// <summary>
		/// Progress callback. Called each frame when progress updates.
		/// </summary>
		/// <value>Progress callback</value>
		Action<float> m_Progress { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:WWWNetworking.ProgressRequest"/> class.
		/// </summary>
		/// <param name="url">URL for request</param>
		/// <param name="progress">Progress callback</param>
		/// <param name="success">Success callback</param>
		/// <param name="error">Error callback</param>
		public ProgressRequest(string url, Action<float> progress, Action<WWW> success, Action<string> error) : base(url, success, error)
		{
			m_Progress = progress;
		}

		#region Overrides

		/// <summary>
		/// Runs the request.
		/// </summary>
		/// <returns>Enumerator for coroutine</returns>
		public override IEnumerator RunRequest()
		{
			using (var www = new WWW(Url)) {
				while (!www.isDone) {
					OnProgress(www.progress);
					yield return null;
				}
				OnProgress(www.progress);

				CheckErrorOrSuccess(www);
			}
		}

		#endregion

		/// <summary>
		/// Invokes the progress callback.
		/// </summary>
		/// <param name="progress">Progress as a value from 0 to 1</param>
		protected void OnProgress(float progress)
		{
			m_Progress?.Invoke(progress);
		}
	}
}
