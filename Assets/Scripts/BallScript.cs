using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {
	
	// Public
	public Vector2 StartPosition = new Vector2(-0.9f, 8.4f);
	public string ButtonAxis = "Button_Reset";
	
	// Private
	private bool mIsButtonPressed;
	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		mIsButtonPressed = false;
		rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (mIsButtonPressed) {
			transform.position = StartPosition;
			rb2d.Sleep();
			rb2d.WakeUp();
		}
	}
	
	void FixedUpdate() {
		mIsButtonPressed = Input.GetAxis(ButtonAxis) > 0;
	}
}
