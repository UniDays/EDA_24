﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDCHOST24
{
    public class Labyrinth
    {
        public List<string> LabyName;
        public const int MAX_WALL_NUM = 8;
        public Wall[] mpWallList;
        public string FileNameNow;

        // 障碍物是否已经被设置
        public bool IsLabySet;


        // 默认构造函数
        public Labyrinth()
        {
            IsLabySet = false;
            // 默认的障碍物数量是8个
            mpWallList = new Wall[MAX_WALL_NUM];
            LabyName = new List<string>();

            mpWallList = new Wall[MAX_WALL_NUM] {new Dot(0, 0), new Dot (0,0)};
        }

        // 从文本读取障碍物信息
        public void ReadFromFile(string FileName)
        { 
            try
            {
                IsLabySet = false;
                TextReader reader = File.OpenText("labyrinth/" + FileName);
                for (int i = 0; i < MAX_WALL_NUM; i++)
                {
                    string text = reader.ReadLine();
                    string[] bits = text.Split(' ');
                    int x1 = int.Parse(bits[0]);
                    int y1 = int.Parse(bits[1]);
                    int x2 = int.Parse(bits[2]);
                    int y2 = int.Parse(bits[3]);
                    mpWallList[i] = new Wall(new Dot(x1, y1), new Dot(x2, y2));
                }
                // 障碍物成功设置
                IsLabySet = true;

                Debug.WriteLine("Labyrinth Created from text.");
            }
            catch (ArgumentException)
            {
                MessageBox.Show("无效的障碍物文件路径");
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("不存在指定的障碍物文件");
            }
            catch (NotSupportedException)
            {
                MessageBox.Show("文件路径格式无效");
            }
            FileNameNow = FileName;
        }

        public void GetLabyName()
        {
            // 将障碍物文件名列表清空
            LabyName.Clear();

            // 绑定到指定的文件夹目录
            DirectoryInfo dir = new DirectoryInfo("labyrinth");

            // 检索表示当前目录的文件和子目录
            FileSystemInfo[] fsinfos = dir.GetFileSystemInfos();

            // 遍历检索的文件和子目录
            foreach (FileSystemInfo fsinfo in fsinfos)
            {
                Console.WriteLine(fsinfo.Name);

                // 将得到的文件名放入到list中
                LabyName.Add(fsinfo.Name); 
            }
        }

        // 判断是否与障碍发生碰撞
        public static bool isCollided(Dot CarPos, int radius = 0)
        {
            if (radius < 0)
            {
                radius = 0;
            }
            foreach(Wall wall in mLabyrinth.mpWallList)
            {
                if (Utility.DistanceL(wall, CarPos) < radius)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
