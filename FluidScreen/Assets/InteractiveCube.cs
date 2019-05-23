using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveCube : MonoBehaviour
{
    public MeshRenderer mr;
    public Transform cubeTransform;
    public CubeGrid motherGrid;

    public float distance;
    public float closePercent;

    void Start()
    {
        motherGrid = CubeGrid.instance;
    }

    void Update()
    {
        {
            if (motherGrid.allDown)
                closePercent = 0;
            else if (motherGrid.allUp)
                closePercent = 1;
            else
            {
                distance =
                    Vector3.SqrMagnitude(
                        motherGrid.mousePosition - transform.position);
                closePercent = distance > motherGrid.maxDistance ? 0 : distance == 0 ? 0 : 1 - distance / motherGrid.maxDistance;
            }
        }
        {
            Vector3 targetHeight = Vector3.up * motherGrid.hPlus * closePercent;
            cubeTransform.localPosition = Vector3.Lerp(cubeTransform.localPosition, targetHeight, motherGrid.influenceSpeed);
        }
        {
            Color targetColor = Color.Lerp(motherGrid.colorDefault, motherGrid.colorClose, closePercent);
            mr.material.color = Color.Lerp(mr.material.color, targetColor, motherGrid.influenceSpeed);
        }
    }
}
