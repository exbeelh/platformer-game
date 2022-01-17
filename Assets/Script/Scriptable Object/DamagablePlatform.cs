using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagablePlatform : MonoBehaviour
{
    [SerializeField]
    private float
        lastTouchDamageTime,
        touchDamageCooldown,
        touchDamage,
        touchDamageWidth,
        touchDamageHeight;
    [SerializeField]
    private Transform
        touchDamageCheck;
    [SerializeField]
    private LayerMask
        whatIsPlayer;

    private AttackDetails attackDetails;

    private Vector2
        touchDamageBotLeft,
        touchDamageTopRight;

    private void Update()
    {
        CheckTouchDamage();
    }

    private void CheckTouchDamage()
    {
        if (Time.time >= lastTouchDamageTime + touchDamageCooldown)
        {
            touchDamageBotLeft.Set(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
            touchDamageTopRight.Set(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

            Collider2D hit = Physics2D.OverlapArea(touchDamageBotLeft, touchDamageTopRight, whatIsPlayer);

            if (hit != null)
            {
                lastTouchDamageTime = Time.time;
                attackDetails.damageAmount = 10;
                hit.SendMessage("Damage", attackDetails);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 botLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 botRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 topRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));
        Vector2 topLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);
    }
}
