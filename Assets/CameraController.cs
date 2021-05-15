using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public float shiftX = 10;
	public float shiftY = 10;
	public float shiftZ = 10;
	public Transform player;
    void Start() {
		transform.position = new Vector3(player.position.x + shiftX, player.position.x + shiftY, player.position.z + shiftZ);
    }

    void Update() {
		transform.position = new Vector3(player.position.x + shiftX, player.position.y + shiftY, player.position.z + shiftZ);
	}
}
