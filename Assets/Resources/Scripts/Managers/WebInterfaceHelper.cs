using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WebInterfaceHelper
{

    [DllImport("__Internal")]
    static extern void setCookie(string name, string data);

    [DllImport("__Internal")]
    static extern string getCookie(string name);
    

    static void SetCookie(string name, string data)
    {
        setCookie(name, data);
    } 

    static string GetCookie(string name)
    {
        return getCookie(name);
    }

}
