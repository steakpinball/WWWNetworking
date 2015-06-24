using System.Collections;

/// <summary>
/// A request must be able to run.
/// </summary>
public interface IRequest
{
	/// <summary>
	/// A coroutine which runs the request.
	/// </summary>
	/// <returns>Enumerator for coroutine</returns>
	IEnumerator RunRequest();
}
