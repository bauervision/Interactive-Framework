using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace InteractiveFramework.ProAssets
{
    [RequireComponent(typeof(LineRenderer))]
    public class LineDrawer : MonoBehaviour
    {
        public TextMeshProUGUI distanceText;
        float distance;
        LineRenderer lineRenderer;
        Vector3 mousePos, startMousePos;


        // Start is called before the first frame update
        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
        }

        // Update is called once per frame
        void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Space))
            // {
            if (Input.GetMouseButtonDown(0))
            {
                startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                print("startMousePos " + startMousePos);
            }

            if (Input.GetMouseButton(0))
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                lineRenderer.SetPosition(0, new Vector3(startMousePos.x, 0f, startMousePos.y));
                lineRenderer.SetPosition(1, new Vector3(mousePos.x, 0f, mousePos.y));
                distance = (mousePos - startMousePos).magnitude;
                if (distanceText != null)
                    distanceText.text = distance.ToString("F2") + " meters";
                else
                    print(distance.ToString("F2") + " meters");
            }
            // }

        }
    }
}