using System;
using System.Collections;
using UnityEngine;

namespace WWWNetworking
{
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
					Progress(www.progress);
					yield return null;
				}
				Progress(www.progress);

				FinalErrorCheck(www);
			}
		}
	}
}
