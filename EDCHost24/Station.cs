<<<<<<< Updated upstream
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EDCHOST24
{
    class Station //所有站点
    {
        private int MAX_STATION = 3;
        private List<Dot> mStationList; //一个包含站点位置信息的list
        public void Reset() 
        { 
            mStantionList.Clear();
        } //num复位
        public Station() //构造函数
        { 
            list<Dot> mStationList = new list<Dot>();
        }
        public void Add(Dot _inPos)
        {
            if (mStationList.Count() < MAX_STATION)
            {
                if (_isNewStationLegal(_inPos))
                {
                    mStationList.Add(_inPos);
                    Debug.WriteLine("New Station, ({0}, {1})", _inPos.x, _inPos.y);
                }
                else
                {
                    Debug.WriteLine("Failed! Location is illegal");
                }
            }
            else
            {
                Debug.WriteLine("Failed! Up to maximum");
            }
        }

        public Dot Index(int i)
        {
            if (i <mStationList.Count())
            {
                return mStationList[i];
            }

            Dot temp = new Dot (-1, -1);
            return  temp;
        }

        private bool _isNewStationLegal(Dot _inPos)
        {

        }
    }

}
=======
﻿using System;
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
        private List<Dot> mStationList; //一个包含站点位置信息的list
        public void Reset() 
        { 
            mStantionList.Clear();
        }
        public Station() //构造函数
        { 
            List<Dot> mStationList = new List<Dot>();
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
>>>>>>> Stashed changes
