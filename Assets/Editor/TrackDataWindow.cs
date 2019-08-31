using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TrackDataWindow : EditorWindow
{

    private float DISTANCE_PER_POINT = 5f;

    private bool recordData = false;
    private PlayerInputProvider inputProvider = null;
    private List<Vector3> positionStack = new List<Vector3>();

    [MenuItem("MixAndJam/Windows/Track Data Editor")]
    public static void OpenTrackDataWindow()
    {
        GetWindow<TrackDataWindow>().Show();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Handles.BeginGUI();

        foreach (var point in positionStack)
        {
            Handles.SphereHandleCap(0, point, Quaternion.identity, .5f, EventType.Repaint);
        }

        Handles.EndGUI();
    }

    private void OnGUI()
    {
        RecordPath();
        RenderRecordButton();

        if (recordData)
            Repaint();
    }

    private void RecordPath()
    {
        if (inputProvider == null)
        {
            return;
        }

        if (positionStack.Count == 0)
        {
            return;
        }

        var currentPosition = inputProvider.transform.position;
        var lastPosition = positionStack[positionStack.Count - 1];
        var distanceFromLast = Vector3.Distance(currentPosition, lastPosition);
        if (distanceFromLast > DISTANCE_PER_POINT)
        {
            var delta = distanceFromLast - DISTANCE_PER_POINT;
            if (delta > 0f)
            {
                var directionToCurrent = (currentPosition - lastPosition).normalized;
                var adjustedPoint = lastPosition + (directionToCurrent * DISTANCE_PER_POINT);
                positionStack.Add(adjustedPoint);
            }
            else
            {
                positionStack.Add(currentPosition);
            }
        }
    }

    private void RenderRecordButton()
    {
        if (GUILayout.Button(recordData ? "Stop" : "Record"))
        {
            recordData = !recordData;

            if (recordData)
            {
                PlayerInputProvider playerInputProvider = Component.FindObjectOfType<PlayerInputProvider>();
                if (playerInputProvider == null)
                {
                    recordData = false;
                    return;
                }

                inputProvider = playerInputProvider;
                positionStack.Add(playerInputProvider.transform.position);
            }
            else
            {
                var newInstance = ScriptableObject.CreateInstance<TrackData>();
                newInstance.SetPoints(positionStack.ToArray());
                EditorUtility.SetDirty(newInstance);
                AssetDatabase.CreateAsset(newInstance, $"Assets/TrackData{DateTime.UtcNow.Ticks}.asset");

                TrackDataController trackDataController = Component.FindObjectOfType<TrackDataController>();
                if (trackDataController != null)
                {
                    trackDataController.TrackNodes = newInstance;
                    EditorUtility.SetDirty(trackDataController);
                }

                AssetDatabase.SaveAssets();

                inputProvider = null;
                positionStack.Clear();
            }
        }
    }

}
