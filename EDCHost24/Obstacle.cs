using System;
using System.Math;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Security.Cryptography;
using System.Diagnostics;
namespace EDCHOST24
{
    /***********************************************
    Obstacle类
    参数：
    RADIUS: 小车的碰撞半径
    ***********************************************/
    public class Obstacle
    {
        public Labyrinth mLabyrinth;
        public Station mStation;
        public readonly int RADIUS; // 碰撞的半径（这里语法存疑）
        // 构造函数
        public Obstacle(Labyrinth labyrinth, Station station, const int r = 8)
        {
            RADIUS = r; 
            mLabyrinth = labyrinth;
            mStation = station;
        }


        // 返回true表示发生碰撞
        public bool isCollidedWall(Dot CarPos, int r = -1)
        {
            if (r < 0)
            {
                r = RADIUS;
            }
            foreach(Wall wall in mLabyrinth.mpWallList)
            {
                if (DistanceL(wall, CarPos) < r)
                {
                    return true;
                }
            }
            return false;
        }
        
        // 返回true表示发生碰撞
        public bool isCollidedStation(Dot CarPos, int r = -1)
        {
            if (r < 0)
            {
                r = RADIUS;
            }
            foreach (Dot point in mStationList)
            {
                if (DistanceP(point, CarPos) < r)
                {
                    return true;
                }
            }
        }

        // 这个函数返回一个点dot是否在障碍的某个距离范围r内
        // 考虑到，如果package和obstacle擦边放也不太好，所以设置参数r
        // 返回true表示在障碍的r范围内
        public bool isObstacle(Dot dot, int r = 0)
        {
            return isCollidedStation(dot, r) || isCollidedWall(dot, r);
        }

        // 私有函数，计算两点的距离
        private int DistanceP(Dot p1, Dot p2)
        {
            int d1 = Abs(p1.x - p2.x);
            int d2 = Abs(p1.y - p2.y);
            return (int)Sqrt(d1 * d1 + d2 * d2);
        }

        //私有函数，用于计算小车和障碍线的距离
        private int DistanceL(Wall wall, Dot CarPos)
        {
            int sameP, bigP, smallP, sameC, diffC; 
            // 相同的坐标，不同坐标中较大的，不同坐标中较小的,
            // 相同坐标对应的小车坐标，不同的坐标对应小车坐标
            if (wall.w1.y == wall.w2.y)
            {
                sameP = wall.w1.y;
                sameC = CarPos.y;
                diffC = CarPos.x;
                if (wall.w1.x > wall.w2.x)
                {
                    bigP = wall.w1.x;
                    samllP = wall.w2.x;
                }
                else
                {
                    bigP = wall.w1.x;
                    smallP = wall.w2.x;
                }
            }
            else
            {
                sameP = wall.w1.x;
                sameC = CarPos.x;
                diffC = CarPos.y;
                if (wall.w1.y > wall.w2.y)
                {
                    bigP = wall.w1.y;
                    samllP = wall.w2.y;
                }
                else
                {
                    bigP = wall.w1.y;
                    smallP = wall.w2.y;
                }
            }
            
            //如果小车在两个障碍点之间，计算垂直距离
            if (smallP <= diffC && diffC <= bigP)
            {
                return Abs(sameP - sameC);
            }
            // 否则计算两点距离
            else
            {
                int d1 = Min(Abs(smallP - diffC), Abs(bigP - diffC));
                int d2 = Abs(sameP - sameC);
                return Sqrt(d1 ** d1 + d2 ** d2);
            }
        }

    }
}