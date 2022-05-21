using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class CameraSizePerScreenSize : MonoBehaviour
{
    public new bool enabled;
    public float sceneWidth;
    public float testSceneWidth;

    Camera _camera;

    void Awake()
    {
        _camera = GetComponent<Camera>();

        testSceneWidth = _camera.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x -
                         _camera.ScreenToWorldPoint(Vector3.zero).x;
        
        if (!enabled) return;

        float unitsPerPixel = sceneWidth / Screen.width;

        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

        _camera.orthographicSize = desiredHalfHeight;
    }
}

