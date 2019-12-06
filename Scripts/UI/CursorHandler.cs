

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.Controllers;
using Edu.Vfs.RoboRapture.Units.Actions;
using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    [SerializeField]
    private Texture2D ActionIcon;
    [SerializeField]
    private Texture2D MovementIcon;

    private void Start()
    {
        PlayerController.PlayerActionSelected += ChangeCursor;
        PlayerController.PlayerActionExecuted += ResetCursor;
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(null, new Vector2(0,0) ,CursorMode.Auto);
    }

    public void ChangeCursor(ActionType type)
    {
        switch (type)
        {
            case ActionType.Action:
                Cursor.SetCursor(ActionIcon, new Vector2(0,0) ,CursorMode.Auto);
                break;
            case ActionType.Movement:
                Cursor.SetCursor(MovementIcon, new Vector2(0,0) ,CursorMode.Auto);
                break;
            default:
                ResetCursor();
                break;
        }
    }
}
