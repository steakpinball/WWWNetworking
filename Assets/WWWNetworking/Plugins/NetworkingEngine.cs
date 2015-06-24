using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WWWNetworking
{
	/// <summary>
	/// Manages network requests by preventing too many from running at once.
	/// Technically can manage anything which runs as a coroutine.
	/// </summary>
	public class NetworkingEngine : MonoBehaviour
	{
		/// <summary>
		/// Maximum number of concurrent downloads.
		/// </summary>
		[SerializeField]
		[Tooltip("Maximum number of concurrent downloads.")]
		private int maxConcurrent = 1;

		/// <summary>
		/// Gets or sets maximum number of concurrently running requests.
		/// Increasing may cause more requests to run.
		/// Decreasing does not cancel any already running requests.
		/// Setting to 0 prevents new requests until set back to positive number.
		/// </summary>
		/// <value>Maximum number of concurrent running requests</value>
		public int MaxConcurrent
		{
			get { return maxConcurrent; }
			set
			{
				maxConcurrent = Math.Max(0, value);
				CheckProcess();
			}
		}

		/// <summary>
		/// Occurs when all queued requests have finished or been cancelled.
		/// </summary>
		public event Action AllCompleted;

		private Queue<IRequest> mQueue = new Queue<IRequest>();
		private int mCurrentCount = 0;

		/// <summary>
		/// Adds a request to the queue or starts it if possible.
		/// </summary>
		/// <param name="request">Request</param>
		public void Add(IRequest request)
		{
			mQueue.Enqueue(request);
			CheckProcessNext();
		}

		/// <summary>
		/// Cancels all progressing and queued requests.
		/// </summary>
		public void CancelAll()
		{
			mQueue.Clear();
			StopAllCoroutines();
			mCurrentCount = 0;
			OnAllCompleted();
		}

		protected void OnAllCompleted()
		{
			if (null != AllCompleted) {
				AllCompleted();
			}
		}

		private void CheckProcess()
		{
			while (mCurrentCount < maxConcurrent && 0 < mQueue.Count) {
				ProcessNext();
			}
		}

		private void CheckProcessNext()
		{
			if (mCurrentCount < maxConcurrent && 0 < mQueue.Count) {
				ProcessNext();
			}
		}

		private void ProcessNext()
		{
			Process(mQueue.Dequeue());
		}

		private void Process(IRequest request)
		{
			StartCoroutine(ProcessWrapper(request));
		}

		private IEnumerator ProcessWrapper(IRequest request)
		{
			++mCurrentCount;

			yield return StartCoroutine(request.RunRequest());

			--mCurrentCount;

			CheckProcessNext();

			if (0 == mCurrentCount) {
				// Finished
				OnAllCompleted();
			}
		}

		#region Base Overrides
		protected virtual void OnValidate()
		{
			// Max concurrent can not be negative
			maxConcurrent = Math.Max(0, maxConcurrent);
		}
		#endregion
	}
}
