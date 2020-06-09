﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{                 //start move  //start count
    public static bool begin, ready, ai; 
    private float time0, go;
    private bool end, reset;
    public static string aitext = "PvP";
    private int AIChoice, AISpeed;

    public Button restart, readyB, aibutton;
    public Text finalresult;

    public AudioSource IHBeat, FHBeat;
    private bool ib1 = true, ib2 = true, fb = true; 

    // Start is called before the first frame update
    void Start() 
    {
        restart.onClick.AddListener(ResetOnClick);
        readyB.onClick.AddListener(ReadyOnClick);
        aibutton.onClick.AddListener(AIOnClick);
        //finalresult.text = "Begin";
        end = false;
        reset = false;
        ready = false;
        begin = false;
        go = Random.Range(2.5f, 10f);
        AIChoice = Random.Range(1, 4);
        AISpeed = Random.Range(0, 2);
    }

    void AIOnClick()
    {
        ai = !ai;
        if (ai)
            aitext = "PvE"; //aibutton.GetComponentInChildren<Text>().text = "PvE";
        else
            aitext = "PvP"; //aibutton.GetComponentInChildren<Text>().text = "PvP";
    }

    void ReadyOnClick()
    {
        time0 = Time.deltaTime;
        ready = true;
    }

    void ResetOnClick()
    {
        reset = true;
    }

    // Update is called once per frame
    void Update()
    {
        string result = "Begin", Left = "Shogun", Right = "Rei";
        int lchoice = LeftPlayer.strikeL;
        int rchoice = RightPlayer.strikeR;
        int winner = 0;
        aibutton.GetComponentInChildren<Text>().text = aitext;

        if (ready == true && end == false) {
            time0 += Time.deltaTime;
            LeftPlayer.timeL += Time.deltaTime;
            RightPlayer.timeR += Time.deltaTime;
            if (time0 <= 2)
            {
               // finalresult.text = "1";
                if (ib1)
                {
                    IHBeat.Play();
                    ib1 = false;
                }
                LeftPlayer.anim.SetTrigger("TriggerReady");
                RightPlayer.anim.SetTrigger("TriggerReady");
            }
            else if (time0 <= go)
            {
                //finalresult.text = "2";
                if (ib2)
                {
                    IHBeat.Play();
                    ib2 = false;
                }
            }
            else
            {
                //finalresult.text = "3";
                if (fb)
                {
                    FHBeat.Play();
                    fb = false;
                }
                begin = true;
                if (ai == true)
                {
                    if (time0 >= go + AISpeed)
                    {
                        LeftPlayer.strikeL = AIChoice;
                        lchoice = LeftPlayer.strikeL;
                        //Debug.Log(AISpeed);
                        LeftPlayer.timeL = Time.deltaTime;
                        LeftPlayer.anim.SetTrigger("TriggerRun");
                        LeftPlayer.katanaOff.SetActive(false);
                        LeftPlayer.katanaOn.SetActive(true);
                    }
                }
                if (rchoice == 0 && lchoice == 0) { /*result = "Begin"; */ } //Initial
                else if (rchoice == 0 && lchoice != 0) { result = Right + " loses"; winner = 1; } //Rei didn't move
                else if (lchoice == 0 && rchoice != 0) { result = Left + " loses"; winner = 2; } //Shogun didn't move
                else
                {
                    if (LeftPlayer.timeL > RightPlayer.timeR + 0.8f) //Rei is patient
                    {
                        result = Right + " Wins, patience"; winner = 2;
                    }
                    else if (RightPlayer.timeR > LeftPlayer.timeL + 0.8f) //Shogun is patient; increase to make difficult mean value = 0.5f
                    {
                        result = Left + " Wins, patience"; winner = 1;
                    }
                    else
                    {
                        if (rchoice != lchoice)
                        {
                            if (rchoice == 1)
                            {
                                if (lchoice == 2)
                                {
                                    result = Left + " wins"; //Shogun: Paper x Rei: Rock
                                    winner = 1;
                                }
                                else if (lchoice == 3)
                                {
                                    result = Right + " wins"; //Shogun: Scissor x Rei: Rock
                                    winner = 2;
                                }
                            }
                            else if (rchoice == 2)
                            {
                                if (lchoice == 1)
                                {
                                    result = Right + " wins"; //Shogun: Rock x Rei: Paper
                                    winner = 2;
                                }
                                else if (lchoice == 3)
                                {
                                    result = Left + " wins"; //Shogun: Scissor x Rei: Paper
                                    winner = 1;
                                }
                            }
                            else if (rchoice == 3)
                            {
                                if (lchoice == 1)
                                {
                                    result = Left + " wins"; //Shogun: Rock x Rei: Scissor
                                    winner = 1;
                                }
                                else if (lchoice == 2)
                                {
                                    result = Right + " wins"; //Shogun: Paper x Rei: Scissor
                                    winner = 2;
                                }
                            }
                        }
                        else { result = "Again!"; winner = 0; } //Draw
                    }
                }

                if(LeftPlayer.pos - RightPlayer.pos >= -10) // && LeftPlayer.pos - RightPlayer.pos < 1)
                {
                    LeftPlayer.anim.SetTrigger("TriggerUp");
                    RightPlayer.anim.SetTrigger("TriggerUp");
                }

                if (LeftPlayer.pos - RightPlayer.pos >= 5)
                {
                    LeftPlayer.strikeL = 0;
                    RightPlayer.strikeR = 0;
                    ready = false;
                    begin = false;
                    //finalresult.text = result;
                    end = true;
                    if (winner == 1)
                    {
                        RightPlayer.anim.SetTrigger("TriggerDeath");
                        LeftPlayer.anim.SetTrigger("TriggerEnd");

                    }
                    else if (winner == 2)
                    {
                        LeftPlayer.anim.SetTrigger("TriggerDeath");
                        RightPlayer.anim.SetTrigger("TriggerEnd");
                    }
                }
            }
            if (LeftPlayer.llose == true && RightPlayer.rlose == true)
            {
                finalresult.text = "Double Harakiri";
                end = true;
            }
            else if (LeftPlayer.llose == true)
            {
                finalresult.text = Left + " does Harakiri";
                end = true;
            }
            else if (RightPlayer.rlose == true)
            {
                finalresult.text = Right + " does Harakiri";
                end = true;
            }
        }


        if (reset == true)
        {
            SceneManager.LoadScene(Random.Range(0, SceneManager.sceneCount + 2));
           // finalresult.text = "Begin";
            LeftPlayer.llose = false;
            RightPlayer.rlose = false;
           // reset = false;
           // ready = false;
           // end = false;
           // ai = false;s
        }
    } 
}
