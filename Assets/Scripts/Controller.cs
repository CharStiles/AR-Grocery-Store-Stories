using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using UnityEngine;

public class Controller : MonoBehaviour
{
 [Header("Controller")]
 [TextArea]
 [Tooltip("Doesn't do anything. Just comments shown in inspector")]
 //public string Notes = "This component shouldn't be removed, it does important stuff.";


    [HideInInspector]
    public Text snobBogSays;
    [HideInInspector]
    public Text observation;
    
    int daysWaitBetween;
    string certainty;
    string startInfo;
    string lastPrediction;
    int waitSeconds;

    int numSeasons;
    Touch touch;
    float doubleTapTimer;
    
    GameObject observationObj;
    
    [HideInInspector]
    public enum modes {looking, playing, eyeContact, stopEyeContact};

    [HideInInspector]
    public modes mode;

    Vector3 snobIsHere;

    Dictionary<string, string> observationsToScene =  
    new Dictionary<string, string>();

    GameObject canvas;
    int tapCount;

    private int seasonNum;
    private string scene;
    void initValues(){ // for Client
        daysWaitBetween = 1;
        observationsToScene.Add("Soft Drink Aisle","murmur_");
        observationsToScene.Add("Other Aisle","murmur_");
        observationsToScene.Add("Paper Towel Aisle","murmur_");
        observationsToScene.Add("Frozen Food Aisle","indulgences_");
        observationsToScene.Add("Candy aisle","indulgences_");
        observationsToScene.Add("Chip aisle","indulgences_");
        observationsToScene.Add("Observation","");
        waitSeconds = 3; // three seconds of being in the same "aisle" and triggers the scene loading
        numSeasons=3;
    
    }

    // Start is called before the first frame update
    void Start()
    {
        // this is for the machine learning
        // make sure vision is tagged vision
        GameObject visionObj = GameObject.FindWithTag("vision");
        //observation = observationObj.GetComponent<Text>();
        observation = visionObj.GetComponent<Examples.ARKitExample2>()._text;//ARKitExample2._text;
        certainty = "";

        observationObj = GameObject.FindWithTag("debugText1");
        snobBogSays = GameObject.FindWithTag("debugText0").GetComponent<Text>();
        canvas = GameObject.FindWithTag("canvas");

        initValues();

        DateTime now = DateTime.Now;
        // the following line gets datetime as a string and turns it back into into datetime, but frist converting that 
        // string to binary (in64) then into datetime
        DateTime startDate = DateTime.FromBinary(long.Parse( (PlayerPrefs.GetString("startDate", now.ToBinary().ToString()))));

        seasonNum = PlayerPrefs.GetInt("seasonNum",0); // whicih season to load for each characters
        double daysSince = (now - startDate).TotalDays; // TODO: figure out why this is changing from launch to launch

        if (startDate == now){ 
            PlayerPrefs.SetString("startDate", now.ToBinary().ToString());
        }
        else if (daysSince >= daysWaitBetween){
            
            seasonNum ++;
            PlayerPrefs.SetInt("seasonNum",seasonNum);
        }
        if (seasonNum >= numSeasons){
            seasonNum = 0;
            PlayerPrefs.SetInt("seasonNum",seasonNum);
        }

        startInfo = String.Format("loading seasons {0}\n {1} days left til next season loads.",seasonNum,Mathf.Round((float)(daysWaitBetween-daysSince)));
        
        doubleTapTimer = 0;
        tapCount = 0;
        mode = modes.looking;
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.frameCount < 60 * 4){
            return;
        }
        if (mode == modes.looking){
            snobBogSays.text = "";// startInfo + "\n" + certainty;
            
            if (observationObj.active  && observation.text == lastPrediction){
                if (Time.frameCount % 60 == 0){
                    certainty += ".";
                }
                
                if (certainty.Length >= (waitSeconds)){
                    if (observation.text == "Paper Towel Aisle" || observation.text == "Frozen Food Aisle" || observation.text == "Candy aisle" || observation.text == "Soft Drink Aisle" || observation.text == "Other Aisle"|| observation.text == "Chip aisle" ){
                        mode = modes.playing;
                        
                        snobIsHere = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z); // this is where the plater was when they found the last snob

                        certainty = "";
                        snobBogSays.text = "playing scene";
                        scene = observationsToScene[observation.text] + seasonNum.ToString();
                        
                        SceneManager.LoadSceneAsync("Scenes/"+scene, LoadSceneMode.Additive);
                       
                        //SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);
                    
                    }   
                }
            }
            else{
                certainty = "";
            }
            lastPrediction = observation.text;
        }
        else if (mode == modes.playing){ // if you walk far away from snob
            snobBogSays.text = "playing scene "+(Vector3.Distance(Camera.main.transform.position, snobIsHere)).ToString();
            if (Vector3.Distance(Camera.main.transform.position, snobIsHere)> 3.0){
                SceneManager.UnloadSceneAsync("Scenes/"+scene);
                mode = modes.looking;
                snobBogSays.text = "looking";
            }
        }
         if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
         {
             tapCount++;
         }
         if (tapCount > 0)
         {
             doubleTapTimer += Time.deltaTime;
         }
         if (tapCount >= 2)
         {
             canvas.SetActive (!canvas.activeInHierarchy);
             doubleTapTimer = 0.0f;
             tapCount = 0;
         }
         if (doubleTapTimer > 0.5f)
         {
             doubleTapTimer = 0f;
             tapCount = 0;
         }
    }
}
