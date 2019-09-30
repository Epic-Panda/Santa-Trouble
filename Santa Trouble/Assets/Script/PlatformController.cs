using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{

	[SerializeField]
	private Transform start;

	[SerializeField]
	private Transform end;

	[SerializeField]
	private float speed = 2;

	private bool toEnd = true;

	private bool stop = false;
	private static PlatformController[] platformController;

	public static PlatformController[] Controller {
		get {
			if (platformController == null)
				platformController = GameObject.FindObjectsOfType<PlatformController> ();
			return platformController;
		}
	}

	public bool Stop {
		get{ return stop; }
		set{ stop = value; }
	}

	// Use this for initialization
	void Start ()
	{
		StartCoroutine (movePlatform ());
	}

	private IEnumerator movePlatform ()
	{
		while (true) {
			while (!stop && toEnd) {
				transform.position = Vector3.MoveTowards (transform.position, end.position, speed * Time.fixedDeltaTime);

				if (transform.position == end.position) {
					toEnd = false;
					yield return new WaitForSeconds (2);
				}

				yield return new WaitForEndOfFrame ();
			}

			while (!stop && !toEnd) {
				transform.position = Vector3.MoveTowards (transform.position, start.position, speed * Time.fixedDeltaTime);

				if (transform.position == start.position) {
					toEnd = true;
					yield return new WaitForSeconds (2);
				}
				yield return new WaitForEndOfFrame ();
			}

			// escape from infinite loop
			while (stop) {
				yield return new WaitUntil (() => stop == false);
			}
		}
	}
}