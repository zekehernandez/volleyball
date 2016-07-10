using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	
	public class Character {
		
		private int mTeamNum;

		public Character(int teamNum) {
			mTeamNum = teamNum;
		}

	}
	
	// Public
	public string ButtonAxis = "Button_P1_A";
	public LayerMask GroundLayer;
	public int TeamNum = 1;
	public float InitialJumpStrength = 100;
	public float JumpStrength = 100;
	public Transform GroundCheck;
	
	// Private
	private Character mCharacter;
	private bool mIsButtonPressed;
	private bool mIsJumping;
	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		mCharacter = new Character(TeamNum);
		mIsJumping = false;
		
		rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		bool isGrounded = Physics2D.Linecast(transform.position, GroundCheck.position,  1 << LayerMask.NameToLayer("Ground"));
		
		if (isGrounded) {
			if (mIsButtonPressed && !mIsJumping) {
				rb2d.AddForce(new Vector2(0, InitialJumpStrength));	
				mIsJumping = true;
			} else if (mIsJumping) 
				mIsJumping = false;
		} else {
			if (mIsButtonPressed) {
				if (mIsJumping) {
					rb2d.AddForce(new Vector2(0, JumpStrength));
				}
			} else if (mIsJumping) 
				mIsJumping = false;
		}
	}

	void FixedUpdate() {
		mIsButtonPressed = Input.GetAxis(ButtonAxis) > 0;
	}
	
}
