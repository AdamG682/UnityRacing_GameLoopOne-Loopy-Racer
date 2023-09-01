using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingManager : MonoBehaviour
{
    //creating game objects for the checkpoints of the race
    public GameObject startRace;
    public GameObject endRace;
    public GameObject[] checkpoints;

    //creating a float of laps (these can be edited within Unity)
    public float laps = 1;

    //creating information such as checkpoint/lap status and whether or not the race has started/finished
    private float currentCheckpoint;
    private float currentLap;
    private bool started;
    private bool finished;

    //creating floats that track lap times and lap
    private float currentLapTime;
    private float bestLapTime;
    private float bestLap;

    //sound effects
    public AudioSource checkpointAudio;
    public AudioSource lapAudio;
    public AudioSource finishedAudio;

    //at the start of the race these values will stay 0 because the race has technically started. Once started, these values will be altered
    private void Start()
    {
        currentCheckpoint = 0;
        currentLap = 1; //starts will lap 1

        started = false;
        finished = false;

        currentLapTime = 0;
        bestLapTime = 0;
        bestLap = 0;
    }

    private void Update()
    {
        if(started && !finished)
        {
            currentLapTime += Time.deltaTime;

            if(bestLap == 0)
            {
                bestLap = 1;
            }
        }

        if(started)
        {
            if(bestLap == currentLap)
            {
                bestLapTime = currentLapTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Checkpoints"))
        {
            GameObject thisCheckpoint = other.gameObject;

            //this checks if the race has started
            if(thisCheckpoint == startRace && !started)
            {
                started = true;
                print("Race Started");
            }

            //checking if the lap ends or if the entire race is finished
            else if(thisCheckpoint == endRace && started)
            {
                //if all of the laps are finished, then the race is complete (best lap is recorded)
                if(currentLap == laps)
                {
                    if(currentCheckpoint == checkpoints.Length)
                    {
                        if(currentLapTime < bestLapTime)
                        {
                            bestLap = currentLap;
                        }
                        
                        finished = true;
                        finishedAudio.Play();
                        print("Race Finished");
                    }
                    else
                    {
                        print("Missing Checkpoints");
                    }
                }
                
                //if all of the checkpoints are met, then start a new lap in the race (best time is recorded)
                else if(currentLap < laps)
                {
                    if(currentCheckpoint == checkpoints.Length)
                    {
                        if(currentLapTime < bestLapTime)
                        {
                            bestLap = currentLap;
                            bestLapTime = currentLapTime;
                        }
                        
                        currentLap++;
                        currentCheckpoint = 0;
                        currentLapTime = 0;
                        lapAudio.Play();
                        print($"Start of Lap {currentLap}");
                    }
                }

                else
                {
                    print("Missing Checkpoint");
                }
            }

            //for loop that is used to make sure that the player is passing through the correct checkpoints and not going in the wrong order
            for (int i = 0; i < checkpoints.Length; i++)
            {
                if(finished)
                {
                    return;
                }

                //when the player passes through the correct checkpoint
                if(thisCheckpoint == checkpoints[i] && i == currentCheckpoint)
                {
                    currentCheckpoint++;
                    print("Checkpoint Passed");
                    checkpointAudio.Play();
                }

                //if the player drives through the incorrect order of checkpoints
                else if(thisCheckpoint == checkpoints[i] && i != currentCheckpoint)
                {
                    print("Wrong Checkpoint");
                }
            }
        }
    }

    //making a basic UI that details lap and lap times
    private void OnGUI()
    {
        //displaying the current time for lap
        string formattedCurrentTime = $"Current: {Mathf.FloorToInt(currentLapTime / 60)}:{currentLapTime % 60:00.000} - (Lap {currentLap})";
        GUI.Label(new Rect(50, 10, 250, 100), formattedCurrentTime);

        //displaying the best time for lap
        string formattedBestTime = $"Best: {Mathf.FloorToInt(bestLapTime / 60)}:{bestLapTime % 60:00.000} - (Lap {bestLap})";
        GUI.Label(new Rect(250, 10, 250, 100), formattedBestTime);
    }
}
