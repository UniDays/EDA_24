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
        public const int AVAILIABLE_MAX_X = 254;
        public const int AVAILIABLE_MIN_X = 0;
        public const int AVAILIABLE_MAX_Y = 254;
        public const int AVAILIABLE_MIN_Y = 0;

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
        public int mScore_1, mScore_2;
        private PackageList mPackagesFirst;
        private PackageList mPackagesSecond;
        public Station mChargeStation_1;
        public Station mChargeStation_2;

        public List<Package> mPackagesRemain;

        // which team is racing A or B
        public Camp mCamp;

        // obstacle
        public Labyrinth mObstacle;

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

            // Default
            mCamp = Camp.A;

            CarA = new Car(Camp.A, 0);
            CarB = new Car(Camp.B, 0);

            mPackagesFirst = new PackageList(AVAILIABLE_MAX_X, AVAILIABLE_MIN_X, 
                        AVAILIABLE_MAX_Y, AVAILIABLE_MIN_Y, INITIAL_PKG_NUM, FIRST_HALF_TIME, TIME_INTERVAL);

            mPackagesSecond = new PackageList(AVAILIABLE_MAX_X, AVAILIABLE_MIN_X, 
                        AVAILIABLE_MAX_Y, AVAILIABLE_MIN_Y, INITIAL_PKG_NUM, FIRST_HALF_TIME, TIME_INTERVAL);

            mChargeStation_1 = new Station ();
            mChargeStation_2 = new Station ();

            mPackagesRemain = new List<Package> ();

            mStartTime = _GetCurrentTime();
            mGameTime = 0;

            mObstacle = new Obstacle();

            mLastPos = Dot(-1, -1);

            mLastWrongAreaTime = -1;
            mLastOnObstacleTime = -1;
            mLastOnStationTime = -1;
        }


        /***********************************************
        Update Parameters
        ***********************************************/
        public void Update()
        {
            _UpdateGameTime();

            // Try to generate packages on each refresh
            if (!GeneratePackage())
            {
                Debug.WriteLine("Package generation is not allowed at this stage");
            }

            // Team A is on racing
            if (mCamp == Camp.A)
            {

            }
            else if (mCamp == Camp.B)
            {

            }

            
        }


        /***********************************************
        Change the team
        ***********************************************/
        public void SetCamp(int _whichone) 
        {
            if (_whichone == 0)
            {
                mCamp = Camp.A;
                mPackagesRemain.Clear();
                _InitialPackagesRemain();
                Debug.WriteLine("Team A is going to race");
            }
            else if (_whichone == 1)
            {
                mCamp = Camp.B;
                mPackagesRemain.Clear();
                _InitialPackagesRemain();
                Debug.WriteLine("Team B is going to race");
            }
            else
            {
                Debug.WriteLine("Expect 0 or 1, but input is out of range");
            }
        }

        /***********************************************
        Enter the Second Half
        ***********************************************/
        public void EnterSecondHalf()
        {
            mGameStage = GameStage.SENCOND_HALF;
            mScore_1 = mCar_1.GetScore();
            mScore_2 = mCar_2.GetScore();
            mCar_1.Reset();
            mCar_2.Reset();

            mPackagesRemain.Clear();

            Debug.WriteLine("GameStage has been set to SECOND_HALF");
            Debug.WriteLine("The score of car has been save");
        }

        /***********************************************
        Initialize and Generate Package
        ***********************************************/
        public bool GeneratePackage ()
        {
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
        Set Charge Station
        ***********************************************/
        public void SetChargeStation ()
        {
            if (mCamp == Camp.A)
            {
                _SetChargeStation(mCar_1);
            } 
            else if (mCamp == Camp.B)
            {
                _SetChargeStation(mCar_2);
            }
        }



        /***********************************************
        Get Penalty for accessing illegal area
        i.e Out of Competition Area, in Obstacle Area, in Opponent's Charge Station
        ***********************************************/
        public void GetPenalty()
        {
            if (mCamp == Camp.A)
            {
                _Penalty(mCar_1);
            }
            else if (mCamp == Camp.B)
            {
                _Penalty(mCar_2);
            }
        }






        /***********************************************************************
        Private Functions
        ***********************************************************************/

        /***********************************************
        Initialize Packages Remain
        ***********************************************/
        private bool _InitialPackagesRemain()
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
        Penalty for Access illegal Area
        ***********************************************/
        private void _Penalty (ref Car _car)
        {
            if ((_IsOutOfCompetitionArea(_car) && _car.mIsInField) ||
                (!_IsOutOfCompetitionArea(_car) && !_car.mIsInField) )
            {
                _car.AddNonGatePunish();
            }

            if ((_IsInObstacle(_car) && _car.mIsInObstacle) ||
                (!_IsInObstacle(_car) && !_car.mIsInObstacle))
            {
                _car.InObstacle();
            }

            if (mGameStage == GameStage.SENCOND_HALF && 
                ((_IsInOpponentStation(_car) && _car.mIsInOpponentChargeStation) ||
                (!_IsInObstacle(_car) && !_car.mIsInOpponentChargeStation)))
            {
                _car.InOpponentStation();
            }
        }

        private bool _IsOutOfCompetitionArea(Car _car)
        {
            
        }

        private bool _IsInObstacle (Car _car)
        {
            
        }

        private bool _IsInOpponentStation (Car _car)
        {
            
        }

        /***********************************************
        Set Charge Station in First-Half
        ***********************************************/
        private void _SetChargeStation (ref Car _car) 
        {
            if (mGameStage == GameStage.SENCOND_HALF) 
            {
                return;
            }

            if (_car.AddChargeCount())
            {
                if (_car.MyCamp == Camp.A)
                {
                    mChargeStation_1.Add(mCar_1.GetCarPos(0));
                }
                else if (_car.MyCamp == Camp.B)
                {
                    mChargeStation_2.Add(mCar_2.GetCarPos(0));
                }
            }
        }

        /***********************************************
        Time
        ***********************************************/
        private static int _GetCurrentTime()
        {
            System.DateTime currentTime = System.DateTime.Now;
            int time = currentTime.Hour * 3600000 + currentTime.Minute * 60000 + currentTime.Second * 1000;
            //Debug.WriteLine("H, M, S: {0}, {1}, {2}", currentTime.Hour, currentTime.Minute, currentTime.Second);
            //Debug.WriteLine("GetCurrentTime，Time = {0}", time); 
            return time;
        }

        private void _UpdateGameTime ()
        {
            mGameTime = _GetCurrentTime() - mStartTime;
        }
    }

}