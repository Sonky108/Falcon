using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfScreenTileController : MonoBehaviourSingleton<OutOfScreenTileController> {

    [SerializeField]
    private Transform FollowedObject;

    [SerializeField]
    private GameObject Wrapper;

    private RectTransform rectTransform;

    public override void OnAwake()
    {
        base.OnAwake();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        //Calling Camera.main is not a optimal way of getting actually rendering camera.
        //It is better to have reference here.
        var viewportResult = Camera.main.WorldToViewportPoint(FollowedObject.transform.position);

        IsOutOfScreen(viewportResult);
    }

    private void IsOutOfScreen(Vector3 viewportResult)
    {
        if(IsInViewport(viewportResult))
        {
            Wrapper.SetActive(false);
        }
        else
        {
            Wrapper.SetActive(true);
            SetPosition(viewportResult);
        }
    }

    private void SetPosition(Vector3 viewportResult)
    {
        if(viewportResult.y > 1)
        {
            var xValue = Mathf.Min(viewportResult.x * Screen.currentResolution.width, Screen.currentResolution.width);
            rectTransform.anchoredPosition = new Vector2(xValue, Screen.currentResolution.height);
        }

        if(viewportResult.x < 0)
        {
            var yValue = Mathf.Min(viewportResult.y * Screen.currentResolution.height, Screen.currentResolution.height);
            rectTransform.anchoredPosition = new Vector2(0, yValue);
        }
        else if(viewportResult.x > 1)
        {
            var yValue = Mathf.Min(viewportResult.y * Screen.currentResolution.height, Screen.currentResolution.height);
            rectTransform.anchoredPosition = new Vector2(Screen.currentResolution.width, yValue);
        }
    }

    private bool IsInViewport(Vector3 viewportResult)
    {
        var xViewport = IsInViewport(viewportResult.x);
        var yViewport = IsInViewport(viewportResult.y);

        return xViewport && yViewport;
    }

    private bool IsInViewport(float value)
    {
        return value >= 0 && value <= 1;
    }
}
