/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: Enemy�� Ÿ�� �˻�
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Ÿ�� �±�
public enum AITargetTag
{
    None,
    Building,
    NPC,
    Player,
}

// Ÿ�� �켱���� enum
public enum AITargetOrderCount
{
    One,
    Two,
    Three
}

// �ν����Ϳ��� �±��� Ÿ�� �켱������ ���ϱ� ���� Ŭ����
[Serializable]
public class AITargetOrder
{
    public AITargetTag targetTag;
    public AITargetOrderCount orderCount;
}

public class EnemyAIManager : MonoBehaviour
{

    public BuildingManager buildingManager;
    public List<AITargetOrder> targetOrder;

    private void Start()
    {
        Init();
    }

    // �ʱ�ȭ
    private void Init()
    {
        List<Building> buildings = buildingManager.buildings;
        for (int i = 0; i < buildings.Count; i++)
        {
            for (int j = 0; j < buildings[i].structures.Count; j++)
            {
                AITarget aiTarget = buildings[i].structures[j].aiTarget;
                UpdateTargetOrder(aiTarget);
            }
        }
    }

    // Ÿ�� ���� ������Ʈ
    public void UpdateTargetOrder(AITarget aiTarget)
    {
        AITargetOrder order = targetOrder.First(x => x.targetTag == aiTarget.targetTag);
        aiTarget.orderCount = order.orderCount;
    }

    // Ÿ�� ��������
    public AITarget GetTarget(Transform ownerTransform)
    {
        // ������ Player�� �ִ��� �˻�
        AITarget targetPlayer = SearchPlayer(ownerTransform);

        // ������ Player�� ���� ��
        if (targetPlayer == null)
        {
            // ���� ����� Building �˻�
            Building targetBuilding = GetTargetBuilding(ownerTransform);
            if (targetBuilding == null)
                return null;

            // Building�� �ı����� ���� Structure Listȭ
            List<Structure> structures = targetBuilding.structures.Where(x => x.IsDead == false).ToList();

            if (structures.Count == 0)
                return null;


            // ���� ����� Structure ��������
            AITarget resultTarget = structures[0]?.aiTarget;

            for (int i = 0; i < structures.Count; i++)
            {
                AITarget curTarget = structures[i].aiTarget;

                Transform curTrans = curTarget.transform;
                Vector3 curVec = curTrans.position - ownerTransform.position;
                Vector3 resultVec = resultTarget.transform.position - ownerTransform.position;

                // �� Structure�� �켱������ �� ���� ��
                if (resultTarget.orderCount <= curTarget.orderCount)
                {
                    // �� Structure�� �Ÿ��� �� ����� ��
                    if (resultVec.magnitude > curVec.magnitude)
                    {
                        resultTarget = curTarget;
                    }
                }
            }
            return resultTarget;
        }
        // ������ Player�� ���� ��
        else
        {
            return targetPlayer;
        }

    }

    // ���� ����� Building�� �˻�
    private Building GetTargetBuilding(Transform ownerTransoform)
    {
        List<Building> buildings = buildingManager.buildings.Where(x => x.IsDead == false).ToList();
        Building target = buildings.Count == 0 ? null : buildings[0];
        for (int i = 0; i < buildings.Count; i++)
        {
            Transform curTrans = buildings[i].transform;
            Vector3 curVec = curTrans.position - ownerTransoform.position;
            Vector3 targetVec = target.transform.position - ownerTransoform.position;

            if (targetVec.magnitude > curVec.magnitude)
            {
                target = buildings[i];
            }
        }
        return target;
    }


    // ���� ���� ���� Player�� �ִ� �� �˻�
    public AITarget SearchPlayer(Transform ownerTransform)
    {
        int layer = 1 << LayerMask.NameToLayer("Player");
        Collider[] cols = Physics.OverlapSphere(ownerTransform.position, 4f, layer);
        if (cols.Length > 0)
        {
            AITarget target = null;
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].GetComponent<AITarget>().IsDead == false)
                {
                    target = cols[i].GetComponent<AITarget>();
                }
            }
            return target;
        }
        else
            return null;
    }

}
