using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager singelton;
    private GrounndPiece[] allGrounPiece;
    // Start is called before the first frame update
    void Start()
    {
        SetupNewLevel();
    }

    private void SetupNewLevel()
    {
        allGrounPiece = FindObjectsOfType<GrounndPiece>();
    }
    private void Awake()
    {
        if (singelton == null)
        {
            singelton = this;
        }
        else if (singelton != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishloaing;
    }

    private void OnLevelFinishloaing(Scene scene, LoadSceneMode mode)
    {
        SetupNewLevel();
    }

    public void CheckComplete()
    {
        bool isFinished = true;
        for (int i = 0; i < allGrounPiece.Length; i++)
        {

            if (allGrounPiece[i].iscolor == false)
            {

                isFinished = false;
                break;
            }

        }

        if (isFinished)
        {
            //Next Level
            NextLevel();
        }
    }
   
    private void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 6)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void MainScene()
    {
        SceneManager.LoadScene(1);
    }

    public void RandomScene()
    {
        int scene = Random.Range(1, 6);
        SceneManager.LoadScene(scene);
    }
}
