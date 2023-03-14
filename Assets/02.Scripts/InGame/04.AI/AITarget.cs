/*
작성자: 최재호(cjh0798@gmail.com)
기능: AI Navigation의 타겟
 */
using UnityEngine;

public class AITarget : MonoBehaviour
{
    public AITargetTag targetTag;
    public AITargetOrderCount orderCount;
    public bool IsDead;
    public float stoppingDistance;
}
