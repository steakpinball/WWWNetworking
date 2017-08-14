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
		/// <summary>
		/// Error callback. Called when a request fails. the parameter is the reason for failure.
		/// </summary>
		/// <value>Error callback</value>
		Action<string> m_Error { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:WWWNetworking.RequestWithError"/> class.
		/// </summary>
		/// <param name="url">URL for request</param>
		/// <param name="success">Success callback</param>
		/// <param name="error">Error callback</param>
		public RequestWithError(string url, Action<WWW> success, Action<string> error) : base(url, success)
		{
			m_Error = error;
		}

		/// <summary>
		/// Invokes the error delegate.
		/// </summary>
		/// <param name="error">Error text</param>
		protected void OnError(string error)
		{
			m_Error?.Invoke(error);
		}

		#region Base Overrides

		/// <summary>
		/// Runs the request.
		/// </summary>
		/// <returns>Enumerator for coroutine</returns>
		public override IEnumerator RunRequest()
		{
			using (var www = new WWW(Url)) {
				yield return www;

				CheckErrorOrSuccess(www);
			}
		}
		#endregion

		/// <summary>
		/// Performs a final error check.
		/// </summary>
		/// <param name="www">Object to check</param>
		protected void CheckErrorOrSuccess(WWW www)
		{
			if (string.IsNullOrEmpty(www.error)) {
				// No error means success
				OnSuccess(www);
			} else {
				OnError(www.error);
			}
		}
	}
}
