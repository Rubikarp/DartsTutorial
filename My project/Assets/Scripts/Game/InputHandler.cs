using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public enum InputPhase
{
    None = 0,
    First = 1,
    Second = 2,
    Waiting = 3,
}

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Vector3 StartPose;
    [SerializeField] private Vector3 EndPose;
    [SerializeField] private float GaugeValue;

    [SerializeField, Space(10), MinMaxSlider(0, 10)]
    private Vector2 ForceRange = new Vector2(2, 5);

    public InputPhase InputPhase;
    public UnityEvent<Vector3> OnSetUp;
    public UnityEvent<Vector3> OnAim;
    public UnityEvent<float> OnLaunch;

    [SerializeField] private CameraPlane StartPlane;
    [SerializeField] private CameraPlane EndPlane;

    void Start()
    {
        InputPhase = InputPhase.First;
    }

    void Update()
    {
        switch (InputPhase)
        {
            case InputPhase.First:
                FirstTouchHandling();
                break;
            case InputPhase.Second:
                SecondTouchHandling();
                break;
            default:
                return;
        }
    }

    private void FirstTouchHandling()
    {
        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    StartPose = StartPlane.GetMouseHitPos();
                    OnSetUp.Invoke(StartPose);
                    break;
                case TouchPhase.Moved:
                    EndPose = EndPlane.GetMouseHitPos();
                    OnAim.Invoke(EndPose);
                    break;
                case TouchPhase.Ended:
                    EndPose = EndPlane.GetMouseHitPos();
                    OnAim.Invoke(EndPose);
                    InputPhase++;
                    break;
                default:
                    break;
            }
        }

    }
    private void SecondTouchHandling()
    {
        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                OnLaunch.Invoke(Mathf.Lerp(ForceRange.x, ForceRange.y, GaugeValue));
                InputPhase++;
            }
        }
        else
        {
            GaugeValue += Time.deltaTime;
            GaugeValue %= 1;
        }

    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(StartPose, EndPose, Color.red);
    }
}
