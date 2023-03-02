using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [HideInInspector] public BattleEntity ownerEntity;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Untagged"))
            return;

        Log.PrintLogMiddleLevel(other.name + "이 맞았습니다.");
        BattleEntity targetEntity = other.GetComponent<BattleEntity>();

        if (targetEntity.ID != ownerEntity.ID)
        {
            targetEntity.OnDamaged(ownerEntity.attackPower, ownerEntity.ID);
        }

    }
}
