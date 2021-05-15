using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlockController : MonoBehaviour {
	public enum Type {
		NORMAL,
		DIE,
		FINISH
	}

	public Type type = Type.NORMAL;

	public GameObject box;

	public float yScale = 1.0f;
	float step = 17.0f;
	float delta = 0.3f;

	float yShift = 0.0f;

	Transform tbox;
    void Start() {
		tbox = box.GetComponent<Transform>();
	}

    void Update() {
		if (Math.Abs(tbox.localScale.y - yScale) <= delta) {
			tbox.localScale = new Vector3(1.0f, yScale, 1.0f);
			tbox.position = new Vector3(tbox.position.x, box.GetComponent<Transform>().localScale.y / 2.0f, tbox.position.z);
		}
		
		if (box.GetComponent<Transform>().localScale.y < yScale) {
			tbox.localScale = new Vector3(1.0f, 
				tbox.localScale.y+step*Time.deltaTime, 1.0f);
			tbox.position = new Vector3(tbox.position.x, box.GetComponent<Transform>().localScale.y / 2.0f, tbox.position.z);

			return;
		}
		else if (box.GetComponent<Transform>().localScale.y > yScale)
		{
			tbox.localScale = new Vector3(1.0f, 
				tbox.localScale.y - step * Time.deltaTime, 1.0f);
			tbox.position = new Vector3(tbox.position.x, box.GetComponent<Transform>().localScale.y / 2.0f, tbox.position.z);
			return;
		}

	}
}
