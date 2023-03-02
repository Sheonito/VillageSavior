/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: �ִϸ��̼� ��ƿ ����� static �Լ��� ����
 */ 
using System.Linq;
using UnityEngine;

public static class AnimationUtil
{
    // �ִϸ��̼� Clip �ð� ��������
    public static int GetAnimationDelay(Animator animator,string clipName)
    {
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;
        AnimationClip clip = controller.animationClips.First(x => x.name == clipName);
        int clipDelay = (int)(clip.length * 1000);

        return clipDelay;
    }
}
