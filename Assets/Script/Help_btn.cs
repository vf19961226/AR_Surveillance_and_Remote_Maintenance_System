using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Diagnostics;
using System.Threading;

public class Help_btn : MonoBehaviour
{
    public string Exe_path;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void help_btn()
    {
        Thread help = new Thread(new ThreadStart(thread_help));
        help.Start();
    }

    private void thread_help()
    {
        Process p = new Process();
        p.StartInfo.FileName = Exe_path;
        p.Start();
    }
}
