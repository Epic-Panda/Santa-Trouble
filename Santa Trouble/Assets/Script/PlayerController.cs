using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	private Rigidbody rb;
	private Animator anim;

	[SerializeField]
	private float speed = 2.5f;

	[SerializeField]
	private float jumpHeight = 2f;

	[SerializeField]
	private int jumpNumber = 2;

	[SerializeField]
	private GameObject respawnPos;

	private Vector3 icyDir = Vector3.zero;
	private bool icy = false;

	private bool stop = false;
	private static PlayerController playerController;

	public static PlayerController Controller {
		get {
			if (playerController == null)
				playerController = GameObject.FindObjectOfType<PlayerController> ();
			return playerController;
		}
	}
	//TODO y<-3 pao je
	public bool Stop {
		get{ return stop; }
		set{ stop = value; }
	}

	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!stop) {
			if (rb.IsSleeping ())
				rb.WakeUp ();
			rotateAndMove ();
			fallCheck ();
		} else {
			rb.Sleep ();
			anim.SetBool ("jump", false);
			anim.SetBool ("run", false);
		}
	}

	/// <summary>
	/// Function that rotates and moves object;
	/// </summary>
	private void rotateAndMove ()
	{

		if (Input.GetMouseButtonDown (0) && jumpNumber > 0) {
			jumpNumber--;
			anim.SetBool ("jump", true);
			rb.AddForce (transform.up * Mathf.Sqrt (-jumpHeight * Physics.gravity.y), ForceMode.VelocityChange);
		}

		if (Input.GetMouseButton (1)) {
			transform.position += transform.forward * speed * Time.fixedDeltaTime;
			anim.SetBool ("run", true);
			icyDir = transform.forward * speed * Time.fixedDeltaTime;
		}

		if (Input.GetMouseButtonUp (1)) {
			anim.SetBool ("run", false);
		}

		if (Input.GetMouseButtonUp (0)) {
			anim.SetBool ("jump", false);
		}

		if (icy) {
			transform.position += icyDir;
		}

		transform.Rotate (new Vector3 (0, Input.GetAxis ("Mouse X"), 0));

		if (Input.anyKey) {
			if (Input.GetKey ("w") || Input.GetKey ("up")) {
				anim.SetBool ("run", true);
				transform.position += transform.forward * speed * Time.fixedDeltaTime;
				icyDir = transform.forward * speed * Time.fixedDeltaTime;
			}

			if (Input.GetKey ("s") || Input.GetKey ("down")) {
				anim.SetBool ("run", true);
				transform.position -= transform.forward * speed * Time.fixedDeltaTime;
				icyDir = -transform.forward * speed * Time.fixedDeltaTime;
			}

			if (Input.GetKey ("d") || Input.GetKey ("right")) {
				transform.Rotate (new Vector3 (0, 2, 0));
			}

			if (Input.GetKey ("a") || Input.GetKey ("left")) {
				transform.Rotate (new Vector3 (0, -2, 0));
			}

			if (Input.GetKeyDown ("space") && jumpNumber > 0) {
				jumpNumber--;
				anim.SetBool ("jump", true);
				rb.AddForce (transform.up * Mathf.Sqrt (-jumpHeight * Physics.gravity.y), ForceMode.VelocityChange);
			}
		}

		if (Input.GetKeyUp ("s") || Input.GetKeyUp ("down") || Input.GetKeyUp ("w") || Input.GetKeyUp ("up")) {
			anim.SetBool ("run", false);
		}

		if (Input.GetKeyUp ("space")) {
			anim.SetBool ("jump", false);
		}
	}

	private void fallCheck ()
	{
		if (transform.position.y <= -3) {
			GameController.Controller.addLife (-1);
			if (GameController.Controller.getLife () > 0)
				transform.position = respawnPos.transform.position;
			else
				GameController.Controller.GameEnd = true;
		}
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.tag == "Platform") {
			jumpNumber = 2;
			icy = false;
		} else if (collision.gameObject.tag == "Moving") {
			jumpNumber = 2;
			icy = false;
			transform.parent = collision.transform;
		} else if (collision.gameObject.tag == "Icy") {
			jumpNumber = 2;
			icy = true;
		}
	}

	void OnTriggerEnter (Collider coll)
	{
		if (coll.tag == "Finish") {
			GameController.Controller.GameEnd = true;
		}
	}

	void OnCollisionExit (Collision collision)
	{
		if (collision.gameObject.tag == "Moving")
			transform.parent = null;
	}
}