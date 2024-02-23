using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어를 따라가는 상태
/// </summary>
public class ChasingState : EnemyStateBase
{
    public AttackState attackState;

    public override EnemyStateBase EnterCurrentState()
    {
        // 뒤로 물러나기
        Debug.Log("chasing enter");
        enemy.speed = enemy.baseSpeed;
        return this;
    }



    public override EnemyStateBase RunCurrentState()
    {
        enemy.direction = enemy.Player.transform.position - transform.position; // 플레이어 방향 백터
        enemy.Anim.SetFloat(enemy.SpeedToHash, enemy.speed); // 이동 애니메이션
        MoveToPlayer();
        RotateToPlayer();

        if (enemy.direction.magnitude <= enemy.attackRange) // 플레이어 근처에 도달
        {
            Debug.Log("Attack으로 전환");
            return attackState;
        }

        return this;
    }
    public override EnemyStateBase ExitCurrentState()
    {
        return this;
    }

    /// <summary>
    /// 플레이어한테 이동하는 함수
    /// </summary>
    void MoveToPlayer()
    {
        enemy.Rigid.MovePosition(enemy.Rigid.position + Time.fixedDeltaTime * enemy.direction.normalized * enemy.speed);
    }

    /// <summary>
    /// 플레이어를 향해 회전하는 함수
    /// </summary>
    void RotateToPlayer()
    {
        Vector3 rotDirection = Vector3.zero;
        rotDirection.x = enemy.direction.x;
        rotDirection.z = enemy.direction.z;
        rotDirection.Normalize();

        if (rotDirection.magnitude > 0.01f)
        {
            enemy.lookAngle = Mathf.Atan2(rotDirection.x, rotDirection.z) * Mathf.Rad2Deg; // 회전할 방향
        }

        float angle = Mathf.LerpAngle(enemy.transform.localRotation.eulerAngles.y, enemy.lookAngle, enemy.rotateSpeed * Time.fixedDeltaTime);
        enemy.transform.localRotation = Quaternion.Euler(0, angle, 0); // rotate Player model
    }
}
