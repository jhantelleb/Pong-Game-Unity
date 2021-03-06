﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

	Rigidbody ballComponent; //rigid body attached to Game Object component
	public GameObject ball;
	private float ballSpeed = 15;
	private float waitTime = 2;

	// Use this for initialization
	void Start () {

		//Instantiate rigidbody Game Component
		ballComponent = GetComponent<Rigidbody> ();

		//Pause ball logic to delay launch
		StartCoroutine (Pause ());
	}
	
	// Update is called once per frame
	void Update () {

		//Checks if the ball is OUT OF BOUNDS from the right, player 1 scores
		if (transform.position.x > ballSpeed) { 
			StartCoroutine (Pause ());
			ScoreboardController.instance.Player1Point ();
		}

		//Checks if the ball is OUT OF BOUNDS from the left, player 2 scores
		if (transform.position.x < -(ballSpeed)) {
			StartCoroutine (Pause ());
			ScoreboardController.instance.Player2Point ();
		}
	}

	// Works with Courotine to delay the movement of the ball.
	IEnumerator Pause () {

		ballComponent.velocity = Vector3.zero; //stop the ball to avoid the glitch
		transform.position = Vector3.zero; //Ball always starts from the center of the scene. 
		ball.SetActive(true);
		//wait for 2 seconds
		yield return new WaitForSeconds (waitTime);
		//Call Function to launch ball
		LaunchBall ();

	}

	void LaunchBall() {

		//Use Random function to set X direction to right or left. 0 for left, 1 for right.
		//Passing Int to the function rounds down the Int, so the max value will always be 1
		int xDirection = Random.Range(0, 2); 

		//Use Random function to set direction of Y axis (up or down) 
		int yDirection = Random.Range(0, 3);

		Vector3 launchDirection = new Vector3();

		//Check if X direction is left or right
		switch (xDirection) { 
		case 0:
			launchDirection.x = -10f; 
			break;
		case 1:
			launchDirection.x = 10f;
			break;
		default:
			break;
		}

		//Check if Y direction is up or down
		switch (yDirection)
		{ 
		case 0:
			launchDirection.y = -10f;
			break;
		case 1:
			launchDirection.y = 10f;
			break;
		default:
			break;

		}

		ballComponent.velocity = launchDirection;

	}

	//When ball hits the boundaries or paddles
	void OnCollisionEnter (Collision hit) {
		float hitPaddlePosition = transform.position.y - hit.gameObject.transform.position.y;

		switch (hit.gameObject.name) {
		case "TopBoundary":
			ballComponent.velocity = new Vector3 (changeXSpeedDirection(), -10f, 0f);
			break;
		case "BottomBoundary":
			ballComponent.velocity = new Vector3 (changeXSpeedDirection(), 10f, 0f);
			break;
		case "RightPaddle":
			ballComponent.velocity = new Vector3 (-10f, 0f, 0f); //bounces straight to the left
			if (hitPaddlePosition < -0.5) { //hit lower half of right paddle
				ballComponent.velocity = new Vector3 (-10f, -10f, 0f); //bounce ball back to the left with an upward angle
			} else if (hitPaddlePosition > 0.5) { //hit upper half of the right paddle
				ballComponent.velocity = new Vector3 (-10f, 10f, 0f); //bounce ball back to the left with a downward angle
			} 
			break;
		case "LeftPaddle":
			ballComponent.velocity = new Vector3 (10f, 0f, 0f); //bounces straight to the right
			if (hitPaddlePosition < -0.5) { //hit lower half of right paddle
				ballComponent.velocity = new Vector3 (10f, -10f, 0f); //bounce ball back to the right with an upward angle
			} else if (hitPaddlePosition > 0.5) { //hit upper half of the right paddle
				ballComponent.velocity = new Vector3 (10f, 10f, 0f); //bounce ball back to the right with a downward angle
			}
			break;
		}
	}


	//To bounce to the opposite direction	
	private float changeXSpeedDirection() {
		float speedInXDirection = 0f;

		if (ballComponent.velocity.x > 0f) { //going to the left
			speedInXDirection = 10f; //bounce to right side
		} else if (ballComponent.velocity.x < 0f) { //going to the right
			speedInXDirection = -10f; //bounce to left side
		}
		return speedInXDirection;
	}

	//Check which direction the ball should launch. 
	//1. Check which player scored a point, then serve the 


}
