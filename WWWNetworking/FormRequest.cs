using System;
using System.Collections;
using UnityEngine;

namespace WWWNetworking
{
	/// <summary>
	/// Sends an upload as part of a request.
	/// </summary>
	public class FormRequest : RequestWithError
	{
		/// <summary>
		/// Form data.
		/// </summary>
		/// <value>Data</value>
		public WWWForm Form { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:WWWNetworking.FormRequest"/> class.
		/// </summary>
		/// <param name="url">URL for request</param>
		/// <param name="form">Form data</param>
		/// <param name="success">Success callback</param>
		/// <param name="error">Error callback</param>
		public FormRequest(string url, WWWForm form, Action<WWW> success, Action<string> error) : base(url, success, error)
		{
			Form = form;
		}

		/// <summary>
		/// Runs the request.
		/// </summary>
		/// <returns>Enumerator for coroutine</returns>
		public override IEnumerator RunRequest()
		{
			using (var www = new WWW(Url, Form)) {
				yield return www;

				CheckErrorOrSuccess(www);
			}
		}
	}
}
