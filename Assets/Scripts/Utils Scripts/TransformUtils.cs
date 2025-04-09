using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class TransformUtils : MonoBehaviour
{

    static public void AlignGameObjects(List<Transform> objects, Vector2 firstPosition, Vector2 lastPosition)
    {
        if (objects.Count == 0) return;
        if (objects.Count == 1)
        {
            objects[0].localPosition = firstPosition;
            return;
        }

        float firstX = firstPosition.x;
        float lastX = lastPosition.x;

        float firstY = firstPosition.y;
        float lastY = lastPosition.y;

        float offsetX = (lastX - firstX) / (objects.Count - 1);
        float offsetY = (lastY - firstY) / (objects.Count - 1);

        for (int i = 0; i < objects.Count; i++)
        {
            float currX = firstX + i * offsetX;
            float currY = firstY + i * offsetY;
            objects[i].localPosition = new Vector2(currX, currY);
        }
    }

    public void DestroyParent(Transform parent, float seconds)
    {
        StartCoroutine(DestroyParentCoroutine(parent, seconds));
    }

    IEnumerator DestroyParentCoroutine(Transform parent, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(parent.gameObject);
    }

    public void DestroyChildren(Transform parent, float seconds)
    {
        StartCoroutine(DestroyChildrenCoroutine(parent, seconds));
    }

    IEnumerator DestroyChildrenCoroutine(Transform parent, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    public void MoveOriginalToTarget(Transform transform, Vector3 targetPosition, float seconds, bool handleActive)
    {
        if (transform == null) return;
        StartCoroutine(MoveToPositionCoroutine(transform, targetPosition, seconds, handleActive));
    }

    public void MoveStartToScreen(Transform transform, Vector3 startPosition, float seconds, bool handleActive)
    {
        if (transform == null) return;
        transform.localPosition = startPosition;
        StartCoroutine(MoveToPositionCoroutine(transform, Vector3.zero, seconds, handleActive));
    }

    public void MoveStartToTarget(Transform transform, Vector3 startPosition, Vector3 targetPosition, float seconds, bool handleActive)
    {
        if (transform == null) return;
        transform.localPosition = startPosition;
        StartCoroutine(MoveToPositionCoroutine(transform, targetPosition, seconds, handleActive));
    }

    private IEnumerator MoveToPositionCoroutine(Transform transform, Vector3 targetPosition, float seconds, bool handleActive)
    {
        float elapsedTime = 0;
        Vector3 startPosition = transform.localPosition;

        while (elapsedTime < seconds)
        {
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / seconds);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetPosition;
        transform.gameObject.SetActive(handleActive);
    }

    public void StartRotation(Transform transform, Vector3 rotationDegrees, float seconds, bool handleActive)
    {
        StartCoroutine(RotateOverTime(transform, rotationDegrees, 0, seconds, handleActive));
    }

    private IEnumerator RotateOverTime(Transform transform, Vector3 rotationDegrees, float waitSeconds, float seconds, bool handleActive)
    {
        float elapsedTime = 0;
        Vector3 startRotation = transform.localEulerAngles;
        Vector3 targetRotation = startRotation + rotationDegrees;

        yield return new WaitForSeconds(waitSeconds);

        while (elapsedTime < seconds)
        {
            Vector3 newRotation = new Vector3(
                rotationDegrees.x != 0 ? Mathf.Lerp(startRotation.x, targetRotation.x, elapsedTime / seconds) : startRotation.x,
                rotationDegrees.y != 0 ? Mathf.Lerp(startRotation.y, targetRotation.y, elapsedTime / seconds) : startRotation.y,
                rotationDegrees.z != 0 ? Mathf.Lerp(startRotation.z, targetRotation.z, elapsedTime / seconds) : startRotation.z
            );

            transform.localEulerAngles = newRotation;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localEulerAngles = new Vector3(
            rotationDegrees.x != 0 ? targetRotation.x % 360f : startRotation.x,
            rotationDegrees.y != 0 ? targetRotation.y % 360f : startRotation.y,
            rotationDegrees.z != 0 ? targetRotation.z % 360f : startRotation.z
        );

        transform.gameObject.SetActive(handleActive);
    }

    public void ChangePosition(Transform transform, Vector3 position, float seconds)
    {
        StartCoroutine(ChangePositionCoroutine(transform, position, seconds));
    }

    private IEnumerator ChangePositionCoroutine(Transform transform, Vector3 position, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        transform.localPosition = position;
    }

    public void SetActive(Transform transform, bool setActive, float seconds)
    {
        StartCoroutine(SetActiveCoroutine(transform, setActive, seconds));
    }

    private IEnumerator SetActiveCoroutine(Transform transform, bool setActive, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        transform.gameObject.SetActive(setActive);
    }
}

