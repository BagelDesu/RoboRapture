//-----------------------------------------------------------------------
// <copyright file="Behaviour.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Enemies
{
    using System.Collections;
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;
    using UnityEngine;

    public abstract class Behaviour : MonoBehaviour
    {
        public abstract IEnumerator Execute(BoardController boardController, List<Point> board);   
    }
}