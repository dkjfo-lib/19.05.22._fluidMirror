using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveCube : MonoBehaviour
{
    static int id;

    public MeshRenderer mr;
    public Transform cubeTransform;
    public CubeGrid motherGrid;

    public float distance;
    public float closePercent;
    [HideInInspector] public int _id;

    void Start()
    {
        motherGrid = CubeGrid.instance;
        _id = id++;
    }

    void Update()
    {
        {
            float targetClosePercent;
            if (motherGrid.allDown)
                targetClosePercent = 0;
            else if (motherGrid.allUp)
                targetClosePercent = 1;
            else
            {
                distance =
                    Vector3.SqrMagnitude(
                        motherGrid.mousePosition - transform.position);
                targetClosePercent = distance > motherGrid.maxDistance ? 0 : distance == 0 ? 0 : 1 - distance / motherGrid.maxDistance;
            }
            float deltaPercent = targetClosePercent - closePercent;
            closePercent += deltaPercent * motherGrid.influenceSpeed;
        }
        {
            cubeTransform.localPosition = Vector3.up * motherGrid.hPlus * closePercent;
        }
        {
            mr.material.color = motherGrid.GetColor(_id, closePercent);
        }
    }
}
