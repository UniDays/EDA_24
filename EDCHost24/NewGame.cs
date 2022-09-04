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
    public enum GameStage { FIRST_HALF_1 = 0, FIRST_HALF_2 = 1, 
                            SENCOND_HALF_1 = 2, SECOND_HALF = 3 ,END = 4};

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
        public PackageList mPackages;

        // obstacle
        public Obstacle mObstacle;

        // Record the time when the trolley enters the area
        public Dot mLastPos;

        public int mLastWrongAreaTime;
        public int mLastOnObstacleTime;
        public int mLastOnStationTime;

        


        public Game()
        {
            Debug.WriteLine("Call Constructor of Game")

            mGameStage = UNSTART;
            mGameState = FIRST_HALF_1;

            CarA = new Car(Camp.A, 0);
            CarB = new Car(Camp.B, 1);

            mPackages = new PackageList(AVAILIABLE_MAX_X, AVAILIABLE_MIN_X, 
                        AVAILIABLE_MAX_Y, AVAILIABLE_MIN_Y, INITIAL_PKG_NUM, FIRST_HALF_TIME, TIME_INTERVAL);

            mStartTime = GetCurrentTime();
            mGameTime = 0;

            mObstacle = new Obstacle();

            mLastPos = Dot(-1, -1);

            mLastWrongAreaTime = -1;
            mLastOnObstacleTime = -1;
            mLastOnStationTime = -1;
        }







        private int GetCurrentTime()
        {
            System.DateTime currentTime = System.DateTime.Now;
            int time = currentTime.Hour * 3600000 + currentTime.Minute * 60000 + currentTime.Second * 1000;
            //Debug.WriteLine("H, M, S: {0}, {1}, {2}", currentTime.Hour, currentTime.Minute, currentTime.Second);
            //Debug.WriteLine("GetCurrentTime，Time = {0}", time); 
            return time;
        }
    }

}