using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EDCHOST24
{
    //说明（ybj 8/27)
    //1、目前尚未知场上允许有多少个外卖
    //2、可能需要设置 不能在对方充电站等地方 设定外卖起始/终点，在checkcoincide里再添加操作
    public class PackageGenerator 
    {
        private Package[] mpPackageList;
        private int PKG_NUM;
        public PackageGenerator(int AMOUNT) //生成指定数量的外卖
        {
            PKG_NUM = AMOUNT;
            mpPackageList = new Package[PKG_NUM];
            int StartX, StartY;
            int EndX, EndY;
            Dot startdots,enddots;
            Random NRand = new Random();
            for (int i = 0; i < PKG_NUM; ++i)
            {
                //需要坐标格式
                StartX = NRand.Next(Game.MAZE_CROSS_NUM);
                StartY = NRand.Next(Game.MAZE_CROSS_NUM);
                EndX = NRand.Next(Game.MAZE_CROSS_NUM);
                EndY = NRand.Next(Game.MAZE_CROSS_NUM);

                startdots = CrossNo2Dot(StartX, StartY);
                enddots = CrossNo2Dot(EndX, EndY);

                if (checkcoincide(startdots) || checkcoincide(enddots))
                {
                    i--;
                    continue;
                }

                mpPackageList[i] = new Package(startdots,enddots);
            }
        }

        //从格点转化为int，传入坐标，返回Dot
        public static Dot CrossNo2Dot(int CrossNoX, int CrossNoY)
        {
            int x = Game.MAZE_SHORT_BORDER_CM + Game.MAZE_SIDE_BORDER_CM + Game.MAZE_CROSS_DIST_CM * CrossNoX;
            int y = Game.MAZE_SHORT_BORDER_CM + Game.MAZE_SIDE_BORDER_CM + Game.MAZE_CROSS_DIST_CM * CrossNoY;
            Dot temp = new Dot(x, y);
            return temp;
        }

        //返回下标为i的PackageDotArray中的点。开发者：xhl
        public Package GetPackage(int i)
        {
            return mpPackageList[i];
        }

        //检验坐标是否与各种其他坐标重复
        public bool checkcoincide(Dot adot)
        {
            bool same = false;
            for(int i=0;i<PKG_NUM;++i)
            {
                if(mpPackageList[i].m_StartPos==adot|| mpPackageList[i].m_EndPos==adot)
                {
                    same = true;
                }
            }
            return same;
        }

    }
}
