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


	private void Awake() {
		controlX = 0f;
		SetMinAndMaxX();
		CreateClouds();

		player = GameObject.Find("Player");
	}

	private void Start() {
		PositionPlayer();
	}

	void SetMinAndMaxX() {
		Vector3 bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

		maxX = bounds.x - 0.5f;
		minX = -bounds.x + 0.5f;
	}

	void Shuffle(GameObject[] cloudArray) {
		for (int i = 0; i < cloudArray.Length; i++) {
			GameObject temp = cloudArray[i];
			int randomIndex = Random.Range(i, cloudArray.Length);
			cloudArray[i] = cloudArray[randomIndex];
			cloudArray[randomIndex] = temp;
		}
	}

	void CreateClouds() {

		Shuffle(clouds);

		float posY = 0f;

		for (int i = 0; i < clouds.Length; i++) {

			//position of cloud at element i
			Vector3 temp = clouds[i].transform.position;

			temp.y = posY;

			// generate clouds to the left and right of the y axis using controlX as a control variable
			if (controlX == 0) {
				temp.x = Random.Range(0.0f, maxX);
				controlX = 1;
			} else if (controlX == 1) {
				temp.x = Random.Range(0.0f, minX);
				controlX = 2;
			} else if (controlX == 2) {
				temp.x = Random.Range(1.0f, maxX);
				controlX = 3;
			} else if (controlX == 3) {
				temp.x = Random.Range(-1.0f, minX);
				controlX = 0;
			}
			//store last y coordinate of cloud
			lastCloudPosY = posY;

			clouds[i].transform.position = temp;

			posY -= distanceBetweenClouds;
		}

	}

	private void OnTriggerEnter2D(Collider2D target) {
		if (target.tag == "Cloud" || target.tag == "Deadly") {
			if (target.transform.position.y == lastCloudPosY) {
				Shuffle(clouds);
				Shuffle(collectables);

				Vector3 temp = target.transform.position;

				for (int i = 0; i < clouds.Length; i++) {
					if (!clouds[i].activeInHierarchy) {
						// generate clouds to the left and right of the y axis using controlX as a control variable
						if (controlX == 0) {
							temp.x = Random.Range(0.0f, maxX);
							controlX = 1;
						} else if (controlX == 1) {
							temp.x = Random.Range(0.0f, minX);
							controlX = 2;
						} else if (controlX == 2) {
							temp.x = Random.Range(1.0f, maxX);
							controlX = 3;
						} else if (controlX == 3) {
							temp.x = Random.Range(-1.0f, minX);
							controlX = 0;
						}

						temp.y -= distanceBetweenClouds;

						lastCloudPosY = temp.y;

						clouds[i].transform.position = temp;
						clouds[i].SetActive(true);
					}
				}

			}
		}
	}

	void PositionPlayer() {
		GameObject[] darkClouds = GameObject.FindGameObjectsWithTag("Deadly");
		GameObject[] cloudsInGame = GameObject.FindGameObjectsWithTag("Cloud");

		for(int i = 0; i < darkClouds.Length; i++) {

			if (darkClouds[i].transform.position.y == 0f) {

				//store dark cloud position
				Vector3 t = darkClouds[i].transform.position;
				
				// swap places with the nearest non deadly cloud
				darkClouds[i].transform.position = new Vector3(cloudsInGame[0].transform.position.x, 
																cloudsInGame[0].transform.position.y,
																cloudsInGame[0].transform.position.z);

				cloudsInGame[0].transform.position = t;

			}
		}

		Vector3 temp = cloudsInGame[0].transform.position;

		for (int i = 1; i < cloudsInGame.Length; i++) {
			if (temp.y < cloudsInGame[i].transform.position.y) {
				temp = cloudsInGame[i].transform.position;
			}
		}

		//so player sits on top of the cloud
		temp.y += 0.8f;

		player.transform.position = temp;
	}

	

}
