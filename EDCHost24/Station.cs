using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EDCHOST24
{
    public class Station //所有站点
    {
        private const int MAX_STATION = 3;
        private list<Dot> mStationList; //一个包含站点位置信息的list
        public void Reset() 
        { 
            mStantionList.Clear();
        }
        public Station() //构造函数
        { 
            list<Dot> mStationList = new list<Dot>();
        }

        public void AddStation (Dot _new_station)
        {
            if (mStationList.Count() < MAX_STATION)
            {
                if (_isPosLegal(_new_station))
                {
                    mStationList.Add(_new_station);
                    Debug.WriteLine("New station has been set, ({0}, {1})", _new_station.x, _new_station.y);
                }
                else
                {
                    Debug.WriteLine("Failed! The postion of new station is illegal");
                }
            }
            else
            {
                Debug.WriteLine("Failed! The number of charging stations has reached the upper limit");
            }

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

        public int Count()
        {
            return mStationList.Count();
        }

        private static bool _isPosLegal (Dot _inDot)
        {

        }
    }

}
