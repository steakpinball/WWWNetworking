using System;
using System.Collections;
using UnityEngine;

namespace WWWNetworking
{
	public class FormRequest : RequestWithError
	{
		public WWWForm Form { get; private set; }

		public FormRequest(string url, WWWForm form, Action<WWW> success, Action<string> error) : base(url, success, error)
		{
			this.Form = form;
		}

		public override IEnumerator RunRequest()
		{
			using (var www = new WWW(Url, Form)) {
				yield return www;

				FinalErrorCheck(www);
			}
		}
	}
}
