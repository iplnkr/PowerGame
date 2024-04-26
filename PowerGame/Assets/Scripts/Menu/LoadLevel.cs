using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] private Animator building;
    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject pane;
    public void LoadScene(string levelName)
    {
        if(pane != null)
        {
            pane.SetActive(true);
        }
        SceneManager.LoadScene(levelName);
    }

    public void BuildingBlackout()
    {
        //play blackout audio
        DeeJay dj = FindObjectOfType<DeeJay>();
        if(dj != null)
        {
            dj.PlayShutdown();
        }
        //
        building.Play("Base Layer.HouseBlackout");
        logo.SetActive(false);
        button.SetActive(false);
    }
}
