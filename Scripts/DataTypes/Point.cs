

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edu.Vfs.RoboRapture.DataTypes
{
    ///<summary>
    /// Represents the position of an object on a grid.
    ///</summary>
    [System.Serializable]
    public struct Point
    {
        public int x;
        public int y;
        public int z;

        public Point(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return string.Format("(x:{0} y:{1} z:{2})", x, y, z);
        }

        public static bool operator ==(Point a, Point b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.x - p2.x, p1.y - p2.y, p1.z - p2.z);
        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
            {
                Point p = (Point)obj;
                return x == p.x && y == p.y && z == p.z;
            }
            return false;
        }

        public bool Equals(Point p)
        {
            return x == p.x && y == p.y && z == p.z;
        }

        public override int GetHashCode()
        {
            return x ^ y ^ z;
        }
    }
}

