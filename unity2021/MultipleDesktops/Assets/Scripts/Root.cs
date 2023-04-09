
using UnityEngine;

/// <summary>
/// 根程序类
/// </summary>
/// <remarks>
/// 不参与模块编译，仅用于在编辑器中开发调试
/// </remarks>
public class Root : RootBase
{
    private void Awake()
    {
        doAwake();
    }

    private void Start()
    {
        entry_.__DebugPreload(exportRoot);
    }

    private void OnDestroy()
    {
        doDestroy();
    }

    private void OnGUI()
    {
        /*
        if (GUI.Button(new Rect(0, 0, 60, 30), "Create"))
        {
            entry_.__DebugCreate("test", "default", "", "");
        }

        if (GUI.Button(new Rect(0, 30, 60, 30), "Open"))
        {
            entry_.__DebugOpen("test", "file", "", 0.5f);
        }

        if (GUI.Button(new Rect(0, 60, 60, 30), "Show"))
        {
            entry_.__DebugShow("test", 0.5f);
        }

        if (GUI.Button(new Rect(0, 90, 60, 30), "Hide"))
        {
            entry_.__DebugHide("test", 0.5f);
        }

        if (GUI.Button(new Rect(0, 120, 60, 30), "Close"))
        {
            entry_.__DebugClose("test", 0.5f);
        }

        if (GUI.Button(new Rect(0, 150, 60, 30), "Delete"))
        {
            entry_.__DebugDelete("test");
        }
        */
        if (GUI.Button(new Rect(0, 0, 120, 30), "Center -> Right"))
        {
            entry_.__DebugSwitch("center", "push_center_to_left");
            entry_.__DebugSwitch("right", "pull_center_from_right");
        }
        if (GUI.Button(new Rect(0, 30, 120, 30), "Right -> Center"))
        {
            entry_.__DebugSwitch("center", "pull_center_from_left");
            entry_.__DebugSwitch("right", "push_center_to_right");
        }
        if (GUI.Button(new Rect(0, 60, 120, 30), "Center -> Left"))
        {
            entry_.__DebugSwitch("center", "push_center_to_right");
            entry_.__DebugSwitch("left", "pull_center_from_left");
        }
        if (GUI.Button(new Rect(0, 90, 120, 30), "Left -> Center"))
        {
            entry_.__DebugSwitch("center", "pull_center_from_right");
            entry_.__DebugSwitch("left", "push_center_to_left");
        }

        if (GUI.Button(new Rect(0, 120, 120, 30), "Center -> Top"))
        {
            entry_.__DebugSwitch("center", "push_center_to_bottom");
            entry_.__DebugSwitch("top", "pull_center_from_top");
        }
        if (GUI.Button(new Rect(0, 150, 120, 30), "Top -> Center"))
        {
            entry_.__DebugSwitch("center", "pull_center_from_bottom");
            entry_.__DebugSwitch("top", "push_center_to_top");
        }

        if (GUI.Button(new Rect(0, 180, 120, 30), "Center -> Bottom"))
        {
            entry_.__DebugSwitch("center", "push_center_to_top");
            entry_.__DebugSwitch("bottom", "pull_center_from_bottom");
        }
        if (GUI.Button(new Rect(0, 210, 120, 30), "Bottom -> Center"))
        {
            entry_.__DebugSwitch("center", "pull_center_from_top");
            entry_.__DebugSwitch("bottom", "push_center_to_bottom");
        }
    }
}

