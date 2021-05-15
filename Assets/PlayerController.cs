using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {
	public MapController map;

	public ParticleSystem winParticles;
	public ParticleSystem loseParticles;

	public int x = 0;
	public int y = 0;
	float z = 0.0f;

	bool isInit = false;
	public bool isWin = false;
	float scale = 1.01f;
    void Start() {
		gameObject.transform.position = new Vector3(0, z, 0);
		winParticles.Stop();
		loseParticles.Stop();
	}

	float lastPress = 0.0f;

    void Update() {
		Drag();
		if (!isInit) {
			StartAnim();
			isInit = true;
		}
		if (Time.time - lastPress > 0.1f) {
			if ((Input.GetKeyUp(KeyCode.LeftArrow) || swipeDir==DraggedDirection.LEFT) && x > 0 && map.layers[map.curLayer][x - 1][y].size == 1) {
				x--;
				lastPress = Time.time;
			}
			if ((Input.GetKeyUp(KeyCode.RightArrow) || swipeDir == DraggedDirection.RIGHT) && x < 10 && map.layers[map.curLayer][x + 1][y].size == 1) {
				print(x);
				x++;
				lastPress = Time.time;
			}
			if ((Input.GetKeyUp(KeyCode.DownArrow) || swipeDir == DraggedDirection.DOWN) && y > 0 && map.layers[map.curLayer][x][y - 1].size == 1) {
				y--;
				lastPress = Time.time;
			}
			if ((Input.GetKeyUp(KeyCode.UpArrow) || swipeDir == DraggedDirection.UP) && y < 10 && map.layers[map.curLayer][x][y + 1].size == 1) {
				y++;
				lastPress = Time.time;
			}
			swipeDir = DraggedDirection.NONE;
		}
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(x, z, y), 5.0f * Time.deltaTime);
		transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(scale, scale, scale), 5.0f * Time.deltaTime);

	}

	public void restart() {
		EndAnim();
	}

	public void EndAnim() {
		if (isWin) {
			winParticles.Play();
		}
		else {
			loseParticles.Play();
		}
		scale = 0.01f;
		StartCoroutine(GoEnd());
	}

	IEnumerator GoEnd() {
		z = 5;
		yield return new WaitForSeconds(0.8f);
		StartAnim();
	}

	public void StartAnim() {
		scale = 1.01f;
		x = 0;
		y = 0;
		gameObject.transform.position = new Vector3(x, z, y);

		transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
		StartCoroutine(GoStart());
	}

	IEnumerator GoStart() {
		z = 1.5f;
		yield return new WaitForSeconds(0.5f);
		if (isWin) {
			winParticles.Stop();
		}
		else {
			loseParticles.Stop();
		}
		isWin = false;
	}

	private enum DraggedDirection {
		NONE,
		UP,
		DOWN,
		RIGHT,
		LEFT
	}
	DraggedDirection swipeDir = DraggedDirection.NONE;
	private DraggedDirection GetDragDirection(Vector3 dragVector) {
		float positiveX = Mathf.Abs(dragVector.x);
		float positiveY = Mathf.Abs(dragVector.y);
		DraggedDirection draggedDir;
		if (positiveX > positiveY) {
			draggedDir = (dragVector.x > 0) ? DraggedDirection.RIGHT : DraggedDirection.LEFT;
		}
		else {
			draggedDir = (dragVector.y > 0) ? DraggedDirection.UP : DraggedDirection.DOWN;
		}
		swipeDir = draggedDir;
		print(draggedDir);
		return draggedDir;
	}

	public const float MAX_SWIPE_TIME = 0.5f;
	public const float MIN_SWIPE_DISTANCE = 0.17f;
	Vector2 startPos;
	float startTime;
	public void Drag() {
		if (Input.touches.Length > 0) {
			Touch t = Input.GetTouch(0);
			if (t.phase == TouchPhase.Began) {
				startPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
				startTime = Time.time;
				print("START");
			}
			if (t.phase == TouchPhase.Ended) {
				if (Time.time - startTime > MAX_SWIPE_TIME) // press too long
					return;

				Vector2 endPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);

				Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

				if (swipe.magnitude < MIN_SWIPE_DISTANCE) // Too short swipe
					return;

				if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y)) {
					if (swipe.x > 0) {
						swipeDir = DraggedDirection.RIGHT;
					}
					else {
						swipeDir = DraggedDirection.LEFT;
					}
				}
				else {
					if (swipe.y > 0) {
						swipeDir = DraggedDirection.UP;
					}
					else {
						swipeDir = DraggedDirection.DOWN;
					}
				}
				print("END");
				print(swipeDir);
			}
		}
	}
}
