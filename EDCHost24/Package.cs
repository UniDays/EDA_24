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

        private int mIsPicked;    //whther it has been deliveried

        // features added by EDA Host 24
        // author : Alfred Dai
        // all times are in ms
        private int mGenerationTime;
        private int mScheduledDeliveryTime;
        private int mActualDeliveryTime;


        public Package(Dot inDeparturePos, Dot inDestinationPos, int inGenerationTime)
        {
            mDeparture = inDeparturePos;
            mDestination = inDestinationPos;
            mGenerationTime = inGenerationTime;
            mScheduledDeliveryTime = 20 * Distance(mDeparture, mDestination) + 1000;
            IsPicked = 0;
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

        public int GetPackageScore(int ArrivalTime)
        {

            if (ArrivalTime > mScheduledDeliveryTime)
            {
                return (mScheduledDeliveryTime - ArrivalTime) * LATE_PENALTY;
            }
            else
            {
                return ARRIVE_CREDIT;
            }
        } 

        public void PickPackage()
        {
            IsPicked = 1;
        }

        public bool IsPicked()
        {
            return mIsPicked;
        }
    }
}
