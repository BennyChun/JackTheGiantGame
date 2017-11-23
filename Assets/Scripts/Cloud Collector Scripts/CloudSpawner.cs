using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour {

	[SerializeField]
	private GameObject[] clouds;

	// distance between y axis of the clouds
	private float distanceBetweenClouds = 3f;

	// to prevent clouds to go outside of camera
	private float minX, maxX;

	// to indicate we need to spawn more clouds
	private float lastCloudPosY;

	[SerializeField]
	private GameObject[] collectables;

	// to control x position of cloud so clouds dont spawn directly under another
	private float controlX;

	private GameObject player;

	private void OnTriggerEnter2D(Collider2D target) {
		if (target.tag == "Cloud" || target.tag == "Deadly") {
			//target.gameObject.SetActive();
		}
	}

}
