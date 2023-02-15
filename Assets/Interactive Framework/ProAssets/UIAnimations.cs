using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveFramework.ProAssets
{
    public class UIAnimations : MonoBehaviour
    {
        public Animator scrollRectAnimator;
        bool showModels;
        public void ToggleModelView()
        {
            showModels = !showModels;
            scrollRectAnimator.SetBool("Show", showModels);
        }
    }
}
