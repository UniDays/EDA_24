using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDCHOST24
{
    //Package,外卖
    //包括：位置Dot Pos，规定的配送时间t，是否已经被获取 int
    public class Package
    {
        public Dot m_StartPos; //外卖起点
        public Dot m_EndPos;//外卖终点
        public int t; //规定运送时间
        public int IsPicked; //是否已经被获取.//改成int，因为bool转换不到byte型，0为没有拾取，1为已拾取
        public Package(Dot StartPos,Dot EndPos)
        {
            m_StartPos = StartPos;
            m_EndPos = EndPos;
            t =calculate_t(StartPos,EndPos);
            IsPicked = 0;
        }

        public Package()
        {
            m_StartPos = new Dot(32, 32);
            m_EndPos = new Dot(222, 222);//需要再更改 ybj 8/27
            t = calculate_t(m_StartPos, m_EndPos);
            IsPicked = 0;
        }

        public Dot GetStartDot()
        {
            return m_StartPos;
        }

        public Dot GetEndDot()
        {
            return m_EndPos;
        }

        public void PickPackage()
        {
            IsPicked = 1;
        }
        
        // ybj 8/27 需调试
        public int calculate_t(Dot StartPos, Dot EndPos)
        {
            return ((int)Math.Sqrt((StartPos.x - EndPos.x) * (StartPos.x - EndPos.x) + (StartPos.y - EndPos.y) * (StartPos.y - EndPos.y)))/2;
        }

    }
}
