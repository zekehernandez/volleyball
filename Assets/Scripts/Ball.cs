using UnityEngine;

public class Ball : MonoBehaviour {
	
	// Public
	public Transform RedStartPosition;
	public Transform BlueStartPosition;
	
	public GameController CurrentGameController;
	
	// public string ButtonAxis = "Button_Reset";
	
	// Private
	private bool mIsButtonPressed;
	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void FixedUpdate() {
	// 	mIsButtonPressed = Input.GetAxis(ButtonAxis) > 0;
	}
	
	// Public
	public void Reset(VolleyUtil.Team team) {
		if (team == VolleyUtil.Team.RedTeam) {
			transform.position = RedStartPosition.position;	
		} else {
			transform.position = BlueStartPosition.position;	
		}
			
		rb2d.Sleep();
		rb2d.WakeUp();	
	}
}
