

using Oculus.Platform;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
// using UnityEditor.VersionControl;
using UnityEngine;

public class RunRaycast : MonoBehaviour
{
    [SerializeField]
    private GameObject _placeObj, _someToStart, _someObjectToPlace, _leftControllerAnchor;
    [SerializeField]
    private Camera _centerEyeCamera;

    EnvironmentDepthAccess _environmentDepthAccess;



    // Start is called before the first frame update
    void Start()
    {
        _environmentDepthAccess = GetComponent<EnvironmentDepthAccess>();

    }

    // Update is called once per frame
    void Update()
    {


        // **** camera **** //
        var viewSpaceCoordinate = new Vector2(0.2f, 0.35f);

        // Perform a ray cast
        var raycastResult = _environmentDepthAccess.RaycastViewSpaceBlocking(viewSpaceCoordinate);

        Vector3 actualPos;
        actualPos = new Vector3 (raycastResult.Position.x/3 + 2*_centerEyeCamera.transform.position.x/3, raycastResult.Position.y/3 + 2*_centerEyeCamera.transform.position.y/3, raycastResult.Position.z/3 + 2*_centerEyeCamera.transform.position.z/3);
        // Instantiate(_someObjectToPlace);
        // position some object on the ray hit
        _someObjectToPlace.transform.position = actualPos;

        //float calcDistance = Vector3.Distance(actualPos, _centerEyeCamera.transform.position);
        //float DisA = Vector3.Distance(raycastResult.Position, _centerEyeCamera.transform.position);
        //var message = "";
        //message += DisA.ToString();
        //message += "\t";
        //message += (DisA / 2).ToString();
        //message += "\t";
        //message += (DisA - 1).ToString();
        
        //Debug.Log(message);

        // use normal to rotate the indicator (be aware that LookRotation takes forward not up direction)
        // _someObjectToPlace.transform.rotation = Quaternion.LookRotation(raycastResult.Normal);

        if (GetComponent<WebClient>().NewValue)
        {
            // new value comes, use the info to place markers
            RootObject info = GetComponent<WebClient>().ObjInfo;
            var videoX = info.videoInfo[0];
            var videoY = info.videoInfo[1];

            // access each detected obj
            foreach (var detection in info.detections)
            {
                var me = "";
                me += detection.categories[0].categoryName;
                me += "\t";

                Instantiate(_placeObj);
                var objX = (detection.boundingBox.originX + detection.boundingBox.width/2) / videoX;
                var objY = (detection.boundingBox.originY + detection.boundingBox.height/2) / videoY;

                me += objX;
                me += "\t";
                me += objY;
                me += "\t";


                objX = MapValue(objX, 0f, 1f, 0.2f, 0.8f);
                objY = MapValue(objY, 0f, 1f, 0.8f, 0.35f);

                me += "calibration: ";
                me += objX;
                me += "\t";
                me += objY;

                var objCoordinates = new Vector2(objX, objY);

                // Perform a ray cast
                var rayResult = _environmentDepthAccess.RaycastViewSpaceBlocking(objCoordinates);

                Vector3 objPos;
                objPos = new Vector3(rayResult.Position.x / 3 + 2 * _centerEyeCamera.transform.position.x / 3, rayResult.Position.y / 3 + 2 * _centerEyeCamera.transform.position.y / 3, rayResult.Position.z / 3 + 2 * _centerEyeCamera.transform.position.z / 3);
                // Instantiate(_someObjectToPlace);
                // position some object on the ray hit
                _placeObj.transform.position = objPos;
                _placeObj.transform.GetChild(0).GetComponent<TMP_Text>().SetText(detection.categories[0].categoryName);

                Debug.Log(me);


            }

            GetComponent<WebClient>().NewValue = false;

        }

    }

    public float MapValue(float OldValue, float OldMin, float OldMax, float NewMin, float NewMax)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

}


