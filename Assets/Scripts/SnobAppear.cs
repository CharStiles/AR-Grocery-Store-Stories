using System;
using System.Collections.Generic;

namespace UnityEngine.XR.iOS
{
	public class SnobAppear : MonoBehaviour
	{
		public GameObject[] snobs;
        Transform[] m_HitTransform;
        GameObject snob;

        ARPoint point = new ARPoint {
        x = 0.5,
        y = 0.5
        };

        void Start(){
            for(int i = 0 ; i < snobs.Length ; i++){
                m_HitTransform[i] = snobs[i].transform;
                snobs[i].SetActive(false);
            }
            
            
        }

        bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
        {
            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
            if (hitResults.Count > 0) {

                
                foreach (var hitResult in hitResults) {
                    Debug.Log ("Got hit! in snobAppear");
                    float dist = Vector3.Distance(UnityARMatrixOps.GetPosition (hitResult.worldTransform), Camera.main.transform.position);
                    if (dist <2.0f || dist > 3.0f){
                        continue;
                    }
                    for(int i = 0 ; i < snobs.Length ; i++){
                        snob = snobs[i];
                        snob.SetActive(true);
                        snob.transform.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);

                        snob.transform.LookAt(Camera.main.transform);
                        snob.transform.eulerAngles = new Vector3(0, snob.transform.eulerAngles.y,snob.transform.eulerAngles.z);
                        //m_HitTransform.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);
                        Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", snob.transform.position.x, snob.transform.position.y, snob.transform.position.z));
                    }
                    return true;
                }
                
            }
            return false;
        }
		
		// Update is called once per frame
		void Update () {
            if (snobs.Length == 0 ){
                return;
            }
			if (snobs[0].activeSelf == false)
            {
                // prioritize reults types
                ARHitTestResultType[] resultTypes = {
                    //ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
                    // if you want to use infinite planes use this:
                    //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                    ARHitTestResultType.ARHitTestResultTypeEstimatedHorizontalPlane, 
                   // ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingGeometry, 
                   // ARHitTestResultType.ARHitTestResultTypeFeaturePoint
                }; 
                
                foreach (ARHitTestResultType resultType in resultTypes)
                {
                    if (HitTestWithResultType (point, resultType))
                    {
                        
                        // if (snobCan.activeSelf){
                        //     snobCan.SetActive(false);
                        // }
                        return;
                    }
                }
			}

		}
	}
}
