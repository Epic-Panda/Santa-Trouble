using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour
{

	[SerializeField]
	private Vector3 rotate = Vector3.zero;

	[SerializeField]
	private int speed = 2;

	[SerializeField]
	private int points = 10;

	private bool stop = false;
	private static CollectableController[] collectableController;

	public static CollectableController[] Controller {
		get {
			if (collectableController == null)
				collectableController = GameObject.FindObjectsOfType<CollectableController> ();
			return collectableController;
		}
	}

	public bool Stop {
		get{ return stop; }
		set{ stop = value; }
	}

	// Use this for initialization
	void Start ()
	{
		
	}

	/*void OnCollisionEnter (Collision coll)
	{
		
		if (coll.gameObject.tag == "Player") {
			GameController.Controller.AddScore (points);
			Destroy (gameObject);
		}
	}*/

	void OnTriggerEnter (Collider collider)
	{
		if (collider.tag == "Player") {
			GameController.Controller.addScore (points);
			Destroy (gameObject);
		}
	}

	void Update ()
	{
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		if (!stop)
			transform.Rotate (rotate * speed);
	}
}
