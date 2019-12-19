using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakesEyeContact : MonoBehaviour
{
    // Start is called before the first frame update
    Transform makesEyeContact;
    bool isMakesEyeContact;
    float timer;
    float full;
    enum modes {looking, playing, eyeContact, stopEyeContact};
    modes mode;
    Quaternion start;
    Vector3 temp0;
    Transform temp1;

    [HideInInspector]
    public GameObject snob;

    void Start()
    {
        mode = modes.playing;
        if(GameObject.FindWithTag("makesEyeContact")!=null){
            makesEyeContact = GameObject.FindWithTag("makesEyeContact").GetComponent<Transform>();
            isMakesEyeContact = true;
        }
        else{
            isMakesEyeContact = false;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        void LateUpdate(){
        // if (mode == modes.looking){

        //     return;
        // }
            //makesEyeContact.LookAt(Camera.main.gameObject.transform);
          if ((Time.frameCount % 300 == 0) && (mode == modes.playing)){
                // start = makesEyeContact.eulerAngles;
                // temp1 = makesEyeContact;
                // temp1.LookAt(Camera.main.gameObject.transform);
                // temp2 = temp1.eulerAngles;
                // end = temp2 += (new Vector3(-120,0,0));
                timer = 1;
                full = 1;
                mode = modes.eyeContact;
                //LerpRotation(1.0f, makesEyeContact, start,end ); // time, what, start, end,
            }
            if (mode == modes.eyeContact && isMakesEyeContact == true){
                timer -= Time.deltaTime;
                timer = Mathf.Clamp(timer,0,float.MaxValue);

                float p = 1-(timer/full);

                // if (curve != null)
                //     p = curve.Evaluate(p);
                //start = new Vector3(makesEyeContact.eulerAngles.x,makesEyeContact.eulerAngles.y,makesEyeContact.eulerAngles.z);
                start = Quaternion.Euler(makesEyeContact.eulerAngles);
                temp1 = makesEyeContact;
                temp1.LookAt(Camera.main.gameObject.transform);
                temp1.eulerAngles += (new Vector3(240,0,0));
                //end = temp2 += (new Vector3(240,0,0));
                Debug.Log("looooking");
                if(Mathf.Abs(temp1.eulerAngles.y - start.y) > 90.0f){
                    timer = 0;
                    Animator anim = snob.GetComponent<Animator>();
                    if (temp1.eulerAngles.y - start.y > 0){ // pos
                        anim.SetFloat("R Direction", (temp1.eulerAngles.y - start.y));
                    }
                    else{
                        anim.SetFloat("L Direction", - (temp1.eulerAngles.y - start.y));
                    }
                }
                makesEyeContact.rotation = Quaternion.Lerp(start,temp1.rotation,p);
                if (timer <= 0){
                    mode = modes.stopEyeContact;
                    timer = 1;
                    full = 1;
                }
            }
                if (mode == modes.stopEyeContact && isMakesEyeContact == true){
                timer -= Time.deltaTime;
                timer = Mathf.Clamp(timer,0,float.MaxValue);

                float p = 1-(timer/full);

                // if (curve != null)
                //     p = curve.Evaluate(p);
                //start = new Vector3(makesEyeContact.eulerAngles.x,makesEyeContact.eulerAngles.y,makesEyeContact.eulerAngles.z);
                start = Quaternion.Euler(makesEyeContact.eulerAngles);
                temp1 = makesEyeContact;
                temp1.LookAt(Camera.main.gameObject.transform);
                temp1.eulerAngles += (new Vector3(240,0,0)); // is this magic?
                
                //end = temp2 += (new Vector3(240,0,0));
                //Debug.Log(p);
                makesEyeContact.rotation = Quaternion.Lerp(temp1.rotation,start,p);
                if (timer <=0){
                    mode = modes.playing;
                }
            }
        //}
    }
}
