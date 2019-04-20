﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace controll
{
    public class Player : MonoBehaviour {
        public int horizontal = 0;     //Used to store the horizontal move direction.
        public int vertical = 0;

        public enum State {
            Idle,
            Walk,
            GameOver
        }
        public State state = State.Idle;

        void Start() {
            StartCoroutine(shootBullet());
        }

        // Update is called once per frame
        private void Update() {
            //If it's not the player's turn, exit the function.

            horizontal = 0;     //Used to store the horizontal move direction.
            vertical = 0;      //Used to store the vertical move direction.

            //Check if we are running either in the Unity editor or in a standalone build.
#if UNITY_STANDALONE || UNITY_WEBPLAYER

            //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
            horizontal = (int)(Input.GetAxisRaw("Horizontal"));

            //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
            vertical = (int)(Input.GetAxisRaw("Vertical"));

            if (horizontal == 0 && vertical == 0) state = State.Idle;
            else state = State.Walk;
            StateMachine();

            //Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
			
		//Check if Input has registered more than zero touches
		if (Input.touchCount > 0)
		{
			//Store the first touch detected.
			Touch myTouch = Input.touches[0];
				
			//Check if the phase of that touch equals Began
			if (myTouch.phase == TouchPhase.Began)
			{
				//If so, set touchOrigin to the position of that touch
				touchOrigin = myTouch.position;
			}
				
			//If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
			else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
			{
				//Set touchEnd to equal the position of this touch
				Vector2 touchEnd = myTouch.position;
					
				//Calculate the difference between the beginning and end of the touch on the x axis.
				float x = touchEnd.x - touchOrigin.x;
					
				//Calculate the difference between the beginning and end of the touch on the y axis.
				float y = touchEnd.y - touchOrigin.y;
					
				//Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
				touchOrigin.x = -1;
					
				//Check if the difference along the x axis is greater than the difference along the y axis.
				if (Mathf.Abs(x) > Mathf.Abs(y))
					//If x is greater than zero, set horizontal to 1, otherwise set it to -1
					horizontal = x > 0 ? 1 : -1;
				else
					//If y is greater than zero, set horizontal to 1, otherwise set it to -1
					vertical = y > 0 ? 1 : -1;
			}
		}
			
#endif //End of mobile platform dependendent compilation section started above with #elif

            Vector3 moveDir = //방향
                (Vector3.forward * vertical) + (Vector3.right * horizontal);
            transform.Translate(moveDir.normalized * 2.0f * Time.deltaTime);

        }


        void StateMachine() {
            switch (state) {
                case State.Idle:

                    break;
                case State.Walk:
            }
        }


        IEnumerator shootBullet() {
            while (state != State.GameOver) {
                if (state == State.Walk)
                    ShootBullet();
                yield return new WaitForSeconds(1.0f);
            }
        }

        public GameObject bullet;
        public void ShootBullet() {
            //for(int i = 0; i<16; i++) {
            ObjectManager.instance.GetBullet(transform.position);

            //Instantiate(bullet, transform.position + transform.forward * 1.0f, Quaternion.identity);
            //}

        }

        public GameObject bullet;
        public void ShootBullet() {
            ObjectManager.instance.GetBullet(transform.position);

        }
    }
}
