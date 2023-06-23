using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemActionCtrl : MonoBehaviour
{
    [SerializeField]
    private float PowerForaddForceXZ;
    [SerializeField]
    private float PowerForaddForceY;
    [SerializeField]
    private float lifeTime;
    [SerializeField]
    private GameObject particle;
    private float currTime;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 randomVec = Random.insideUnitCircle;
        GetComponent<Rigidbody>().AddForce(new Vector3(randomVec.x, 0f, randomVec.y) * PowerForaddForceXZ);
        GetComponent<Rigidbody>().AddForce(Vector3.up * PowerForaddForceY);
        currTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        if (currTime > lifeTime)
        {
            Instantiate(particle, transform.position, Quaternion.identity);
            GameObject.FindObjectOfType<PlayerData>().AddGold();
            Destroy(gameObject);
        }
    }
}
