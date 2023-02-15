using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace InteractiveFramework.CloudAccess
{
    public class AWSPost : PostData
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
            Debug.Log($"Azure POST: returned data : {CloudDataManager.instance.GetCloudData()}");
        }
    }
}
