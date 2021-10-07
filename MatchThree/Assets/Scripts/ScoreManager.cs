using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    int time;
    int score = 0;
    GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        ScoreText.text = "Score : " + score;
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Score(int dscore)
    {
        score += dscore;
        ScoreText.text = "Score " + score;
    }
    //checking if the score is low than a certain number
    public void makingScore()
    {
        if (score < 0)

        {
            manager.LoadingScene();
        }
    }

}
