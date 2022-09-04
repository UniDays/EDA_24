using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDCHOST24
{
    //Package
    public class Package
    {
        private Dot mDeparture;     // Departure of Package
        private Dot mDestination;   // Destination of Package

        private int mIsPicked;    //whther it has been deliveried or arrived //0-unpicked; 1-picked; 2-arrived;

        // features added by EDA Host 24
        // author : Alfred Dai
        // all times are in ms
        private int mGenerationTime;
        private int mScheduledDeliveryTime;
        private int mActualDeliveryTime;
        private int mPackageLevel;  //judge the package is easy/normal/hard to be arrived //0-easy; 1-normal; 2-hard;
        private int mScheduledScore;


        public Package(Dot inDeparturePos, Dot inDestinationPos, int inGenerationTime)
        {
            mDeparture = inDeparturePos;
            mDestination = inDestinationPos;
            mGenerationTime = inGenerationTime;
            mScheduledDeliveryTime = 20 * Distance(mDeparture, mDestination) + 1000;
            IsPicked = 0;
            //judge level
            if(mDeparture.x >= 40 && mDeparture.x <= 214 && mDeparture.y >= 40 && mDeparture.y <= 214 
                && mDestination.x >= 40 && mDestination.x <= 214 && mDestination.y >= 40 && mDestination.y <= 214)
            {
                if(Distance(mDeparture, mDestination) <= 120)
                {
                    mPackageLevel = 0;
                }
                else
                {
                    mPackageLevel = 1;
                }
            }
            else
            {
                mPackageLevel = 2;
            }
            //judge score
            switch(mPackageLevel)
                {
                case 0: mScheduledScore = ARRIVE_EASY_CREDIT; break;
                case 1: mScheduledScore = ARRIVE_NORMAL_CREDIT; break;
                case 2: mScheduledScore = ARRIVE_HARD_CREDIT; break;
                }
        }

        public Dot GetDeparture()
        {
            return mDeparture;
        }

        public Dot GetDestination()
        {
            return mDestination;
        }

        public Dot GetGenerationTime()
        {
            return mGenerationTime;
        }

        public int Distance2Departure (Dot CarPos)
        {
            return Distance(CarPos, mDeparture);
        }

        public int Distance2Destination (Dot CarPos)
        {
            return Distance(CarPos, mDestination);
        }
        public int GetPackageScore()
        {
            int PackageScore = 0;
            int Penalty = 0;
            if(IsPicked == 2)
            {
                PackageScore = mScheduledScore;
            }
            if (mActualDeliveryTime > mScheduledDeliveryTime)
            {
                if((mScheduledDeliveryTime - mActualDeliveryTime) * LATE_PENALTY <= mScheduledScore)
                {
                    Penalty = (mScheduledDeliveryTime - mActualDeliveryTime) * LATE_PENALTY;
                }
                else
                {
                    Penalty = mScheduledScore;
                }
            }
            PackageScore -= Penalty;
            return PackageScore;
        } 

        public void PickPackage()
        {
            IsPicked = 1;
        }
        public void DropPackage()
        {
            IsPicked = 2;
        }
        public bool IsPicked()
        {
            return mIsPicked;
        }
        public void UpdateArrivalTime(int ArrivalTime)
        {
            mActualDeliveryTime = ArrivalTime;
        }

        public int GenerationTime ()
        {
            return mGenerationTime;
        }

        public int ScheduledDeliveryTime ()
        {
            return mScheduledDeliveryTime;
        }
    }
}
