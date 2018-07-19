using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class PL_SD
    {
        //初始化参数
        public PL_SD()
        {
            Is_PL = false;
            Is_tag = false;
            Num = 0;
            Zc_high = 0;
            Zc_low = 0;
            Zc_mid = 0;
            Yz = 0;
            No = 0;
            Zero = 0;
            Bids = new List<BID_SD>();

            //以下为时段属性初始化
            State = -1;
            Enter_Num = 0;
            Zc_Date = "";
            Zc_Date2 = "";
            Zc_Date3 = "";
        }

        /// <summary>
        /// 试探失败，初始化
        /// </summary>
        private void Init()
        {
            Is_PL = false;
            Is_tag = false;
            Num = 0;
            Zc_high = 0;
            Zc_low = 0;
            Zc_mid = 0;
            Yz = 0;
            No = 0;
            Zero = 0;

            //以下为时段属性初始化
            State = -1;
            Enter_Num = 0;
            Zc_Date = "";
            Zc_Date2 = "";
            Zc_Date3 = "";
        }

        public double Zc_low { get; set; }  // 支撑区域底部
        public double Zc_high { get; set; } // 支撑区域顶部
        public double Zc_mid { get; set; } // 支撑区域中位线
        public int Num { get; set; } //试探成功次数
        public int Yz { get; set; } //一致性，0：没有 1：早盘 2：欧盘 3：美盘
        public int No { get; set; }//支撑区确立后蜡烛条数
        public bool Is_PL { get; set; } //是否可以试探
        public List<BID_SD> Bids { get; set; } //入场位集合

        public bool Is_tag { get; set; } //是否为目标

        public double BID { get; set; }  //入场位 
        public int Zero { get; set; }//支撑区域成立的蜡烛位置


        /*以下为时段判断属性*/
        public int Enter_Num { get; set; } //入场位序列
        public string Zc_Date { get; set; } //支撑区域日期
        public double ZP_ZD { get; set; } //时段早盘最低点
        public double ZP_ZG { get; set; } //时段早盘最高点
        public double ZP_SP { get; set; }//时段早盘收盘
        public double ZP_KP { get; set; }//时段早盘开盘

        public string Zc_Date2 { get; set; } //第一天试探日期
        public double ZP_ZD2 { get; set; } //第一天试探时段早盘最低点
        public double ZP_ZG2 { get; set; } //第一天试探时段早盘最高点
        public double ZP_SP2 { get; set; }//第一天试探时段早盘收盘
        public double ZP_KP2 { get; set; }//第一天试探时段早盘开盘

        public string Zc_Date3 { get; set; } //第二天试探日期
        public double ZP_ZD3 { get; set; } //第二天试探时段早盘最低点
        public double ZP_ZG3 { get; set; } //第二天试探时段早盘最高点
        public double ZP_SP3{ get; set; }//第二天试探时段早盘收盘
        public double ZP_KP3 { get; set; }//第二天试探时段早盘开盘

        
        public double WP_ZD { get; set; } //时段欧盘最低点
        public double WP_ZG { get; set; } //时段欧盘最高点
        public double WP_SP { get; set; }//时段欧盘收盘
        public double WP_KP { get; set; }//时段欧盘开盘


        public double WP_ZD2 { get; set; } //第一天试探时段欧盘最低点
        public double WP_ZG2 { get; set; } //第一天试探时段欧盘最高点
        public double WP_SP2 { get; set; }//第一天试探时段欧盘收盘
        public double WP_KP2 { get; set; }//第一天试探时段欧盘开盘

        public double WP_ZD3 { get; set; } //第二天试探时段欧盘最低点
        public double WP_ZG3 { get; set; } //第二天试探时段欧盘最高点
        public double WP_SP3 { get; set; }//第二天试探时段欧盘收盘
        public double WP_KP3 { get; set; }//第二天试探时段欧盘开盘



        public double MP_ZD { get; set; } //时段美盘最低点
        public double MP_ZG { get; set; } //时段美盘最高点
        public double MP_SP { get; set; }//时段美盘收盘
        public double MP_KP { get; set; }//时段美盘开盘

        public double MP_ZD2 { get; set; } //第一天试探时段美盘最低点
        public double MP_ZG2 { get; set; } //第一天试探时段美盘最高点
        public double MP_SP2 { get; set; }//第一天试探时段美盘收盘
        public double MP_KP2 { get; set; }//第一天试探时段美盘开盘

   
        public double MP_ZD3 { get; set; } //第二天试探时段美盘最低点
        public double MP_ZG3 { get; set; } //第二天试探时段美盘最高点
        public double MP_SP3 { get; set; }//第二天试探时段美盘收盘
        public double MP_KP3 { get; set; }//第二天试探时段美盘开盘


        public int State { get; set; }  //试探状态值，-1:未设置区域，0=已设置区域，未试探，1=第一天亚盘试探中 ，2=第二天欧盘试探中,3=第三天美盘试探中

        /// <summary>
        /// 设置支撑区
        /// </summary>
        public int Set_Zc(AUDUSD_1H now, int seq, double zs, double zy, double beyond)
        {
            Algorithm al = new Algorithm();

            if (now.date == "2017.12.22")
            {
                ;
            }

            if (State == -1) //初始化状态
            {
                if ((int)al.Sds[now.time] == 2)   //以美盘作为开始
                {
                    if (now.time != "15:00")
                    {
                        return -1; //未进行设置
                    }
                    else  //从15：00开盘位算起
                    {
                        Zc_Date = now.date;
                        MP_KP = (double)now.kp;  //设置美盘开盘位
                        MP_ZD = (double)now.zd; //设置美盘最低位，以后K线每次比对，取最小值
                        MP_ZG = (double)now.zg; //设置美盘开盘位，以后K线每次比对，取最大值
                        //后续可加入支撑区大小设置，太大可能影响入场位
                        State = 0;
                        return 0; //完成初始化设置
                    }
                }
            }
            else if (State == 0) //已初步确立支撑区域
            {
                if (Zc_Date == now.date)
                {
                    MP_ZD = Math.Min((double)now.zd, MP_ZD);//设置美盘最低位，以后K线每次比对，取最小值
                    MP_ZG = Math.Max((double)now.zg, MP_ZG);//设置美盘开盘位，以后K线每次比对，取最大值
                    if (now.time == "23:00")
                    {
                        MP_SP = (double)now.sp; //设置美盘收盘位
                        Zc_low = MP_ZD;
                        Zc_high = MP_SP;
                        Zc_mid = (Zc_high + Zc_low) / 2;
                        State = 1;
                    }
                    return 0;
                }
            }




            else if (State == 1)
            {
                if (Zc_Date2 == "") //首次测试
                {
                    Zc_Date2 = now.date;
                    ZP_KP = (double)now.kp;  //设置亚盘开盘位
                    ZP_ZD = (double)now.zd; //设置亚盘最低位，以后K线每次比对，取最小值
                    ZP_ZG = (double)now.zg; //设置亚盘开盘位，以后K线每次比对，取最大值

                    if (now.time == "08:00") //有可能第一个K就是收盘K
                    {
                        ZP_SP = (double)now.sp; //设置亚盘收盘位

                        if (ZP_ZD < Zc_low)  //最低点超过支撑区域，试探失败
                        {
                            //if (ZP_SP >= Zc_low)
                            //{
                            //    State = 2;
                            //    return 2;
                            //}
                            //else
                            //{
                            //    Init();
                            //    return -1;
                            //}
                            Init();
                            return -1;

                        }
                        else if (ZP_ZD >= Zc_low && ZP_ZD < Zc_high) //最低点在支撑区域之间，支撑成功
                        {
                            State = 2;
                            return 2;
                        }
                        else if (ZP_ZD >= Zc_high)//最低点在支撑区域上，暂时定义为成功
                        {
                            State = 2;
                            return 2;
                        }
                    }

                    State = 1;
                    return 1; //完成次日亚盘设置
                }
                else if (Zc_Date2 == now.date)
                {
                    ZP_ZD = Math.Min((double)now.zd, ZP_ZD);//设置亚盘最低位，以后K线每次比对，取最小值
                    ZP_ZG = Math.Max((double)now.zg, ZP_ZG);//设置亚盘开盘位，以后K线每次比对，取最大值
                    if (now.time == "08:00")
                    {
                        ZP_SP = (double)now.sp; //设置亚盘收盘位

                        if (ZP_ZD < Zc_low)  //最低点超过支撑区域，试探失败
                        {
                            //if (ZP_SP >= Zc_low)
                            //{
                            //    State = 2;
                            //    return 2;
                            //}
                            //else
                            //{
                            //    Init();
                            //    return -1;
                            //}
                            Init();
                            return -1;

                        }
                        else if (ZP_ZD >= Zc_low && ZP_ZD < Zc_high) //最低点在支撑区域之间，支撑成功
                        {
                            State = 2;
                            return 2;
                        }
                        else if (ZP_ZD >= Zc_high)//最低点在支撑区域上，暂时定义为成功
                        {
                            State = 2;
                            return 2;
                        }
                    }
                    return 1;
                }
            }

            else if (State == 2)
            {

                if (now.time == "09:00") //首次测试
                {
                    //Zc_Date2 = now.date;
                    WP_KP = (double)now.kp;  //设置欧盘开盘位
                    WP_ZD = (double)now.zd; //设置欧盘最低位，以后K线每次比对，取最小值
                    WP_ZG = (double)now.zg; //设置欧盘开盘位，以后K线每次比对，取最大值



                    State = 2;
                    return 2; //完成次日亚盘设置
                }
                else
                {
                    WP_ZD = Math.Min((double)now.zd, WP_ZD);//设置欧盘最低位，以后K线每次比对，取最小值
                    WP_ZG = Math.Max((double)now.zg, WP_ZG);//设置欧盘开盘位，以后K线每次比对，取最大值
                    if (now.time == "14:00")
                    {
                        WP_SP = (double)now.sp; //设置欧盘收盘位

                        if (WP_ZD < Zc_low)  //最低点超过支撑区域，试探失败
                        {
                            //if (WP_SP >= Zc_low)
                            //{
                            //    State = 3;
                            //    return 3;
                            //}
                            //else
                            //{
                            //    Init();
                            //    return -1;
                            //}

                            Init();
                            return -1;
                        }
                        else if (WP_ZD >= Zc_low && WP_ZD < Zc_high) //最低点在支撑区域之间，支撑成功
                        {
                            State = 3;
                            return 3;
                        }
                        else if (WP_ZD >= Zc_high)//最低点在支撑区域上，暂时定义为成功
                        {
                            State = 3;
                            return 3;
                        }
                    }
                    return 2;
                }
            }

            else if (State == 3)
            {

                if (now.time == "15:00") //首次测试
                {
                    //Zc_Date2 = now.date;
                    MP_KP2 = (double)now.kp;  //设置次日美盘开盘位
                    MP_ZD2 = (double)now.zd; //设置次日美盘最低位，以后K线每次比对，取最小值
                    MP_ZG2 = (double)now.zg; //设置次日美盘开盘位，以后K线每次比对，取最大值

                    State = 3;
                    return 3; //完成次日亚盘设置
                }
                else
                {
                    MP_ZD2 = Math.Min((double)now.zd, MP_ZD2);//设置次日美盘最低位，以后K线每次比对，取最小值
                    MP_ZG2 = Math.Max((double)now.zg, MP_ZG2);//设置次日美盘开盘位，以后K线每次比对，取最大值
                    if (now.time == "23:00")
                    {
                        MP_SP2 = (double)now.sp; //设置次日美盘收盘位

                        if (MP_ZD2 < Zc_low)  //最低点超过支撑区域，试探失败
                        {
                            //if (MP_SP2 >= Zc_low)
                            //{
                            //    Enter_Num = now.num;
                            //    Set_Enter(48, zs, beyond);
                            //    State = 4;
                            //    Init();
                            //    return 4;
                            //}
                            //else
                            //{
                            //    Init();
                            //return -1;
                            //}
                            Init();
                            return -1;
                        }
                        else if (MP_ZD2 >= Zc_low && MP_ZD2 < Zc_high) //最低点在支撑区域之间，支撑成功
                        {
                            Enter_Num = now.num;
                            Set_Enter(zs, beyond);
                            State = 4;
                            Init();
                            return 4;
                        }
                        else if (MP_ZD2 >= Zc_high)//最低点在支撑区域上，暂时定义为成功
                        {
                            Enter_Num = now.num;
                            Set_Enter(zs, beyond);
                            State = 4;
                            Init();
                            return 4;
                        }
                    }
                    return 2;
                }
            }

            return 0;


        }        /// <summary>
                 /// 设置支撑区
                 /// </summary>
        public int Set_Zc2(AUDUSD_1H now, int seq, double zs, double zy, double beyond)
        {
            Algorithm al = new Algorithm();

            

            if (State == -1) //初始化状态
            {
                if ((int)al.Sds[now.time] == 2)   //以美盘作为开始
                {
                    if (now.time != "15:00")
                    {
                        return -1; //未进行设置
                    }
                    else  //从15：00开盘位算起
                    {
                        Zc_Date = now.date;
                        MP_KP = (double)now.kp;  //设置美盘开盘位
                        MP_ZD = (double)now.zd; //设置美盘最低位，以后K线每次比对，取最小值
                        MP_ZG = (double)now.zg; //设置美盘开盘位，以后K线每次比对，取最大值
                        //后续可加入支撑区大小设置，太大可能影响入场位
                        State = 0;
                        return 0; //完成初始化设置
                    }
                }
            }
            else if (State == 0) //已初步确立支撑区域
            {
                if ((int)al.Sds[now.time] == 2)   //必需美盘作为开始，否则重置
                {
                    if (Zc_Date == now.date)
                    {
                        MP_ZD = Math.Min((double)now.zd, MP_ZD);//设置美盘最低位，以后K线每次比对，取最小值
                        MP_ZG = Math.Max((double)now.zg, MP_ZG);//设置美盘开盘位，以后K线每次比对，取最大值
                        if (now.time == "23:00")
                        {
                            MP_SP = (double)now.sp; //设置美盘收盘位
                            Zc_low = MP_ZD;
                            Zc_high = MP_SP;
                            Zc_mid = (Zc_high + Zc_low) / 2;
                            State = 1;
                        }
                        return 0;
                    }
                }
                else
                {
                    Init();
                    return -1;
                }
            }




            else if (State == 1)
            {
                if ((int)al.Sds[now.time] == 0)   //必需以欧盘作为开始，否则重置
                {
                    if (Zc_Date2 == "") //首次测试
                    {
                        Zc_Date2 = now.date;
                        ZP_KP = (double)now.kp;  //设置亚盘开盘位
                        ZP_ZD = (double)now.zd; //设置亚盘最低位，以后K线每次比对，取最小值
                        ZP_ZG = (double)now.zg; //设置亚盘开盘位，以后K线每次比对，取最大值

                        if (now.time == "08:00") //有可能第一个K就是收盘K
                        {
                            ZP_SP = (double)now.sp; //设置亚盘收盘位

                            if (ZP_ZD < Zc_low)  //最低点超过支撑区域，试探失败
                            {
                                //if (ZP_SP >= Zc_low)
                                //{
                                //    State = 2;
                                //    return 2;
                                //}
                                //else
                                //{
                                //    Init();
                                //    return -1;
                                //}
                                Init();
                                return -1;

                            }
                            else if (ZP_ZD >= Zc_low && ZP_ZD < Zc_high) //最低点在支撑区域之间，支撑成功
                            {
                                State = 2;
                                return 2;
                            }
                            else if (ZP_ZD >= Zc_high)//最低点在支撑区域上，暂时定义为成功
                            {
                                State = 2;
                                return 2;
                            }
                        }

                        State = 1;
                        return 1; //完成次日亚盘设置
                    }
                    else if (Zc_Date2 == now.date)
                    {
                        ZP_ZD = Math.Min((double)now.zd, ZP_ZD);//设置亚盘最低位，以后K线每次比对，取最小值
                        ZP_ZG = Math.Max((double)now.zg, ZP_ZG);//设置亚盘开盘位，以后K线每次比对，取最大值
                        if (now.time == "08:00")
                        {
                            ZP_SP = (double)now.sp; //设置亚盘收盘位

                            if (ZP_ZD < Zc_low)  //最低点超过支撑区域，试探失败
                            {
                                //if (ZP_SP >= Zc_low)
                                //{
                                //    State = 2;
                                //    return 2;
                                //}
                                //else
                                //{
                                //    Init();
                                //    return -1;
                                //}
                                Init();
                                return -1;

                            }
                            else if (ZP_ZD >= Zc_low && ZP_ZD < Zc_high) //最低点在支撑区域之间，支撑成功
                            {
                                State = 2;
                                return 2;
                            }
                            else if (ZP_ZD >= Zc_high)//最低点在支撑区域上，暂时定义为成功
                            {
                                State = 2;
                                return 2;
                            }
                        }
                        return 1;
                    }
                }
                else
                {
                     Init();
                    return -1;
                }
            }

            else if (State == 2)
            {
                if ((int)al.Sds[now.time] == 1)   //必需以欧盘作为开始，否则重置
                {

                    if (now.time == "09:00") //首次测试
                    {
                        //Zc_Date2 = now.date;
                        WP_KP = (double)now.kp;  //设置欧盘开盘位
                        WP_ZD = (double)now.zd; //设置欧盘最低位，以后K线每次比对，取最小值
                        WP_ZG = (double)now.zg; //设置欧盘开盘位，以后K线每次比对，取最大值



                        State = 2;
                        return 2; //完成次日亚盘设置
                    }
                    else
                    {
                        WP_ZD = Math.Min((double)now.zd, WP_ZD);//设置欧盘最低位，以后K线每次比对，取最小值
                        WP_ZG = Math.Max((double)now.zg, WP_ZG);//设置欧盘开盘位，以后K线每次比对，取最大值
                        if (now.time == "14:00")
                        {
                            WP_SP = (double)now.sp; //设置欧盘收盘位

                            if (WP_ZD < Zc_low)  //最低点超过支撑区域，试探失败
                            {
                                //if (WP_SP >= Zc_low)
                                //{
                                //    State = 3;
                                //    return 3;
                                //}
                                //else
                                //{
                                //    Init();
                                //    return -1;
                                //}

                                Init();
                                return -1;
                            }
                            else if (WP_ZD >= Zc_low && WP_ZD < Zc_high) //最低点在支撑区域之间，支撑成功
                            {
                                State = 3;
                                return 3;
                            }
                            else if (WP_ZD >= Zc_high)//最低点在支撑区域上，暂时定义为成功
                            {
                                State = 3;
                                return 3;
                            }
                        }
                        return 2;
                    }
                }
                else
                {
                    Init();
                    return -1;
                }
            }

            else if (State == 3)
            {
                if ((int)al.Sds[now.time] == 2)   //必需美盘作为开始，否则重置
                {
                    if (now.time == "15:00") //首次测试
                    {
                        //Zc_Date2 = now.date;
                        MP_KP2 = (double)now.kp;  //设置次日美盘开盘位
                        MP_ZD2 = (double)now.zd; //设置次日美盘最低位，以后K线每次比对，取最小值
                        MP_ZG2 = (double)now.zg; //设置次日美盘开盘位，以后K线每次比对，取最大值

                        State = 3;
                        return 3; //完成次日亚盘设置
                    }
                    else
                    {
                        MP_ZD2 = Math.Min((double)now.zd, MP_ZD2);//设置次日美盘最低位，以后K线每次比对，取最小值
                        MP_ZG2 = Math.Max((double)now.zg, MP_ZG2);//设置次日美盘开盘位，以后K线每次比对，取最大值
                        if (now.time == "23:00")
                        {
                            MP_SP2 = (double)now.sp; //设置次日美盘收盘位

                            if (MP_ZD2 < Zc_low)  //最低点超过支撑区域，试探失败
                            {
                                //if (MP_SP2 >= Zc_low)
                                //{
                                //    Enter_Num = now.num;
                                //    Set_Enter(48, zs, beyond);
                                //    State = 4;
                                //    Init();
                                //    return 4;
                                //}
                                //else
                                //{
                                //    Init();
                                //return -1;
                                //}
                                Init();
                                return -1;
                            }
                            else if (MP_ZD2 >= Zc_low && MP_ZD2 < Zc_high) //最低点在支撑区域之间，支撑成功
                            {
                                Enter_Num = now.num;
                                Set_Enter(zs, beyond);
                                State = 4;
                                Init();
                                return 4;
                            }
                            else if (MP_ZD2 >= Zc_high)//最低点在支撑区域上，暂时定义为成功
                            {
                                Enter_Num = now.num;
                                Set_Enter(zs, beyond);
                                State = 4;
                                Init();
                                return 4;
                            }
                        }
                        return 2;
                    }
                }
                else
                {
                    Init();
                    return -1;
                }
            }

            return 0;


        }


        /// <summary>
        /// 设置入场位
        /// </summary>
        /// <param name="num">设置试探成功次数</param>
        /// <param name="seq">设置试探蜡烛次数要求</param>
        /// <param name="zs">设置止损点数要求</param>
        /// <param name="zs">入场位序列</param>
        /// <param name="l">入场位类列表</param>
        private void Set_Enter( double zs, double beyond)
        {

            //设置入场位置=支撑最低点-超出点+zs限制
            BID = Math.Min(Zc_low - beyond + zs, Zc_high); //考虑踏空次数，看是否需要取两者小值 
                                                           //    Console.WriteLine("产生入场位置序列{0}，支撑位置产生序列{1}，入场位置{2}",no,Zero,BID);
            BID_SD b = new BID_SD();
            b.zc_date = this.Zc_Date;
            b.set_date = this.Zc_Date2;
            b.Zc_high = Zc_high;
            b.Zc_low = Zc_low;
            b.Zc_mid = Zc_mid;
            b.Zero = Enter_Num;
            b.Fin = Enter_Num;
            b.bid = BID;
            b.No = Enter_Num;
            b.zc_date = Zc_Date;
            b.set_date = Zc_Date2;
            Bids.Add(b);
            //清零，重新开始
            Init();

           
            return;
        }

    }



    class BID_SD //入场位类
    {
        public BID_SD()
        {
            Is_PL = false;
            Zc_high = 0;
            Zc_low = 0;
            Zc_mid = 0;
            Yz = 0;
            No = 0;
            Zero = 0;
            Fin = 0;
            Is_Enter = false;
            Out = 0;
            Expire = 0;
            Is_expire = false;
            Target = 0;
            Stop = 0;
            BapPing = -1;
            Result = -1;
            Profit = 0;
        }

        public double Zc_low { get; set; }  // 支撑区域底部
        public double Zc_high { get; set; } // 支撑区域顶部
        public double Zc_mid { get; set; } // 支撑区域中位线
        public int Yz { get; set; } //一致性，0：没有 1：早盘 2：欧盘 3：美盘
        public int No { get; set; }//支撑区确立后蜡烛条数
        public bool Is_PL { get; set; } //是否可以试探

        public double bid { get; set; }  //入场位 
        public int Zero { get; set; }   //支撑区域成立的蜡烛位置 
        public int Fin { get; set; }   //入场位成立的蜡烛位置
        public bool Is_Enter { get; set; } //是否最终入场
        public double Out { get; set; }  //出场位
        public int Expire { get; set; } //设置入场位确立后，多少根K后取消
        public bool Is_expire { get; set; }//是否超时
        public int Enter_No { get; set; } //实际进场位置序列
        public int Out_No { get; set; } //实际出场位置序列
        public double Target { get; set; } //止盈位
        public double Stop { get; set; } //止损位
        public double BapPing { get; set; }//平保位
        public int Result { get; set; } //结果

        public double Profit { get; set; } //利润点数


        /**以下是趋势线坐标属性 **/
        public double Zc_x { get; set; } //横坐标
        public double Zc_y { get; set; } //纵坐标

        public double Now_a { get; set; } //近期趋势线a常数  y=ax+b
        public double Now_b { get; set; } //近期趋势线a常数  y=ax+b
        public double Last_a { get; set; } //远期趋势线a常数  y=ax+b
        public double Last_b { get; set; } //远期趋势线a常数  y=ax+b
        public double Now_rate { get; set; } //近期趋势线斜率
        public double Last_rate { get; set; } //远期趋势线斜率

        /*以下为时段判断属性*/
        public int Enter_Num { get; set; } //入场位序列
        public string zc_date { get; set; }//支撑区域确定日期
        public string set_date { get; set; }//测试完成日期
        public string enter_date { get; set; }//进场日期
        public string outer_date { get; set; }//出场日期

       

       


        /// <summary>
        /// 下单函数
        /// </summary>
        /// <param name="i"></param>
        /// <param name="expire">入场位有效蜡烛</param>
        public bool Enter(AUDUSD_1H i, int expire, double zs, double zy)
        {
            Expire = Expire + 1;//
            if (Expire < expire)
            {
                //if (i.zd <= bid&&i.kp>=i.sam_5 && i.sam_5 >= i.sam_10 && i.sam_10 >= i.sam_20)  //增加均线判断
                if (i.zd <= bid)   //最低点在买入点以下，可买入
                {

                    Is_Enter = true;
                    Enter_No = i.num;
                    Target = bid + zy;
                    Stop = bid - zs;
                    enter_date = i.date;
                    return true;
                }
            }
            return false;
        }

        
        /// <summary>
        /// 出场函数--买涨
        /// </summary>
        /// <param name="i"></param>
        /// <returns>1：止损，0 : 平局 ，2：止盈</returns>
        public int OutLine(AUDUSD_1H i)
        {


            if (i.zg >= Target)
            {
                if (i.zd > Stop)//止赢：最高点触及止盈位，且未触及止损位
                {
                    Profit = (Target - bid);
                    Result = 2;
                    Out_No = i.num;
                    outer_date = i.date;
                    return 2;
                }
                else if (i.zd <= Stop) //平局：止损止盈都触及
                {
                    Profit = 0;
                    Result = 0;
                    Out_No = i.num;
                    outer_date = i.date;
                    return 0;
                }
            }
            else if (i.zg < Target)
            {
                if (i.zd > Stop) //下一次：最高点未触及止赢，且最低点未触及止损位
                {
                    return -1;
                }
                else if (i.zd <= Stop) //止损：最低点触及止损，且未触及止盈位
                {
                    Profit = (Stop - bid);
                    outer_date = i.date;
                    Result = 1;
                    Out_No = i.num;
                    return 1;
                }
            }
            return -1;
        }

        /// <summary>
        /// 出场函数--买涨
        /// </summary>
        /// <param name="i"></param>
        /// <returns>1：止损，0 : 平局 ，2：止盈</returns>
        public int OutLine(AUDUSD_1H i, int a)
        {

            if (BapPing == -1)
            {
                if (i.zg >= Target)
                {
                    if (i.zd > Stop)//止赢：最高点触及止盈位，且未触及止损位
                    {
                        Profit = (Target - bid);
                        Result = 2;
                        Out_No = i.num;
                        outer_date = i.date;
                        return 2;
                    }
                    else if (i.zd <= Stop) //平局：止损止盈都触及
                    {
                        Profit = 0;
                        Result = 0;
                        Out_No = i.num;
                        outer_date = i.date;
                        return 0;
                    }
                }
                else if (i.zg < Target)
                {

                    if (i.zd <= Stop) //止损：最低点触及止损，且未触及止盈位
                    {
                        Profit = (Stop - bid);
                        Result = 1;
                        Out_No = i.num;
                        outer_date = i.date;
                        return 1;
                    }
                    else if (i.zd > Stop) //下一次：最高点未触及止赢，且最低点未触及止损位
                    {
                        //设置平保位
                        if (i.sp - bid >= 0.004)
                        {
                            BapPing = bid;
                        }
                        if (i.sp - bid >= 0.008)
                        {
                            BapPing = bid + 0.004;
                        }
                        if (i.sp - bid >= 0.012)
                        {
                            BapPing = bid + 0.008;
                        }
                        if (i.sp - bid >= 0.016)
                        {
                            BapPing = bid + 0.012;
                        }
                        if (i.sp - bid >= 0.02)
                        {
                            BapPing = bid + 0.016;
                        }
                        if (i.sp - bid >= 0.024)
                        {
                            BapPing = bid + 0.02;
                        }
                        if (i.sp - bid >= 0.028)
                        {
                            BapPing = bid + 0.024;
                        }
                        if (i.sp - bid >= 0.032)
                        {
                            BapPing = bid + 0.028;
                        }
                        if (i.sp - bid >= 0.036)
                        {
                            BapPing = bid + 0.032;
                        }
                        return -1;
                    }
                }
            }
            else if (BapPing != -1)
            {
                if (i.zg >= Target)
                {
                    if (i.zd > BapPing)//止赢：最高点触及止盈位，且未触及止损位
                    {
                        Profit = (Target - bid);
                        Result = 2;
                        Out_No = i.num;
                        outer_date = i.date;
                        return 2;
                    }
                    else if (i.zd <= BapPing) //保平平局：止盈保平都触及
                    {
                        Profit = BapPing - bid;
                        Result = 2;
                        Out_No = i.num;
                        outer_date = i.date;
                        return 2;
                    }
                }
                else if (i.zg < Target)
                {

                    if (i.zd <= BapPing) //保平出场：最低点触及平保，且未触及止盈位
                    {
                        Profit = BapPing - bid;
                        Result = 2;
                        Out_No = i.num;
                        outer_date = i.date;
                        return 2;
                    }
                    else if (i.zd > BapPing) //下一次：最高点未触及止赢，且最低点未触及止损位
                    {
                        //设置平保位
                        if (i.sp - bid >= 0.004)
                        {
                            BapPing = Math.Max(bid, BapPing);
                        }
                        if (i.sp - bid >= 0.008)
                        {
                            BapPing = Math.Max(bid + 0.004, BapPing);
                        }
                        if (i.sp - bid >= 0.012)
                        {
                            BapPing = Math.Max(bid + 0.008, BapPing);
                        }
                        if (i.sp - bid >= 0.016)
                        {
                            BapPing = Math.Max(bid + 0.012, BapPing);
                        }
                        if (i.sp - bid >= 0.02)
                        {
                            BapPing = Math.Max(bid + 0.016, BapPing);
                        }
                        if (i.sp - bid >= 0.024)
                        {
                            BapPing = Math.Max(bid + 0.02, BapPing);
                        }
                        if (i.sp - bid >= 0.028)
                        {
                            BapPing = Math.Max(bid + 0.024, BapPing);
                        }
                        if (i.sp - bid >= 0.032)
                        {
                            BapPing = Math.Max(bid + 0.028, BapPing);
                        }
                        if (i.sp - bid >= 0.036)
                        {
                            BapPing = Math.Max(bid + 0.032, BapPing);
                        }
                        return -1;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// 出场函数--买涨
        /// </summary>
        /// <param name="i"></param>
        /// <returns>1：止损，0 : 平局 ，2：止盈</returns>
        public int OutLine(AUDUSD_1H i, int a, int b)
        {

            if (BapPing == -1)
            {
                if (i.zg >= Target)
                {
                    if (i.zd > Stop)//止赢：最高点触及止盈位，且未触及止损位
                    {
                        Profit = (Target - bid);
                        Result = 2;
                        Out_No = i.num;
                        outer_date = i.date;
                        return 2;
                    }
                    else if (i.zd <= Stop) //平局：止损止盈都触及
                    {
                        Profit = 0;
                        Result = 0;
                        Out_No = i.num;
                        outer_date = i.date;
                        return 0;
                    }
                }
                else if (i.zg < Target)
                {

                    if (i.zd <= Stop) //止损：最低点触及止损，且未触及止盈位
                    {
                        Profit = (Stop - bid);
                        Result = 1;
                        Out_No = i.num;
                        outer_date = i.date;
                        return 1;
                    }
                    else if (i.zd > Stop) //下一次：最高点未触及止赢，且最低点未触及止损位
                    {
                        //设置平保位
                        if (i.sp - bid >= 0.004)
                        {
                            BapPing = bid;
                        }

                        return -1;
                    }
                }
            }
            else if (BapPing != -1)
            {
                if (i.zg >= Target)
                {
                    if (i.zd > BapPing)//止赢：最高点触及止盈位，且未触及止损位
                    {
                        Profit = (Target - bid);
                        Result = 2;
                        Out_No = i.num;
                        outer_date = i.date;
                        return 2;
                    }
                    else if (i.zd <= BapPing) //保平平局：止盈保平都触及
                    {
                        Profit = BapPing - bid;
                        Result = 2;
                        Out_No = i.num;
                        outer_date = i.date;
                        return 2;
                    }
                }
                else if (i.zg < Target)
                {

                    if (i.zd <= BapPing) //保平出场：最低点触及平保，且未触及止盈位
                    {
                        Profit = BapPing - bid;
                        Result = 0;
                        Out_No = i.num;
                        outer_date = i.date;
                        return 0;
                    }
                    else if (i.zd > BapPing) //下一次：最高点未触及止赢，且最低点未触及止损位
                    {
                        //设置平保位
                        if (i.sp - bid >= 0.004)
                        {
                            BapPing = Math.Max(bid, BapPing);
                        }
                        return -1;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// 出场函数--买跌
        /// </summary>
        /// <param name="i"></param>
        /// <returns>1：止损，0 : 平局 ，2：止盈</returns>
        public int OutLine2(AUDUSD_1H i)
        {
            if (i.zd <= Target)
            {
                if (i.zg < Stop)//止赢：最低点触及止赢，且最高点未触及止损位
                {
                    Profit = (bid - Target);
                    Result = 2;
                    Out_No = i.num;
                    outer_date = i.date;
                    return 2;
                }
                else if (i.zg >= Stop) //平局：最低点触及止赢，且最高点且触及止损位
                {
                    Profit = 0;
                    Result = 0;
                    Out_No = i.num;
                    outer_date = i.date;
                    return 0;
                }
            }
            else if (i.zd > Target)
            {
                if (i.zg < Stop) //下一次：最低点未触及止赢，且最高点未触及止损位
                {
                    return -1;
                }
                else if (i.zg >= Stop) //止损：最高点触及止损位
                {
                    Profit = (bid - Stop);
                    Result = 1;
                    Out_No = i.num;
                    outer_date = i.date;
                    return 1;
                }
            }
            return -1;
        }
    }
}

