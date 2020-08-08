using UnityEngine;

public class InputManager : Singelton<InputManager>
{
    public Vector2 CurrentTouchPoint
    {
        get
        {
            return CameraEngine.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    public bool IsPointerOverUI
    {
        get
        {
            return EventManager.Instance.IsPointerOverGameObject;
        }
    }

    private bool isDrawing;

    private void Update()
    {  
        if (Input.GetMouseButtonDown(0))
        {         
            if (IsPointerOverUI.Equals(false))
            {
                isDrawing = true;

                LevelManager.Instance.CreateNewLine();
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (isDrawing && IsPointerOverUI.Equals(false))
            {
                LevelManager.Instance.UpdateCurrentLine();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;

            LevelManager.Instance.TryPlaceLine();
        }
    }
}
