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
		private int m_maxConcurrent = 1;

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

		/// <summary>
		/// Occurs when all queued requests have finished or been cancelled.
		/// </summary>
		public event Action AllCompleted;

		private Queue<IRequest> m_queue = new Queue<IRequest>();
		private int m_currentCount = 0;

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
			m_currentCount = 0;
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
			while (m_currentCount < m_maxConcurrent && 0 < m_queue.Count) {
				ProcessNext();
			}
		}

		private void CheckProcessNext()
		{
			if (m_currentCount < m_maxConcurrent && 0 < m_queue.Count) {
				ProcessNext();
			}
		}

		private void ProcessNext()
		{
			Process(m_queue.Dequeue());
		}

		private void Process(IRequest request)
		{
			StartCoroutine(ProcessWrapper(request));
		}

		private IEnumerator ProcessWrapper(IRequest request)
		{
			++m_currentCount;

			yield return StartCoroutine(request.RunRequest());

			--m_currentCount;

			CheckProcessNext();

			if (0 == m_currentCount) {
				// Finished
				OnAllCompleted();
			}
		}

		#region Base Overrides
		protected virtual void OnValidate()
		{
			// Max concurrent can not be negative
			m_maxConcurrent = Math.Max(0, m_maxConcurrent);
		}
		#endregion
	}
}
