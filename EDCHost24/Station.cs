using System;
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
            List<Dot> mStationList = new List<Dot>();
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
