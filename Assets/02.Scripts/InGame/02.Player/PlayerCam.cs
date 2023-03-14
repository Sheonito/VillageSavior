/*
작성자: 최재호(cjh0798@gmail.com)
기능: Player 카메라
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
