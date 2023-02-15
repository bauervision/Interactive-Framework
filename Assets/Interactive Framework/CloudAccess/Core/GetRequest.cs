using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace InteractiveFramework.CloudAccess
{


    public class GetRequest
    {

        /// <summary>
        /// Handle all errors and data that is returned
        /// </summary>
        /// <param name="webRequest"></param>
        /// <returns></returns>
        public IEnumerator GetData(UnityWebRequest webRequest)
        {
            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequest.error);
                    SetIncomingCloudData(webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + webRequest.error);
                    SetIncomingCloudData(webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    SetIncomingCloudData(webRequest.downloadHandler.text);
                    Debug.Log(": Success: " + webRequest.downloadHandler.text);
                    break;
            }

        }

        void SetIncomingCloudData(string incomingData)
        {
            CloudDataManager.instance.SetCloudData(incomingData);
        }
    }



}

