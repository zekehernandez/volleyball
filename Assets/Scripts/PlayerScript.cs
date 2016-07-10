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
	private bool mIsSwinging;
	private Rigidbody2D rb2d;
	private HingeJoint2D armJoint;

	// Use this for initialization
	void Start () {
		mCharacter = new Character(TeamNum);
		mIsJumping = false;
		mIsSwinging = false;
		
		rb2d = GetComponent<Rigidbody2D>();
		armJoint = transform.Find("Arm").transform.GetComponent<HingeJoint2D>();
	}
	
	void SetArmSwing () {
		JointAngleLimits2D limits = armJoint.limits;
		JointMotor2D motor = armJoint.motor;
		
		limits.max = 220;
		armJoint.limits = limits;
		
		armJoint.useMotor = true;
		
		mIsSwinging = true;	
	}
	
	void SetArmRest() {
		JointAngleLimits2D limits = armJoint.limits;
		limits.max = 90;
		armJoint.limits = limits;
		armJoint.useMotor = false;
		
		mIsSwinging = false;
	}
	
	// Update is called once per frame
	void Update () {
		bool isGrounded = Physics2D.Linecast(transform.position, GroundCheck.position,  1 << LayerMask.NameToLayer("Ground"));
		bool shouldSwing = false;
		
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
				} else {
					shouldSwing = true;	
				}
			} else if (mIsJumping) 
				mIsJumping = false;
		}
		
		if (mIsSwinging && !shouldSwing) {
			SetArmRest();
		} else if (!mIsSwinging && shouldSwing) {
			SetArmSwing();
		}
	}

	void FixedUpdate() {
		mIsButtonPressed = Input.GetAxis(ButtonAxis) > 0;
	}
	
}
