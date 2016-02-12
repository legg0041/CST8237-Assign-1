/*
this tutorial was used as a base
http://noobtuts.com/unity/2d-snake-game
*/
using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the food, currently randomly moves food on collision.
/// Food always starts at same position but can be changed to be randomly 
/// spawned.
/// </summary>
public class SpawnFood : MonoBehaviour {

    //Food Prefab
    public GameObject foodPrefab;

    //z depth
    int _zPosition = 10;

    //borders
    public Transform borderTop;
    public Transform borderBottom;
    public Transform borderLeft;
    public Transform borderRight;


    /// <summary>
    /// Initialize things at startup
    /// </summary>
    void Start () {

	}
	
    /// <summary>
    /// Currently not used, could be used to set initial position of food
    /// to random position
    /// </summary>
	void Spawn () {

        //get random point
        Vector2 _randPosition = RandomPoint();

        //spawn the food
        Instantiate(foodPrefab, _randPosition, Quaternion.identity);
    }

    /// <summary>
    /// Creates a random Vector3 within the set borders
    /// </summary>
    /// <returns>
    /// Returns a Vector3 within the borders
    /// </returns>
    Vector3 RandomPoint()
    {
        //position x between left and right borders
        int x = (int)Random.Range(borderLeft.position.x, borderRight.position.x);

        //position y between top and bottom borders
        int y = (int)Random.Range(borderTop.position.y, borderBottom.position.y);
        //create vector
        Vector3 _newPosition = new Vector3(x, y, _zPosition);
        //return it
        return _newPosition;
    }

    /// <summary>
    /// Handles the collision with the head of the snake
    /// </summary>
    /// <param name="coll">
    /// The collision that has occurred
    /// </param>
    void OnTriggerEnter2D(Collider2D coll)
    {
        //on collision with head move food
        if (coll.name.StartsWith("Head"))
        {
            //move to randomized point
            this.gameObject.transform.position = RandomPoint();
        }
    }

}
