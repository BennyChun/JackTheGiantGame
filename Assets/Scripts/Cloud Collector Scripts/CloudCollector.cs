using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCollector : MonoBehaviour {

	//the target is the cloud that collides with the collector
	private void OnTriggerEnter2D(Collider2D target) {
		if (target.tag == "Cloud" || target.tag == "Deadly") {
			target.gameObject.SetActive(false);
		}
	}

}
