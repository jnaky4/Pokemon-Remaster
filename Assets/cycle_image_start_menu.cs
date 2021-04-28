using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cycle_image_start_menu : MonoBehaviour
{
    private RectTransform rt;
    private Vector2 open;
    private Vector2 closed;
    private bool IsOpen;
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(start());
    }

    // Update is called once per frame
    void Update()
    {
 
    }
    IEnumerator start()
    {
        rt = GetComponent<RectTransform>();
        open = new Vector2(-5.0f, -5.0f);
        closed = new Vector2(-82.0f, -5.0f);
        IsOpen = false;

        while (true)
        {
            yield return StartCoroutine(wait_time(2));
            cycle_image();
        }

    }
    IEnumerator wait_time(int time)
    {
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(time);
    }
    public void cycle_image()
    {
        int move = 10;
        float x = rt.anchoredPosition.x;
        float y = rt.anchoredPosition.y;
        /*        yield return new WaitForSeconds(5);*/
        while (move > 0)
            x -= 10;
        rt.anchoredPosition = new Vector2(x, y);

    }
    
}
