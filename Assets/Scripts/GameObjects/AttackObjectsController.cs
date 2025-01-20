using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjectsController : MonoBehaviour
{
    public PrefabGenerateInfo[] GeneratePrefabInfo;

    public GameObject RootObject;

    public GameObject HitObjectsRoot;

    void Awake()
    {
        if (!HitObjectsRoot)
            HitObjectsRoot = transform.Find("HitObjects").gameObject;
        if (!RootObject)
            RootObject = transform.root.gameObject;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GeneratePrefab(string prefabName)
    {
        for (int i = GeneratePrefabInfo.Length; i-- > 0;)
        {
            if (GeneratePrefabInfo[i].Name == prefabName)
            {
                GeneratePrefab(GeneratePrefabInfo[i]);
                return;
            }
        }
        Logger.DebugLog(string.Format("Prefab GenerateName {0} Not Exists", prefabName));
    }

    void GeneratePrefab(PrefabGenerateInfo prefabInfo)
    {
        GameObject obj = null;
        if (prefabInfo.ChildToHitObjects)
        {
            obj = GameObject.Instantiate(prefabInfo.Prefab, HitObjectsRoot.transform, prefabInfo.ChildToHitObjects);
        }
        else
        {
            obj = GameObject.Instantiate(prefabInfo.Prefab);
        }

        if (obj)
        {
            if(prefabInfo.ChildToHitObjects)
            {
                obj.transform.localPosition = new Vector2(prefabInfo.Offset.x, prefabInfo.Offset.y);
            }
            else
            {
                obj.transform.position = new Vector2(transform.position.x + (transform.localScale.x * prefabInfo.Offset.x), transform.position.y + prefabInfo.Offset.y);
            }
            
            var objectHitBox = obj.GetComponentInChildren<HitBox>();
            if(objectHitBox)
            {
                objectHitBox.SendMessage("SetOwner", RootObject);
                objectHitBox.AirborneVector = new Vector2(objectHitBox.AirborneVector.x * transform.localScale.x, objectHitBox.AirborneVector.y);
            }
            obj.transform.localScale = Vector3.one;

            var objectProjectileController = obj.GetComponent<Projectile>();
            if(objectProjectileController)
            {
                var direction = transform.localScale.x > 0 ? Direction.Right : Direction.Left;
                objectProjectileController.SetDirection(direction);
            }

            var objectZcontroller = obj.GetComponentInChildren<ZController>();
            if (objectZcontroller)
            {
                objectZcontroller.Z = prefabInfo.Offset.z;
            }
        }
    }

    void DestroyHitObjects()
    {
        for (int i = HitObjectsRoot.transform.childCount; i-- > 0;)
        {
            Destroy(HitObjectsRoot.transform.GetChild(i).gameObject);
        }
    }
}

[System.Serializable]
public struct PrefabGenerateInfo
{
    public string Name;

    public GameObject Prefab;

    public bool ChildToHitObjects;

    public Vector3 Offset;
}