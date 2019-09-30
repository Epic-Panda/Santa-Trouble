using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField]
	private GameObject player;

	[SerializeField]
	private Vector3 offset;

	// Use this for initialization
	void Start ()
	{
		if (player == null)
			player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		Vector3 pos = player.transform.right * offset.x + player.transform.up * offset.y + player.transform.forward * offset.z;
		transform.position = player.transform.position + pos;

		Vector3 targetRot = new Vector3 (transform.rotation.x, player.transform.rotation.y, transform.rotation.z);
		Quaternion lookAt = Quaternion.LookRotation (targetRot);
		transform.rotation = lookAt;
	}
}
