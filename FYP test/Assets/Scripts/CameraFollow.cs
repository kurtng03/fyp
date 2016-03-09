using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    public playerController target;
    public Vector3 focusAreaSize;

    public float verticalOffset;

    FocusArea focusArea;
    void Start()
    {
        focusArea = new FocusArea(target.GetComponent<Collider>().bounds, focusAreaSize);
    }

    void LateUpdate()
    {
        focusArea.Update(target.GetComponent<Collider>().bounds);

        Vector3 focusPosition = focusArea.centre + Vector3.up * verticalOffset;
        transform.position = focusPosition + Vector3.forward * -10;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(focusArea.centre, focusAreaSize);
    }

    struct FocusArea
    {
        public Vector3 centre;
        public Vector3 velocity;
        float left, right;
        float top, bottom;

        public FocusArea(Bounds targetBounds, Vector3 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector3.zero;
            centre = new Vector3((left + right) / 2, (top + bottom) / 2, 0);
        }


        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            // Y 
            float shiftY = 0;
            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;
            centre = new Vector3((left + right) / 2, (top + bottom) / 2, 0);

            velocity = new Vector3(shiftX, shiftY);
        }
    }
}
