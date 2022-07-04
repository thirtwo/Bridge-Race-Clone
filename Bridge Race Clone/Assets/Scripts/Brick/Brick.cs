using System.Collections;
using UnityEngine;
public class Brick : MonoBehaviour
{
    public enum BrickColor
    {
        blue,
        red,
        green,
        purple
    }
    public BrickColor color;
    private readonly GameObject[] trails = new GameObject[2];
    private bool isCollected = false;
    public bool IsCollected => isCollected;
    private void Awake()
    {
        trails[0] = transform.GetChild(0).gameObject;
        trails[1] = transform.GetChild(1).gameObject;
    }

    public void OnCollect()
    {
        isCollected = true;
        StartCoroutine(Co_Oncollect());
    }
    private IEnumerator Co_Oncollect()
    {
        trails[0].SetActive(true);
        trails[1].SetActive(true);
        yield return new WaitForSeconds(0.7f);
        trails[1].SetActive(false);
        trails[0].SetActive(false);
    }

    public void Explode(Vector3 pos)
    {
        transform.SetParent(null);
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddExplosionForce(Random.Range(5f, 15f), pos, Random.Range(5f, 15f),1,ForceMode.Impulse);
    }
}
