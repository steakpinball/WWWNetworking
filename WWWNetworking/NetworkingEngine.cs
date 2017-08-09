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
		int m_maxConcurrent = 1;

		/// <summary>
		/// Gets or sets maximum number of concurrently running requests.
		/// Increasing may cause more requests to run.
		/// Decreasing does not cancel any already running requests.
		/// Setting to 0 prevents new requests until set back to positive number.
		/// </summary>
		/// <value>Maximum number of concurrent running requests</value>
		public int MaxConcurrent
		{
			get { return m_maxConcurrent; }
			set
			{
				m_maxConcurrent = Math.Max(0, value);
				CheckProcess();
			}
		}

		public int ActiveCount { get; private set; }

		/// <summary>
		/// Occurs when all queued requests have finished or been cancelled.
		/// </summary>
		public event Action AllCompleted;

		Queue<IRequest> m_queue = new Queue<IRequest>();

		public int QueuedCount { get { return m_queue.Count; } }

		/// <summary>
		/// Adds a request to the queue or starts it if possible.
		/// </summary>
		/// <param name="request">Request</param>
		public void Add(IRequest request)
		{
			m_queue.Enqueue(request);
			CheckProcessNext();
		}

		/// <summary>
		/// Cancels all progressing and queued requests.
		/// </summary>
		public void CancelAll()
		{
			m_queue.Clear();
			StopAllCoroutines();
			ActiveCount = 0;
			OnAllCompleted();
		}

		protected void OnAllCompleted()
		{
			if (null != AllCompleted) {
				AllCompleted();
			}
		}

		void CheckProcess()
		{
			while (ActiveCount < m_maxConcurrent && 0 < m_queue.Count) {
				ProcessNext();
			}
		}

		void CheckProcessNext()
		{
			if (ActiveCount < m_maxConcurrent && 0 < m_queue.Count) {
				ProcessNext();
			}
		}

		void ProcessNext()
		{
			Process(m_queue.Dequeue());
		}

		void Process(IRequest request)
		{
			StartCoroutine(ProcessWrapper(request));
		}

		IEnumerator ProcessWrapper(IRequest request)
		{
			++ActiveCount;

			yield return StartCoroutine(request.RunRequest());

			--ActiveCount;

			CheckProcessNext();

			if (0 == ActiveCount) {
				// Finished
				OnAllCompleted();
			}
		}

		#region Overrides
		protected virtual void OnValidate()
		{
			// Max concurrent can not be negative
			m_maxConcurrent = Math.Max(0, m_maxConcurrent);
		}
		#endregion
	}
}
