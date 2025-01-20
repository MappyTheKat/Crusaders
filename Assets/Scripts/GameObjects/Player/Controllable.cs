using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// 유저의 키 인풋을 캐치하고 캐릭터 커맨드를 실행하는, 인게임용 컨트롤 컴포넌트.
/// 메뉴 등이 뜰 때는, 다른 메뉴 선택용 컴포넌트를 사용한다.
/// </summary>
public class Controllable : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 항상 유저의 키 인풋을 캐치한다.
    }

    void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
    }
}
