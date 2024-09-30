using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static AudioClip ClickEffect;
    static AudioSource audioSrc;
    public GameObject mainPannel;
    public GameObject instructionsPannel;
    //public GameObject lvlsPannel;
    //public GameObject creditPannel;

    private void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        instructionsPannel.SetActive(false);
        //lvlsPannel.SetActive(false);
        //creditPannel.SetActive(false);
    }

    public void PlaySound(string name)
    {
        ClickEffect = Resources.Load<AudioClip>(name);
        audioSrc.PlayOneShot(ClickEffect);
    }

    public void LoadlLevel(int lvl)
    {
        SceneManager.LoadScene(lvl);
    }

    public void PanelHowToPlay()
    {
        mainPannel.SetActive(false);
        //creditPannel.SetActive(false);
        //lvlsPannel.SetActive(false);
        instructionsPannel.SetActive(true);
    }

    //public void PanelLvlSelect()
    //{
    //    instructionsPannel.SetActive(false);
    //    //creditPannel.SetActive(false);
    //    mainPannel.SetActive(false);
    //    //lvlsPannel.SetActive(true);
    //}

    //public void PanelCredits()
    //{
    //    instructionsPannel.SetActive(false);
    //    mainPannel.SetActive(false);
    //    //lvlsPannel.SetActive(false);
    //    //creditPannel.SetActive(true);
    //}

    public void BackToMenu()
    {
        instructionsPannel.SetActive(false);
        //lvlsPannel.SetActive(false);
        //creditPannel.SetActive(false);
        mainPannel.SetActive(true);
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
