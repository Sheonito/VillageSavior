/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: Player ī�޶�
 */
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public Vector3 offset;
    private Player _myPlayer;
    public void Setup(Player myPlayer)
    {
        _myPlayer = myPlayer;
    }

    private void Update()
    {
        if (_myPlayer == null)
            return;

        Camera.main.transform.position = _myPlayer.transform.position + offset;
    }
}
