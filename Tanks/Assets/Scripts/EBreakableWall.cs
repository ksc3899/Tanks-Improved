using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBreakableWall : MonoBehaviour
{
    public Vector3 refPosition;
    public int rows = 2;
    public int columns = 5;
    public LayerMask destructibleBlocks;
    public float strikeForce = 1000f;

    private List<Vector3> wallBlocks = new List<Vector3>();

    private void InitiateList()
    {
        wallBlocks.Clear();

        float xRef = refPosition.x;
        float yRef = refPosition.y;
        float zRef = refPosition.z;

        for(int z = 0; z < columns; z++)
        {
            for(int y = 0; y < rows; y++)
            {
                wallBlocks.Add(new Vector3(xRef, yRef + y, zRef - z));
            }
        }
    }

    public void WallDestruction(Vector3 strikePoint, Vector3 shellDirection)
    {
        Collider[] blocks = Physics.OverlapSphere(strikePoint, 1f, destructibleBlocks);

        foreach (Collider c in blocks)
        {
            int random = Random.Range(0, 100);
            if (random % 2 == 0)
            {
                c.gameObject.AddComponent<Rigidbody>();
                Rigidbody rb = c.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(strikeForce * shellDirection, ForceMode.Impulse);
                DestroWallBlock(c.gameObject);
            }
        }

        Destroy(this.gameObject, 15f);
    }

    private void DestroWallBlock(GameObject toDestroy)
    {
        Destroy(toDestroy, 4f);
    }

    private void Start()
    {
        InitiateList();
    }

}
