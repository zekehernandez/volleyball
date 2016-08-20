using UnityEngine;

public class Player : MonoBehaviour {
	
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
	public int SwingDirection = 1;
	public int BumpDirection = -1;
	public int SwingAngle = 220;
	
	public float InitialJumpStrength = 100;
	public float JumpStrength = 100;
	public float HorizontalModifier = 1;
	public float HorizontalCap = 2000;
	public float HorizontalPostJump = 0.5f;
	public float SwingSpeed = 1500;
	public float SweetSpot = 1;
	
	public Transform GroundCheck;
	public Transform Ball;
	public Transform StartPosition;
	public Transform ServePosition;
	
	public GameController CurrentGameController;
	
	// Private
	private Character mCharacter;
	private bool mIsButtonPressed;
	private bool mIsJumping;
	private bool mIsSwinging;
	private bool mIsBumping;
	private Rigidbody2D rb2d;
	private HingeJoint2D armJoint;

	// Use this for initialization
	void Start () {
		mCharacter = new Character(TeamNum);
		mIsJumping = false;
		mIsSwinging = false;
		
		SweetSpot = SweetSpot * SwingDirection;
		
		rb2d = GetComponent<Rigidbody2D>();
		armJoint = transform.Find("Arm").transform.GetComponent<HingeJoint2D>();
		
		Physics2D.IgnoreCollision(Ball.GetComponent<Collider2D>(), GetComponent<Collider2D>());
	}
	
	void SetArmSwing () {
		JointAngleLimits2D limits = armJoint.limits;
		JointMotor2D motor = armJoint.motor;
		
		limits.max = SwingAngle * SwingDirection;
		armJoint.limits = limits;
		
		armJoint.useMotor = true;
		motor.motorSpeed = SwingSpeed * SwingDirection;
		armJoint.motor = motor;
		
		mIsSwinging = true;	
		mIsBumping = false;
	}
	
	void SetArmBump () {
		JointAngleLimits2D limits = armJoint.limits;
		JointMotor2D motor = armJoint.motor;
		
		limits.max = -45 * BumpDirection;
		armJoint.limits = limits;
		
		armJoint.useMotor = true;
		motor.motorSpeed = SwingSpeed * BumpDirection;
		armJoint.motor = motor;
		
		
		mIsBumping = true;
		mIsSwinging = false;
	}
	
	void SetArmRest() {
		JointAngleLimits2D limits = armJoint.limits;
		limits.max = 90 * SwingDirection;
		armJoint.limits = limits;
		armJoint.useMotor = false;
		
		mIsSwinging = false;
		mIsBumping = false;
	}
	
	bool BallIsOnMySide() {
		return ((StartPosition.position.x < 0) && (Ball.position.x <= 0))
			|| ((StartPosition.position.x > 0) && (Ball.position.x >= 0));
	}
	
	float GetHorizontalStrength(bool initial) {
		float horizontalSpot;
		float horizontalStrength;
		
		if (BallIsOnMySide()) {
			horizontalSpot = Ball.position.x;
		} else {
			horizontalSpot = StartPosition.position.x;
		}
		
		horizontalStrength = (horizontalSpot - (transform.position.x + SweetSpot)) * HorizontalModifier;
		if (Mathf.Abs(horizontalStrength) > HorizontalCap) {
			horizontalStrength = HorizontalCap * Mathf.Sign(horizontalStrength);	
		}
		
		if (!initial) {
			horizontalStrength = horizontalStrength * HorizontalPostJump;		
		}
		
		return horizontalStrength;
	}
	
	// Update is called once per frame
	void Update () {
		bool isGrounded = Physics2D.Linecast(transform.position, GroundCheck.position,  1 << LayerMask.NameToLayer("Ground"));
		bool shouldSwing = false;
		bool shouldBump = false;
		
		if (isGrounded) {
			if (mIsButtonPressed && !mIsJumping) {

				rb2d.AddForce(new Vector2(GetHorizontalStrength(true), InitialJumpStrength));	
				mIsJumping = true;
				shouldBump = true;
			} else if (mIsJumping) 
				mIsJumping = false;
		} else {
			if (mIsButtonPressed) {
				if (mIsJumping) {
					rb2d.AddForce(new Vector2(GetHorizontalStrength(false), JumpStrength));
					shouldBump = true;
				} else {
					shouldSwing = true;	
				}
			} else if (mIsJumping) 
				mIsJumping = false;
		}
		
		if ((mIsSwinging && !shouldSwing) || (mIsBumping && !shouldBump)){
			SetArmRest();
		} else if (!mIsSwinging && shouldSwing) {
			SetArmSwing();
		} else if (!mIsBumping && shouldBump) {
			SetArmBump();
		}
	}

	void FixedUpdate() {
		mIsButtonPressed = Input.GetAxis(ButtonAxis) > 0;
	}
	
	public void Reset(bool isServing) {
		if (isServing) {
			transform.position = ServePosition.position;
		} else {
			transform.position = StartPosition.position;
		}
		
		rb2d.Sleep();
		rb2d.WakeUp();
	}
	
}
