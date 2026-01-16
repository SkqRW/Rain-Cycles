using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using DevInterface;
using Plugin;
using UnityEngine;
using System.Linq;

namespace FilesSetting;

public class RCDEVTools
{
    public static void Terminate()
    {
        On.DevInterface.Page.ctor -= DevInterface_Page_ctor;
    } 
    public static void Init()
    {
        On.DevInterface.Page.ctor += DevInterface_Page_ctor;
    }

    public static void DevInterface_Page_ctor(On.DevInterface.Page.orig_ctor orig, Page self, DevUI owner, string IDstring, DevUINode parentNode, string name)
    {
        orig(self, owner, IDstring, parentNode, name);

        if(owner != null){            
            //self.subNodes.Add(new Button(owner, "RainCycles", self, new Vector2(790, 680f), 220f, "Rain Cycles"));
            self.subNodes.Add(new RCPanel(owner, "RC_Panel", self, new Vector2(790, 580f), new Vector2(200f, 100f), "Rain Cycles"));
        }
    }
}
