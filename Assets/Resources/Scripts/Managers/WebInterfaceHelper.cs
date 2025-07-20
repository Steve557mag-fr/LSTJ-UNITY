using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WebInterfaceHelper
{

    [DllImport("__Internal")]
    static extern void setCookie(string name, string data);

    [DllImport("__Internal")]
    static extern string getCookie(string name);
    

    internal static void SetCookie(string name, string data)
    {
        setCookie(name, data);
        Debug.Log("[WEBI]: cookie set!");
    } 

    internal static string GetCookie(string name)
    {
        Debug.Log("[WEBI]: cookie get!");
        return getCookie(name);
    }

}
