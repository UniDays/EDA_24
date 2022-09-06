using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EDCHOST24
{
    //队名
    public enum Camp
    {
        NONE = 0, A, B
    };

    // STL: Store a series of position of car
    public class MyQueue <T>
    {
        private int MAX_LONGTH;

        private List<T> mItem;

        public PosQueue(int _MaxLongth)
        {
            if (_MaxLongth < 1)
            {
                throw new Exception("MaxLongth is expected to be larger than 0");
            }
            MAX_LONGTH = _MaxLongth;
            mItem = new List<T>(); 
            mPointer = 0;
        }

        public void Enqueue (T _item)
        {
            if (mItem.Count < MAX_LONGTH)
            {
                mItem.Add(_item);
            }
            else if (mItem.Count == MAX_LONGTH)
            {
                mItem.RemoveAt(0);
                mItem.Add(_item);
            }
        }

        public T Item (int _index)
        {
            if (_index < mItem.Count && _index >= 0)
            {
                return mItem[_index];
            }
            else if (_index <= -1 && _index >= -mItem.Count)
            {
                return mItem[mItem.Count + _index];
            }
            else
            {
                throw new Exception("PosQueue: index is out of range");
            }
        }

        public int Count()
        {
            return mItem.Count;
        }
    }





    public class Car //选手的车
    {

        // the object of package and picked by car and first collision time
        private class PackagesAndTime
        {
            public Package mPkg;
            public int mFirstCollisionTime;

            public PackageAndPickedTime(Package _pkg, int _FirstCollisionTime = -1)
            {
                mPkg = _pkg;
                mFirstCollisionTime = _FirstCollisionTime;
            }
        }

        public const int RUN_CREDIT = 10;          //小车启动可以得到10分;
        public const int PICK_CREDIT = 5;          //接到一笔订单得5分;
        public const int ARRIVE_EASY_CREDIT = 20;  //规定时间内送达外卖可以得到20/25/30分;
        public const int ARRIVE_NORMAL_CREDIT = 25;
        public const int ARRIVE_HARD_CREDIT = 30;
        public const int CHARGE_CREDIT = 5;        // credit for set a charge station
        public const int LATE_PENALTY = 5;         //外卖超时1秒惩罚5分;
        public const int IGNORE_PENALTY = 5;       //未接单惩罚5分;
        public const int ON_BLACk_LINE_PENALTY = 10;    
        public const int IN_OPPONENT_STATION_PENALTY = 10; // in cm per frame
        public const int IN_OBSTACLE_PENALTY = 10; // in cm per frame
        public const int MARK_PENALTY = 50;        
        public const int MAX_PKG_COUNT = 5;        
        public const int ENERGY_EXHAUSTION_PENALTY = 50; // 50 ms per cm
        


        public const int COLLISION_RADIUS = 8;
        public const int COLLISION_DETECTION_TIME = 1000; // in ms
        public const int MAX_MILEAGE = 2000; // in cm


        public MyQueue<Dot> mQueuePos;

        public MyQueue<int> mFlagIsInChargeStation;

        public Camp MyCamp;               //A or B get、set直接两个封装好的函数
        public int mScore;               //得分
        
        private int mMileage;              //小车续航里程

        // Flags of Location

        // Locations where car would get penalty
        private int mIsOnBlackLine;   
        private int mIsInOpponentChargeStation;
        private int mIsInObstacle;       
        
        private int mIsInChargeStation;           //小车目前是否在充电区域内 0不在充电区内 1在充电区内

        private int mGameTime;


        public List<PackagesAndTime> mPickedPackages;


        /********************************************
        Interface
        *********************************************/
        public Car(Camp c)
        {
            
        }


        public int Update(Dot _CarPos, int _GameTime, int _IsOnBlackLine, 
            int _IsInObstacle, int _IsInOpponentStation, int _IsInChargeStation, 
            ref List<Package> _PackagesRemain)
        {
            mGameTime = _GameTime;

            UpdatePos(_CarPos);
        }

        public int GetScore ()
        {
            return mScore;
        }

        /********************************************
        Private Functions
        *********************************************/

        private void PickPackage(Dot _CarPos, ref List<Package> _PackagesRemain)      //拾取外卖
        {
            foreach (var pkg in _PackagesRemain)
            {
                if (pkg.Distance2Departure(_CarPos) <= COLLISION_RADIUS && 
                    mPickedPackages.Count <= MAX_PKG_COUNT)
                {
                    mPickedPackages.Add(new PackagesAndTime (pkg));
                    _PackagesRemain.RemoveAt(pkg);
                    mScore += PICK_CREDIT;
                }
            }
        }

        private void DropPackage(Dot _CarPos)      //送达外卖 
        {
            foreach (var PkgAndTime in mPickedPackages)
            {
                if (PkgAndTime.mPkg.Distance2Destination(_CarPos) <= COLLISION_RADIUS)
                {
                    if (PkgAndTime.mFirstCollisionTime != -1 && 
                        mGameTime - PkgAndTime.mFirstCollisionTime > COLLISION_DETECTION_TIME)
                    {
                        mPickedPackages.RemoveAt(PkgAndTime);
                        mScore += PkgAndTime.mPkg.GetPackageScore(mGameTime);
                    }
                    else if (PkgAndTime.mFirstCollisionTime == -1)
                    {
                        PkgAndTime.mFirstCollisionTime = mGameTime;
                    }
                }
                else if (PkgAndTime.mFirstCollisionTime != -1)
                {
                    PkgAndTime.mFirstCollisionTime = -1;
                }
            }
        }

        private void UpdatePos (Dot _CarPos)
        {
            mQueuePos.Enqueue(_CarPos);
        }

        private void UpdateMileage (out int _Time_Penalty)
        {
            int DeltaDistance = Dot.Distance(PosQueue[0], PosQueue[-1]);
            mMileage -= DeltaDistance;
            if (mMileage < 0)
            {
                _Time_Penalty = DeltaDistance * ENERGY_EXHAUSTION_PENALTY;
            }
        }

        private void Charge (int IsInChargeStation)
        {
            mFlagIsInChargeStation.Enqueue(IsInChargeStation);
            for (int i = 0; i < mFlagIsInChargeStation.Count();i++)
            {
                if (!mFlagIsInChargeStation)
                {
                    return;
                }
            }

            mMileage = MAX_MILEAGE;
        }

        private void SetChargeStation (int HasSetStation)
        {
            if (HasSetStation)
            {
                mScore += CHARGE_CREDIT;
            }
        }

        private void OnBlackLinePenaly (int IsOnBlackLine)
        {
            if (IsOnBlackLine && !mIsOnBlackLine)
            {
                mIsOnBlackLine = IsOnBlackLine;
            } 
            else if (!IsOnBlackLine && mIsOnBlackLine)
            {
                mIsOnBlackLine = IsOnBlackLine;
                mScore -= ON_BLACk_LINE_PENALTY;
            }
        }

        private void InOpponentStationPenalty (int IsInOpponentStation)
        {
            if (IsInOpponentStation)
            {
                mMileage -= IN_OPPONENT_STATION_PENALTY;
            }
        }

        private void InObstacle (in IsInObstacle)
        {
            if (mIsInObstacle)
            {
                mMileage -= IN_OBSTACLE_PENALTY;
            }
        }

        private void GetMark(int HasGetMark)
        {
            mScore -= MARK_PENALTY;
        }
    }
}