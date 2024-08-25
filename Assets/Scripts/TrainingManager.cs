using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TrainingManager : MonoBehaviour
{
    //Game objects 
    public TMP_Text actionText;
    public TMP_Text instructionsText;
    public Button startButton;
    public Image circle;
    public Button next;
    public Button back; 

    private List<Color> colors; 
    private int maxTrials = 15;
    private float yellowProbability = 0.5f;
    private float interBlockSec = 30f; 

    
    void Start()
    {
        //Button and gameObjects
        startButton.gameObject.SetActive(true);
        instructionsText.gameObject.SetActive(true);
        actionText.gameObject.SetActive(false);
        circle.gameObject.SetActive(false);
        back.gameObject.SetActive(false);
        next.gameObject.SetActive(false);

        startButton.onClick.AddListener(startTraining);
        back.onClick.AddListener(BackToMenu);
        next.onClick.AddListener(NextExperiment); 

        instructionsText.text = "Color training";

        colors = new List<Color> { Color.red, Color.green, Color.blue, Color.yellow };

    }
    
    private void startTraining()
    {
        startButton.gameObject.SetActive(false);
        StartCoroutine(ShowColorRutine());
    }

    IEnumerator ShowColorRutine()
    {
        int numBlocks = 0;
        int numTrials = 0; 

        //Display task instructions
        instructionsText.text = "In each trial, a random color will be shown.\nYou should grip ONLY when the color yellow appears.\n Keep applying force the whole time yellow is on the screen.";
        yield return new WaitForSecondsRealtime(10f);
        instructionsText.text = "In the first block, you'll get a written hint on what to do.\nIn the second block, only the color will be shown.";
        yield return new WaitForSecondsRealtime(10f);
        instructionsText.text = "Let's get started!";
        yield return new WaitForSecondsRealtime(5f);

        instructionsText.gameObject.SetActive(false);
        circle.gameObject.SetActive(true);
        actionText.gameObject.SetActive(true);

        while (true)
        {
            while (numTrials < maxTrials)
            {

                // Decide whether to show yellow or not
                int colorIndex;
                bool showYellow = UnityEngine.Random.value <= yellowProbability;
                if (showYellow)
                {
                    colorIndex = colors.Count - 1; // Yellow color index
                    actionText.text = "Grip!";
                }
                else
                {
                    // Choose a color different from yellow
                    colorIndex = UnityEngine.Random.Range(0, colors.Count - 1);
                    actionText.text = "Don't grip!";
                }

                //set the color of the circle
                circle.gameObject.SetActive(true);
                circle.color = colors[colorIndex];

                //5 seconds of contraction
                yield return new WaitForSecondsRealtime(5f);

                //5 seconds of resting time
                circle.gameObject.SetActive(false);
                actionText.text = "Relax"; 
                yield return new WaitForSecondsRealtime(5f);

                numTrials++;
                Debug.Log($"Num Trials: {numTrials}");
            }

            if (numBlocks < 1)
            {
                actionText.gameObject.SetActive(false);
                instructionsText.gameObject.SetActive(true );
                instructionsText.text = "Very good!\n After a short break we will repeat the same but without the written hint";
                yield return new WaitForSecondsRealtime(interBlockSec);
                instructionsText.gameObject.SetActive(false); 
            }
            else
            {
                actionText.gameObject.SetActive(false);
                instructionsText.gameObject.SetActive(true);
                instructionsText.text = "Training completed!";
                yield return new WaitForSecondsRealtime(10f);
                endTask();
            }

            numTrials = 0;
            numBlocks++;
            
        }
        
    }

    private void endTask()
    {
        StopAllCoroutines();
        back.gameObject.SetActive(true) ;
        next.gameObject.SetActive(true) ;

    }

    public void BackToMenu()
    {
        GameManager._instance.ChangeScene(GameManager.Scenes.Menu);
    }

    public void NextExperiment()
    {
        GameManager._instance.ChangeScene(GameManager.Scenes.MotorTask);
    }


}
