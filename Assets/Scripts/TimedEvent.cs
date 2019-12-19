using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour {

    public static EventManager instance
    {
        get
        {
            if (_instance == null && Application.isPlaying)
            {
                var instance = new GameObject("TimedEvent Manager");
                DontDestroyOnLoad(instance);
                _instance = instance.AddComponent<EventManager>();
            }
            return _instance;
        }
    }

    private static EventManager _instance;

    public static void PerformUntil(Action performer, Func<bool> until, Action onComplete )
    {
        instance.StartCoroutine(instance.performUntil(performer, until, onComplete));
    }


    protected IEnumerator performUntil(Action performer, Func<bool> until, Action onComplete, int frameMax = -1)
    {

        yield return new WaitForEndOfFrame();
        int count = 0;
        while (!until())
        {
            count++;

            if (frameMax != -1 && count > frameMax)
                break;

            performer();
            yield return new WaitForEndOfFrame();
        }

        if (onComplete != null)
            onComplete();

    }

    public static void startTimedEvent<T>(float t, Action<T> action, T arg)
    {
         instance.StartCoroutine(instance.timedEvent(t, action, arg));
    }

    protected IEnumerator timedEvent<T>(float time, Action<T> action, T arg)
    {
        yield return new WaitForSeconds(time);
        if (action != null)
        action.Invoke(arg);
    }

    public static void LerpPosition(float t, Transform transform, Vector3 startPos, Vector3 endPos , Action onComplete = null, AnimationCurve curve = null)
    {
        instance.StartCoroutine(instance.lerpPosition(t,transform,startPos,endPos,onComplete,curve));
    }

    protected IEnumerator lerpPosition(float t, Transform transform,  Vector3 startPos, Vector3 endPos, Action onComplete = null, AnimationCurve curve = null)
    {
        float full = t;
        float timer = t;

        while ( timer > 0 )
        {
            timer -= Time.deltaTime;
            timer = Mathf.Clamp(timer,0,float.MaxValue);

            float p = 1-timer/full;

            if (curve != null)
                p = curve.Evaluate(p);

            transform.position = Vector3.LerpUnclamped(startPos,endPos,p);
            yield return new WaitForEndOfFrame();
        }
        if (onComplete != null)
            onComplete();

    }
        public static void LerpRotation(float t, Transform transform, Vector3 startPos, Vector3 endPos , Action onComplete = null, AnimationCurve curve = null)
    {
        instance.StartCoroutine(instance.lerpRotation(t,transform,startPos,endPos,onComplete,curve));
    }

    protected IEnumerator lerpRotation(float t, Transform transform,  Vector3 startPos, Vector3 endPos, Action onComplete = null, AnimationCurve curve = null)
    {
        float full = t;
        float timer = t;

        while ( timer > 0 )
        {
            timer -= Time.deltaTime;
            timer = Mathf.Clamp(timer,0,float.MaxValue);

            float p = 1-timer/full;

            if (curve != null)
                p = curve.Evaluate(p);
                
            Debug.Log("in here");
            transform.eulerAngles = Vector3.LerpUnclamped(startPos,endPos,p);
            yield return new WaitForEndOfFrame();
        }
        if (onComplete != null)
            onComplete();

    }

    public static void LerpRotationBack(float t, Transform transform , Action onComplete = null, AnimationCurve curve = null)
    {
        instance.StartCoroutine(instance.lerpRotationBack(t,transform,onComplete,curve));
    }

    protected IEnumerator lerpRotationBack(float t, Transform transform, Action onComplete = null, AnimationCurve curve = null)
    {
        float full = t;
        float timer = t;
        Vector3 startPos;
        Vector3 endPos;
        Vector3 temp;
        while ( timer > 0 )
        {
            timer -= Time.deltaTime;
            timer = Mathf.Clamp(timer,0,float.MaxValue);
            endPos = transform.position;
            
            transform.LookAt(Camera.main.gameObject.transform);
            temp = transform.eulerAngles;
            startPos = temp += (new Vector3(-120,0,0));
            
            //startPos = transform.LookAt(Camera.main.gameObject.transform).eulerAngles += (new Vector3(-120,0,0));
            float p = 1-timer/full;

            if (curve != null)
                p = curve.Evaluate(p);

            transform.eulerAngles = Vector3.LerpUnclamped(startPos,endPos,p);
            yield return new WaitForEndOfFrame();
        }
        if (onComplete != null)
            onComplete();

    }

    public static void LerpValue(float t, Action<float> setter, Action onComplete = null, AnimationCurve curve = null)
    {
        instance.StartCoroutine(instance.lerpValue(t,setter,onComplete,curve));
    }

    public static void LerpValue(float t, Action<float,float> setter, Action onComplete = null, AnimationCurve curve = null)
    {
        instance.StartCoroutine(instance.lerpValue(t,setter,onComplete,curve));
    }


    protected IEnumerator lerpValue(float t, Action<float> setter , Action onComplete = null, AnimationCurve curve = null)
    {
        float full = t;
        float timer = t;

        while ( timer > 0 )
        {
            timer -= Time.deltaTime;
            timer = Mathf.Clamp(timer,0,float.MaxValue);

            float p = 1-timer/full;

            if (curve != null)
                p = curve.Evaluate(p);

            setter(p);

            yield return new WaitForEndOfFrame();
        }

        if (onComplete != null)
            onComplete();

    }
        
    public static AudioSource PlaySoundOneShot(Transform t, Vector3 localPos, AudioClip clip, float volume, float time = 0, float endTime = -1)
    {
        var go = new GameObject();
        go.transform.parent = t;
        go.transform.localPosition = localPos;
        var source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.time = time;
        source.volume = volume;
        source.Play();
        instance.StartCoroutine(instance.destroySoundOnComplete(source, endTime));
        return source;
    }

    public IEnumerator destroySoundOnComplete(AudioSource source, float endTime)
    {

        if (endTime == -1)
            endTime = source.clip.length + .1f;

        yield return new WaitForSeconds(endTime);
        Destroy(source.gameObject);
    }

    protected IEnumerator lerpValue(float t, Action<float,float> setter , Action onComplete = null, AnimationCurve curve = null)
    {
        float full = t;
        float timer = t;

        while ( timer > 0 )
        {
            timer -= Time.deltaTime;
            timer = Mathf.Clamp(timer,0,float.MaxValue);

            float p = 1-timer/full;

            if (curve != null)
                p = curve.Evaluate(p);

            setter(p,1-timer/full);

            yield return new WaitForEndOfFrame();
        }

        if (onComplete != null)
            onComplete();

    }

    public static void startTimedEvent(float t, Action action)
    {
        instance.StartCoroutine(instance.timedEvent(t, action));
    }

    protected IEnumerator timedEvent(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        
        if (action != null)
        action.Invoke();
    }

    public static void startInfiniteTimedLoop(float t, Action action)
    {
        instance.StartCoroutine(instance.infiniteTimedLoop(t,action));
    }

    protected IEnumerator infiniteTimedLoop(float t, Action action)
    {
        while (true)
        {
            action();
            yield return new WaitForSeconds(t);
        }
    }


}
