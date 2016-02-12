using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadScore : MonoBehaviour {

    //key to get from player pref
    string key = "HIGH_SCORE";
    //UI
    public Text scoreText;

    // Use this for initialization
    void Start () {
        if (PlayerPrefs.HasKey(key))
        {
            var _high_score = PlayerPrefs.GetInt(key);
            scoreText.text = _high_score.ToString("000000000000000");
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
