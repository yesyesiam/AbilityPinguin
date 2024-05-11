using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDoubleAbility : Ability
{
    private GameObject VFX;

    public ShadowDoubleAbility(GameObject vfx)
    {
        VFX = vfx;
    }
    public override void ApplyCast()
    {
        Debug.Log("let's let in a double!");
        /*var rows = RowManager.NextRows();
        if (rows.Length > 0)
        {
            Vector3 initialPosition = TestOne.transform.position + TestOne.transform.forward * 1;
            var snowball = GameObject.Instantiate(VFX, initialPosition, Quaternion.LookRotation(RowManager.GetCurentDirection()));
            TestOne.StartCoroutine(MoveThroughPoints(rows, snowball.transform));

            ChangeCooldownCount(CooldownTotal);
        }*/
    }

    /*IEnumerator MoveThroughPoints(Row[] positions, Transform ball)
    {
        bool stopMoving = false;
        foreach (var targetRow in positions)
        {
            if (stopMoving) { yield break; }
            var index = targetRow.GetAliveIndex(Random.Range(0, 2));
            yield return TestOne.StartCoroutine(TestOne.MoveFromTo(ball, targetRow.GetPosition(index), () =>
            {
                if (targetRow.PerformPlatformAction(index))
                {
                    GameObject.Destroy(ball.gameObject, 0.5f);
                    stopMoving = true;
                }
                // Колбэк для завершения перемещения к текущей точке
                Debug.Log("Object moved to: " + targetRow);
            }));
        }
        GameObject.Destroy(ball.gameObject, 1f);
        Debug.Log("Movement completed");
    }*/
}
