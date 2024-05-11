using System;
using UnityEngine;
using UnityEngine.Events;

public class SwipeDetection : MonoBehaviour
{
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    public float minDistanceForSwipe = 20f;

    public event Action OnUp;
    public event Action OnLeft;
    public event Action OnRight;

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUpPosition = touch.position;
                fingerDownPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }
        }
    }

    public void HandleEvent(int dir)
    {
        if (dir == 1)
        {
            OnUp?.Invoke();
        } 
        else if(dir == 2)
        {
            OnLeft?.Invoke();
        }
        else if (dir == 3)
        {
            OnRight?.Invoke();
        }
    }

    void DetectSwipe()
    {
        float verticalSwipe = fingerDownPosition.y - fingerUpPosition.y;
        float horizontalSwipe = fingerDownPosition.x - fingerUpPosition.x;

        if (Mathf.Abs(verticalSwipe) > minDistanceForSwipe && Mathf.Abs(horizontalSwipe) <= minDistanceForSwipe)
        {
            if (verticalSwipe < 0)
            {
                // Свайп вниз, игнорируем в этом случае
            }
            else
            {
                // Свайп вверх
                Debug.Log("Swipe Up");
                OnUp?.Invoke(); // Вызов события при свайпе вверх
            }
            fingerUpPosition = fingerDownPosition;
        }

        if (Mathf.Abs(horizontalSwipe) > minDistanceForSwipe && Mathf.Abs(verticalSwipe) <= minDistanceForSwipe)
        {
            if (horizontalSwipe < 0)
            {
                // Свайп влево
                Debug.Log("Swipe Left");
                OnLeft?.Invoke(); // Вызов события при свайпе влево
            }
            else
            {
                // Свайп вправо
                Debug.Log("Swipe Right");
                OnRight?.Invoke(); // Вызов события при свайпе вправо
            }
            fingerUpPosition = fingerDownPosition;
        }
    }
}

