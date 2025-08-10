using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIMenger : MonoBehaviour
{
    [SerializeField] TMP_Text Score;
    private void Start()
    {
        Score.text = PlayerPrefs.GetInt("Score", 0).ToString();
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
}
