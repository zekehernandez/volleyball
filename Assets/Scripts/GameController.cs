using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	
	public static int StartSide;
	
	public Player RedA;
	public Player RedB;
	public Player BlueA;
	public Player BlueB;
	
	public Ball GameBall;
	
	public string ButtonAxis;
	
	private int mScoreTeamRed;
	private int mScoreTeamBlue;
	private bool mIsButtonPressed;

	// Use this for initialization
	void Start() {
		mScoreTeamRed = 0;
		mScoreTeamBlue = 0;
	}
	
	// Update is called once per frame
	void Update() {
		if (mIsButtonPressed) {
			Reset(VolleyUtil.Team.RedTeam);
		}
	}
	
	void FixedUpdate() {
		mIsButtonPressed = Input.GetAxis(ButtonAxis) > 0;
	}
	
	public void AddScore(int newScoreValueA, int newScoreValueB) {
        mScoreTeamRed += newScoreValueA;
		mScoreTeamBlue += newScoreValueB;
        UpdateScore();
    }

    void UpdateScore() {
       // scoreText.text = "Score: " + score;
    }
	
	public void Reset(VolleyUtil.Team team) {
		if (team == VolleyUtil.Team.RedTeam) {
			RedB.Reset(true);
			BlueB.Reset(false);
		} else {
			BlueB.Reset(true);
			RedB.Reset(false);
		}
		
		RedA.Reset(false);
		BlueA.Reset(false);
		GameBall.Reset(team);
	}
}
