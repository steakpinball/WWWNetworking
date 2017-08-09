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
		public string Url { get; private set; }
		public Action<WWW> Success { get; private set; }

		public Request(string url, Action<WWW> success)
		{
			this.Url = url;
			this.Success = success;
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
					Success(www);
				}
			}
		}
		#endregion
	}
}