using System;
using System.Collections;
using UnityEngine;

namespace WWWNetworking
{
	/// <summary>
	/// A basic download request.
	/// Downloads a file and runs some code afterward.
	/// </summary>
	public class Request : IRequest
	{
		/// <summary>
		/// The Url for the request.
		/// </summary>
		/// <value>The URL</value>
		public string Url { get; private set; }

		/// <summary>
		/// Success callback. Called after a request receives a success response.
		/// </summary>
		/// <value>Success callback</value>
		Action<WWW> m_Success { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:WWWNetworking.Request"/> class.
		/// </summary>
		/// <param name="url">URL for the request</param>
		/// <param name="success">Success callback</param>
		public Request(string url, Action<WWW> success)
		{
			Url = url;
			m_Success = success;
		}

		/// <summary>
		/// Invokes the success delegate.
		/// </summary>
		/// <param name="www">Www.</param>
		protected void OnSuccess(WWW www)
		{
			m_Success?.Invoke(www);
		}

		#region IRequest Implementations

		/// <summary>
		/// Downloads thing at provided url and runs success handler.
		/// </summary>
		public virtual IEnumerator RunRequest()
		{
			using (var www = new WWW(Url)) {
				yield return www;

				if (string.IsNullOrEmpty(www.error)) {
					// No error means success
					OnSuccess(www);
				}
			}
		}

		#endregion
	}
}