using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace InteractiveFramework.CloudAccess
{
    public class FirebasePost : PostData
    {
        public IEnumerator PostNewData(string url, string data)
        {
            using (UnityWebRequest newRequest = UnityWebRequest.Post(url, data))
            {
                yield return PostNewData(newRequest);
                // now do something once the fetch is completed
                SendFinished();
            }
        }

        void SendFinished()
        {
            Debug.Log($"Firebase POST: returned data : {CloudDataManager.instance.GetCloudData()}");
        }
    }
}
