using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace InteractiveFramework.CloudAccess
{
    public class FirebaseGet : GetRequest
    {
        public IEnumerator SendNewRequest(string url)
        {
            using (UnityWebRequest newRequest = UnityWebRequest.Get(url))
            {
                yield return GetData(newRequest);
                // now do something once the fetch is completed
                SendFinished();
            }
        }

        void SendFinished()
        {
            Debug.Log($"Firebase GET: returned data : {CloudDataManager.instance.GetCloudData()}");
        }
    }
}
