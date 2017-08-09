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
		public Action<float> Progress { get; private set; }

		public ProgressRequest(string url, Action<float> progress, Action<WWW> success, Action<string> error) : base(url, success, error)
		{
			this.Progress = progress;
		}

		public override IEnumerator RunRequest()
		{
			using (var www = new WWW(Url)) {
				while (!www.isDone) {
					OnProgress(www.progress);
					yield return null;
				}
				OnProgress(www.progress);

				FinalErrorCheck(www);
			}
		}

		protected void OnProgress(float progress)
		{
			if (null != Progress) {
				Progress(progress);
			}
		}
	}
}
