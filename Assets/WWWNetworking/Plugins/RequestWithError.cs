using System;
using System.Collections;
using UnityEngine;

namespace WWWNetworking
{
	/// <summary>
	/// Runs a request with error checking.
	/// </summary>
	public class RequestWithError : Request
	{
		public Action<string> Error { get; private set; }

		public RequestWithError(string url, Action<WWW> success, Action<string> error) : base(url, success)
		{
			this.Error = error;
		}

		#region Base Overrides
		public override IEnumerator RunRequest()
		{
			using (var www = new WWW(Url)) {
				yield return www;

				FinalErrorCheck(www);
			}
		}
		#endregion

		protected void FinalErrorCheck(WWW www)
		{
			if (string.IsNullOrEmpty(www.error)) {
				// No error means success
				Success(www);
			} else {
				Error(www.error);
			}
		}
	}
}
