using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;




public class Client_HTTP_Talker : MonoBehaviour
{
    WWWForm form;
    // Use this for initialization
    void Start()
    {
        form = new WWWForm();
       
    }

    void Update()
    {
        /*
        if (Input.GetButton("Jump"))
        {
            form = new WWWForm();
            //form.AddField("frameCount", Time.frameCount.ToString());
            Debug.Log("added");
            Play_Object play = new Play_Object("xyz", "123123");
            

            
            form.AddField("Play", JsonConvert.SerializeObject(play));
            WWW www = new WWW("192.168.43.184:8000/test/", form);

            while (!www.isDone)
            {
                Debug.Log("progress: " + www.progress);
            }

            Debug.Log("passei");
        }
         * */
    }


}
