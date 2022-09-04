using System;
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
    // Token
    public enum GameState { UNSTART = 0, RUN = 1, PAUSE = 2, END = 3 };
    public enum GameStage { FIRST_HALF = 0,  SENCOND_HALF= 1, END = 2};

    public class Game
    {
        // size of competition area
        public const int MAX_SIZE = 254;
        public const int AVAILIABLE_MAX_X = 222;
        public const int AVAILIABLE_MIN_X = 32;
        public const int AVAILIABLE_MAX_Y = 222;
        public const int AVAILIABLE_MIN_Y = 32;

        // size of car
        public const int COLLISION_RADIUS = 8;

        // initial amount of package
        public const int INITIAL_PKG_NUM = 5;
        // time interval of packages
        public const int TIME_INTERVAL = 1500;

        // time of first and second half
        public const int FIRST_HALF_TIME = 60000;
        public const int SECOND_HALF_TIME = 180000;

        // state
        public GameStage mGameStage;
        public GameState mGameState;

        // Time
        // Set time zero as the start time of each half
        public int mStartTime; // system time, update for each half
        public int mGameTime;

        // car and package
        public Car mCar_1, mCar_2;
        private PackageList mPackagesFirst;
        private PackageList mPackagesSecond;

        public List<Package> mPackagesRemain;

        // which team is racing A/B
        public Camp mCamp;

        // obstacle
        public Obstacle mObstacle;

        // Record the time when the trolley enters the area
        public Dot mLastPos;

        public int mLastWrongAreaTime;
        public int mLastOnObstacleTime;
        public int mLastOnStationTime;

        


        public Game()
        {
            Debug.WriteLine("Call Constructor of Game");

            mGameState = GameState.UNSTART;
            mGameStage = GameStage.FIRST_HALF;

            mCamp = Camp.A;

            CarA = new Car(Camp.A, 0);
            CarB = new Car(Camp.B, 0);

            mPackagesFirst = new PackageList(AVAILIABLE_MAX_X, AVAILIABLE_MIN_X, 
                        AVAILIABLE_MAX_Y, AVAILIABLE_MIN_Y, INITIAL_PKG_NUM, FIRST_HALF_TIME, TIME_INTERVAL);

            mPackagesSecond = new PackageList(AVAILIABLE_MAX_X, AVAILIABLE_MIN_X, 
                        AVAILIABLE_MAX_Y, AVAILIABLE_MIN_Y, INITIAL_PKG_NUM, FIRST_HALF_TIME, TIME_INTERVAL);

            mPackagesRemain = new List<Package> ();

            mStartTime = GetCurrentTime();
            mGameTime = 0;

            mObstacle = new Obstacle();

            mLastPos = Dot(-1, -1);

            mLastWrongAreaTime = -1;
            mLastOnObstacleTime = -1;
            mLastOnStationTime = -1;
        }


        /***********************************************
        Time
        ***********************************************/
        public void UpdateGameTime ()
        {
            mGameTime = GetCurrentTime() - mStartTime;
        }

        private static int GetCurrentTime()
        {
            System.DateTime currentTime = System.DateTime.Now;
            int time = currentTime.Hour * 3600000 + currentTime.Minute * 60000 + currentTime.Second * 1000;
            //Debug.WriteLine("H, M, S: {0}, {1}, {2}", currentTime.Hour, currentTime.Minute, currentTime.Second);
            //Debug.WriteLine("GetCurrentTime，Time = {0}", time); 
            return time;
        }


        /***********************************************
        Initialize and Generate Package
        ***********************************************/
        public bool GeneratePackage ()
        {
            UpdateGameTime();

            if (mGameStage == GameStage.FIRST_HALF && 
                mGameTime >= mPackagesFirst.NextGenerationTime)
            {
                mPackagesRemain.Add(mPackagesFirst.GeneratePackage);
                return true;
            }
            else if (mGameStage == GameStage.SENCOND_HALF &&
                mGameTime >= mPackagesSecond.NextGenerationTime)
            {
                mPackagesRemain.Add(mPackagesSecond.GeneratePackage);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool InitialPackages()
        {
            if (mGameStage == GameStage.FIRST_HALF)
            {
                for (int i = 0;i < mPackagesFirst.Amount;i++)
                {
                    mPackagesRemain.Add(mPackagesFirst.Index(i));
                }
                return true;
            }
            else if (mGameStage == GameStage.SENCOND_HALF)
            {
                for (int i = 0;i < mPackagesSecond.Amount;i++)
                {
                    mPackagesRemain.Add(mPackagesSecond.Index(i));
                }
                return true;
            }
            else 
            {
                return false;
            }
        }


        /***********************************************
        Pick and Delivery Package
        ***********************************************/
        public void PickPackage()
        {
            if (mCamp == Camp.A)
            {
                foreach(Package pkg in mPackagesRemain)
                {
                    if (pkg.GetDeparture(mCar_1.mPos) <= COLLISION_RADIUS)
                    {
                        mCar_1.PickPackage(pkg);
                    }
                }

            } 
            else if (mCamp == Camp.B)
            {
                foreach(Package pkg in mPackagesRemain)
                {
                    if (pkg.GetDeparture(mCar_2.mPos) <= COLLISION_RADIUS)
                    {
                        mCar_2.PickPackage(pkg);
                    }
                }
            }
        }

        public void DeliveryPackage()
        {
            if (mCamp == Camp.A)
            {
                foreach(Package pkg in mCar_1.mPickedPackages)
                {
                    if (pkg.GetDestination(mCar_1.mPos) <= COLLISION_RADIUS)
                    {
                        mCar_1.DropPackage(pkg);
                    }
                }

            } 
            else if (mCamp == Camp.B)
            {
                foreach(Package pkg in mCar_2.mPickedPackages)
                {
                    if (pkg.GetDeparture(mCar_2.mPos) <= COLLISION_RADIUS)
                    {
                        mCar_2.DropPackage(pkg);
                    }
                }
            }
        }


        /***********************************************
        Judge whether the car is in illegal area 
        i.e Out of Compertion Area, in Obstacle Area, in Opponent's Charge Station
        ***********************************************/
        public void CheckLocation()
        {
            if (mCamp = Camp.A)
            {
                // car's position state changed
                if ((IsOutOfCompetitionArea(mCar_1) && mCar_1.mIsInField) ||
                    (!IsOutOfCompetitionArea(mCar_1) && !mCar_1.mIsInField) )
                {
                    mCar_1.AddNonGatePunish();
                }

                if ()
            }
            else if (mCamp = Camp.B)
            {
                if ((IsOutOfCompetitionArea(mCar_2) && mCar_2.mIsInField) ||
                    (!IsOutOfCompetitionArea(mCar_2) && !mCar_2.mIsInField) )
                {
                    mCar_2.AddNonGatePunish();
                }
            }
        }


        /***********************************************
        Private Function
        ***********************************************/
        private bool IsOutOfCompetitionArea(Car _car)
        {
            Dot CarPos = _car.mPos;

            if (CarPos.x <= AVAILIABLE_MIN_X || CarPos.x >= AVAILIABLE_MAX_X ||
                CarPos.y <= AVAILIABLE_MIN_Y || CarPos.y >= AVAILIABLE_MAX_Y)
            {
                return true;
            }
            else
            {
                return true;
            }
        }

        private bool IsInObstacle (Car _car)
        {
            return mObstacle.isCollidedWall(_car.mPos, COLLISION_RADIUS);
        }

        private bool IsInOpponentStation (Car _car)
        {
            
        }
    }

}