//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

using UnityEngine;
using UnityEngine.Internal;

/// <summary>
/// Unity script for a game object that follows the position of an eye in 3D space.
/// </summary>
[RequireComponent(typeof(EyePositionDataComponent), typeof(Renderer))]
public class FollowEyePosition : MonoBehaviour
{
    // Scale: 1 mm maps to 0.001 units in world space
    private const float Scale = 1 / 1000.0f;    

    private EyePositionDataComponent _eyePositionDataComponent;
    private Renderer _rendererComponent;
    private float _accumulatedTime;

    /// <summary>
    /// Choice of eye position to follow, the position of the right or the left eye.
    /// </summary>
    public Eye eyeToFollow = Eye.Left;

    /// <summary>
    /// The threshold in milliseconds to be reached before the eye is hidden
    /// after eye tracking has been lost. This prevents flickering 
    /// if the calibration is suboptimal.
    /// </summary>
    [Range(0, 1000)]
    public float thresholdMs = 100f;

    /// <summary>
    /// Represents an eye.
    /// </summary>
    public enum Eye
    {
        Left,
        Right
    }

    protected void Start()
    {
        _eyePositionDataComponent = GetComponent<EyePositionDataComponent>();
        _rendererComponent = GetComponent<Renderer>();
        _rendererComponent.enabled = false;
        _accumulatedTime = 0f;
    }

    protected void Update()
    {
        // Get the latest eye position data for both eyes from the eye position data component.
        var eyePosition = _eyePositionDataComponent.LastEyePosition;       
        var renderEye = false;

        if (eyePosition != null && eyePosition.IsValid)
        {
            // Get the eye position of the selected eye to follow.
            var singleEyePosition = eyeToFollow == Eye.Left ? eyePosition.LeftEye : eyePosition.RightEye;
            if (singleEyePosition.IsValid)
            {
                // Show the game object.
                _rendererComponent.enabled = true;

                // Move the game object to the current position of the selected eye to follow
                transform.position = new Vector3(-singleEyePosition.X * Scale, singleEyePosition.Y * Scale,
                    singleEyePosition.Z * Scale);

                // Reset the accumulated time.
                _accumulatedTime = 0f;

                // Show the eyes.
                renderEye = true;
            }
        }

        if (!renderEye)
        {
            // Increase the accumulated time with
            // the time elapsed since the last update.
            _accumulatedTime += Time.deltaTime;

            // Reached the threshold to hide the game object?
            if (_accumulatedTime >= (thresholdMs / 1000f))
            {
                // If there is no position for the eye to follow, 
                // for example during blink: hide the game object.
                _rendererComponent.enabled = false;
            }            
        }
    }
}