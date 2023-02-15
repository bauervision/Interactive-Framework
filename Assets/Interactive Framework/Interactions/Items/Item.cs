using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace InteractiveFramework.Interactions.Items
{

    public class Item : MonoBehaviour
    {
        public bool hasMetCondition;
        public bool hideWhenTriggered;
        public UnityEvent itemTriggerSuccess = new UnityEvent();
        public UnityEvent itemTriggerFail = new UnityEvent();
        public UnityEvent onConditionMet = new UnityEvent();
        public UnityEvent ExitTrigger = new UnityEvent();


        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player Right Hand")
            {
                if (hasMetCondition)
                {
                    itemTriggerSuccess.Invoke();

                    if (hideWhenTriggered)
                        Destroy(this.gameObject);
                }
                else
                    itemTriggerFail.Invoke();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                ExitTrigger.Invoke();
            }
        }

        public void ConditionHasBeenMet() { hasMetCondition = true; onConditionMet.Invoke(); }
    }

}
