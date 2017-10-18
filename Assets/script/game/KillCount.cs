
using UnityEngine;
using UnityEngine.UI;

public class KillCount : MonoBehaviour
{

    // Use this for initialization
    private int count;
    private Text textScript;
    // Use this for initialization
    void Start()
    {
        textScript = this.GetComponent<Text>();
        _setText();
    }
    public void AddCount(int num)
    {
        count += num;
    }
    public void CountInit()
    {
        count = 0;
    }
    // Update is called once per frame
    void Update()
    {
        _setText();
    }
    void _setText()
    {
        textScript.text = count.ToString();
    }
}
