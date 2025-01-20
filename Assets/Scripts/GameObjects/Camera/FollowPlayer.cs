using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Camera MainCamera;
    public GameObject PlayerCharacter;

    public bool isVerticalLock = false;
    public bool isHorizontalLock = false;

    public float InterpolationRate = 0.5f;

    // Use this for initialization
    void Start()
    {
        MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerCharacter)
        {
            // 스크롤락을 프로그래밍적으로 넣는게 맞는가?
            Vector3 CurrentCameraPosition = MainCamera.transform.position;
            Vector2 NewCameraPosition = Vector2.Lerp(PlayerCharacter.transform.position, CurrentCameraPosition, InterpolationRate);
            if (isVerticalLock)
            {
                NewCameraPosition.y = CurrentCameraPosition.y;
            }
            if (isHorizontalLock)
            {
                NewCameraPosition.x = CurrentCameraPosition.x;
            }
            Vector3 NewCameraPositionVector3 = new Vector3(NewCameraPosition.x, NewCameraPosition.y, CurrentCameraPosition.z);
            MainCamera.transform.position = NewCameraPositionVector3;
        }
    }

    public void SetPlayer(GameObject player)
    {
        PlayerCharacter = player;
    }
}
