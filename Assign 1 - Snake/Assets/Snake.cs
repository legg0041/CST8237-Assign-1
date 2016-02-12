/*
this tutorial was used as a base
http://noobtuts.com/unity/2d-snake-game
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the moment to moment state of the snake, lives, and points
/// </summary>
public class Snake : MonoBehaviour {

    //holds last direction to be used in Move()
    Vector2 _setDirection = Vector2.up;
    //keep track of tail
    List<Transform> tailList = new List<Transform>();

    //did the snke eat something
    bool _ateFood = false;

    //lives
    public int livesLeft = 3;

    //points
    int _totalScore = 0;

    //tail
    public GameObject tailPrefab;

    //moveSpeed
    public float moveSpeed = 0.1f;

    //points per food
    int _addPoints = 100;
    int _tailPoints = 10;

    Vector3 _startPoint = new Vector3(0, -7, 10);

    //UI
    Text _livesText;
    Text _scoreText;

    //key to get from player pref
    string _key = "HIGH_SCORE";

    /// <summary>
    /// Used at startup
    /// </summary>
    void Start () {

        //repeat call to Move every 0.1s
        InvokeRepeating("Move", moveSpeed, moveSpeed);
	}
	
	/// <summary>
    /// Called every frame to check for input from user
    /// </summary>
	void Update () {
        //check for right arrow
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (_setDirection != Vector2.left)
            {
                //set direction
                _setDirection = Vector2.right;
            }
        }//check for left arrow
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (_setDirection != Vector2.right)
                {
                    //set direction
                    _setDirection = Vector2.left;
                }
            }//check for up arrow
            else
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    if (_setDirection != Vector2.down)
                    {
                        //set direction
                        _setDirection = Vector2.up;
                    }
                }//check for down arrow
                else
                {
                    if (Input.GetKey(KeyCode.DownArrow))
                    {
                        if (_setDirection != Vector2.up)
                        {
                            //set direction
                            _setDirection = Vector2.down;
                        }
                    }
                }
            }
        }
	}

    /// <summary>
    /// Called from InvokeRepeating to move snake head and tail
    /// </summary>
    void Move()
    {
        //hold current head position
        Vector3 _curPosition = transform.position;
        //move head
        transform.Translate(_setDirection);
        //if food has been eaten
        if (_ateFood)
        {
            //load prefab
            GameObject tp = (GameObject)Instantiate(tailPrefab, _curPosition, Quaternion.identity);
            //add to tail list
            tailList.Insert(0, tp.transform);
            //reset flag
            _ateFood = false;
            //increase speed
            moveSpeed = (moveSpeed * 0.75f);
        }
        //tail exists?
        if(tailList.Count > 0)
        {
            //move last tail piece to where head used to be
            tailList.Last().position = _curPosition;
            //move segment inside of list and remove
            tailList.Insert(0, tailList.Last());
            tailList.RemoveAt(tailList.Count - 1);
        }
    }

    /// <summary>
    /// Handles the various collisions that can occur
    /// </summary>
    /// <param name="coll">
    /// The collision that has occurred
    /// </param>
    void OnTriggerEnter2D(Collider2D coll)
    {
        //if collision is with food
        if (coll.name.StartsWith("FoodPrefab"))
        {
            //change flag to true
            _ateFood = true;
            //add points
            _totalScore += _addPoints;
            _addPoints = (int)(_addPoints + (tailList.Count() * _tailPoints));
            //change value in UI
            _scoreText = GameObject.Find("scoreText").GetComponent<Text>();
            _scoreText.text = _totalScore.ToString("000000000000000");
        }
        else //collided with tail or border
        {
            //subtract a life
            livesLeft -= 1;
            //change value in UI
            _livesText = GameObject.Find("livesText").GetComponent<Text>();
            _livesText.text = livesLeft.ToString();

            //get all tailPrefab objects
            var tempList = GameObject.FindGameObjectsWithTag("tail");
            //loop through and destroy them
            foreach (GameObject t in tempList)
            {
                Destroy(t);
            }
            //clear the list
            tailList.Clear();
            //reset the snakes position
            this.transform.position = _startPoint;

            //check if game over
            if (livesLeft == 0)
            {
                //check for high score
                if (PlayerPrefs.HasKey(_key))
                {
                    //get high sore
                    var _high_score = PlayerPrefs.GetInt(_key);
                    //check current vs highscore
                    if(_totalScore > _high_score)
                    {
                        //set new high score
                        PlayerPrefs.SetInt(_key, _totalScore);
                    }
                }
                else
                {
                    //set new high score
                    PlayerPrefs.SetInt(_key, _totalScore);
                }
                SceneManager.LoadScene("Main");
            }
        }
    }

}
