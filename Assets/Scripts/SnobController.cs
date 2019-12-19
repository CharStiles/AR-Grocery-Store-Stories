using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.iOS
{
public class SnobController : MonoBehaviour
{
    //System.Type snobAppear = SnobAppear;
    [Header("audio clips pls")]
    public AudioClip mainSound;

    [Tooltip("you can leave this empty if there is no idle sound")]
    public AudioClip idleSound;

    [Header("animator with idle and/or main animation in it")] 
    [Header("make sure idle is called 'idle' and main is 'main'")] 
    public Animator animate;

    [Header("0 for no eye contact 1 for consistent, inbetween for itermiten")]
    [Range(0.0f,1.0f)]
    public float eyeContactTime = 0;    

    [Tooltip("you can leave this empty if there is no eyecontact happening")]
    public GameObject baseOfTheNeckForEyeContact;

    void Awake(){
        SnobAppear sa = gameObject.AddComponent(typeof(SnobAppear)) as SnobAppear;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (baseOfTheNeckForEyeContact != null){
            baseOfTheNeckForEyeContact.tag = "makesEyeContact";
        }
        if (eyeContactTime > 0.0){
            MakesEyeContact mec = gameObject.AddComponent(typeof(MakesEyeContact)) as MakesEyeContact;
        }
        AudioSource audioSource  = gameObject.GetComponent<AudioSource>();
        if(audioSource == null){
            audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        }
        audioSource.clip = mainSound;
        if (mainSound != null){
            audioSource.Play();
        }
        animate.Play("main");
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
}
