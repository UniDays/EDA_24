using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDCHOST22
{
    public class Station //所有站点
    {
        public int num;//站点数量
        public list<Dot> mStationList; //一个包含站点位置信息的list
        public void ResetIndex() 
        { 
            num = 0; 
            mStantionList.Clear();
        } //num复位
        public Station(int Num) //构造函数
        { 
            num = Num;
            list<Dot> mStationList = new list<Dot>();
        }

        public static bool isCollided(Dot CarPos, int radius)
        {
            foreach(Dot dot in mStationList)
            {
                if (Utility.DistanceP(CarPos, dot) < radius)
                {
                    return true;
                }
            }
            return false;
        }
    }

}
