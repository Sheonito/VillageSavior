/*
작성자: 최재호(cjh0798@gmail.com)
기능: 애니메이션 유틸 기능을 static 함수로 제공
 */ 
using System.Linq;
using UnityEngine;

public static class AnimationUtil
{
    // 애니메이션 Clip 시간 가져오기
    public static int GetAnimationDelay(Animator animator,string clipName)
    {
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;
        AnimationClip clip = controller.animationClips.First(x => x.name == clipName);
        int clipDelay = (int)(clip.length * 1000);

        return clipDelay;
    }
}
