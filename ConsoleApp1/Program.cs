using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;






namespace ConsoleApp1
{
    class Program
    {

        public static void Test_SD( double zs, double zy)
        {

            //using (DataContext db = new DataContext(@"Data Source=.\SQLEXPRESS; Uid=dellmen;pwd=asd123456789+;database=test;"))
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                System.Diagnostics.Stopwatch oTime = new System.Diagnostics.Stopwatch();
                oTime.Start();
                Table<AUDUSD_1H> Customers = Customers = db.GetTable<AUDUSD_1H>();
                Console.WriteLine("取数函数" + oTime.Elapsed.Minutes + "分" + oTime.Elapsed.Seconds + "秒");
               // List<BID> Bids = new List<BID>();
                //计时，测试

                PL_SD a = new PL_SD();
              //  bool set_num;//接受试探函数返回结果
                foreach (AUDUSD_1H i in Customers)
                {
                    a.Set_Zc2(i, 48, zs, zy, 0.0015);
                  
                }
                Console.WriteLine("试探函数" + oTime.Elapsed.Minutes + "分" + oTime.Elapsed.Seconds + "秒");

                //进场
                foreach (BID_SD b in a.Bids)
                {
                    foreach (AUDUSD_1H i in Customers)//检测是否符合能进场
                    {
                        if (b.Fin < i.num)
                        {
                            bool result;
                            result = b.Enter(i, 48, (double)zs, (double)zy); 
                            if (result)
                            {
                                break;
                            }
                        }
                    }
                }
                Console.WriteLine("进场函数" + oTime.Elapsed.Minutes + "分" + oTime.Elapsed.Seconds + "秒");

                foreach (BID_SD b in a.Bids)
                {
                    if (b.Is_Enter)//只要已进场点
                    {
                        foreach (AUDUSD_1H i in Customers)//检测是否最终结果
                        {
                            if (b.Enter_No < i.num)
                            {
                                int result;
                                result = b.OutLine(i,1,1);
                                if (result != -1) { break; }//有结果则跳出循环
                            }
                        }
                    }
                }
                Console.WriteLine("出场函数" + oTime.Elapsed.Minutes + "分" + oTime.Elapsed.Seconds + "秒");


                foreach (BID_SD b in a.Bids)
                {
                    if (b.Is_Enter && b.Result != -1)
                    {
                        result_detail detail = new result_detail();
                        detail.bz = "EURUSD";
                        detail.tx = "1DSD";
                        detail.Fin = b.Fin;
                        detail.Zero = b.Zero;
                        detail.zc_date = b.zc_date;
                        detail.set_date = b.set_date;
                        detail.enter_date = b.enter_date;
                        detail.outer_date = b.outer_date;
                        detail.bid = b.bid;
                        detail.Zc_high = b.Zc_high;
                        detail.Zc_low = b.Zc_low;
                        detail.zc_mid = b.Zc_mid;
                        detail.Expire = b.Expire;
                        detail.Out_No = b.Out_No;
                        detail.Profit = b.Profit * 10000;
                        db.result_detail.InsertOnSubmit(detail);
                        db.SubmitChanges();
                        Console.WriteLine("产生入场位置序列{0}，支撑位置产生序列{1}，入场位置{2},支撑区域高点{3}，支撑区域低点{4}，离进场位置{5}，出场位置{6},利润点数{7}", b.Fin, b.Zero, b.bid, b.Zc_high, b.Zc_low, b.Expire, b.Out_No, b.Profit);

                    }
                }


                int jc = a.Bids.Where(p => p.Is_Enter).Count();
                int zycs = a.Bids.Where(p => p.Result == 2 && p.Is_Enter).Count();
                int zscs = a.Bids.Where(p => p.Result == 1 && p.Is_Enter).Count();
                int pg = a.Bids.Where(p => p.Result == 0 && p.Is_Enter).Count();
                double ykb, avg_profit;
                if (zscs != 0)
                {
                    ykb = zycs * 1.0 / (zscs + zycs);
                }
                else
                {
                    ykb = zycs * 1.0;
                }
                double profit = 0; //(int)Bids.ToList().Sum(p=>p.Profit) * 10000;
                foreach (var i in a.Bids)
                {
                    profit = profit + i.Profit * 10000;
                }
                if (jc != 0)
                {
                    avg_profit = (int)profit / jc;
                }
                else
                {
                    avg_profit = (int)profit;
                }
                Console.WriteLine("统计函数" + oTime.Elapsed.Minutes + "分" + oTime.Elapsed.Seconds + "秒");

                // Console.WriteLine("共进场{0},止赢{1},止损{2},平局{3}", jc, zycs, zscs, pg);
                Console.WriteLine("共进场{0},止赢{1},止损{2},平局{3},盈亏比{4},利润点数{5}，平均每单利润{6}", jc, zycs, zscs, pg, ykb, profit, avg_profit);


                result r = new result();
                r.bz = "EURUSD";
                r.tx = "1HSDZC2EN3";
                //r.yx = yx;
                //r.st = st;
                r.zs = zs;
                r.zy = zy;
                r.xdzs = jc;
                r.zycs = zycs;
                r.zscs = zscs;
                r.ykb = ykb;
                r.profit = (int)profit;
                r.avg_profit = avg_profit;
                db.result.InsertOnSubmit(r);
                db.SubmitChanges();


                Console.WriteLine("写入函数" + oTime.Elapsed.Minutes + "分" + oTime.Elapsed.Seconds + "秒");
                // Bids.Where(p => p.Result == 2&&p.Is_Enter).Count();

                // Console.WriteLine("共有入场位{0}，实际进场{1}",Bids.Count(),count);
            }
        }

        // public static DataClasses1DataContext db= new DataClasses1DataContext();
        //public static Table<AUDUSD_1H> Customers= Customers = db.GetTable<AUDUSD_1H>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Customers"></param>
        /// <param name="yx">影线长度</param>
        /// <param name="st">成功试探次数</param>
        /// <param name="zs">止损</param>
        /// <param name="zy">止盈</param>
        public static void Test(double yx, int st, double zs, double zy)
        {

            //using (DataContext db = new DataContext(@"Data Source=.\SQLEXPRESS; Uid=dellmen;pwd=asd123456789+;database=test;"))
             using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
            System.Diagnostics.Stopwatch oTime = new System.Diagnostics.Stopwatch();
            oTime.Start();
            Table<AUDUSD_1H> Customers = Customers = db.GetTable<AUDUSD_1H>();
            Console.WriteLine("取数函数" + oTime.Elapsed.Minutes + "分" + oTime.Elapsed.Seconds + "秒");
            List<BID> Bids = new List<BID>();
            //计时，测试
            
            PL a = new PL();
                bool set_num;//接受试探函数返回结果
                foreach (AUDUSD_1H i in Customers)
                {
                    //  a.Zc_high = 0;
                    //   a.Zc_low = 0;
                    a.Is_tag = false;
                    //set_num = a.Set_Num(i);
                    set_num = a.Set_Num(i);
                    if (set_num == false) //失败才需要设置支撑区
                    {
                        a.Set_Zc(i, (double)yx,0.002); //设置影线长度20点作为支撑区域起点
                    }
                    else  //如满足入场位要求，打印
                    {
                        a.Set_Enter(st, 10, (double)zs, i.num, Bids, 0.0015);
                    }
                    // if (a.Zc_high - a.Zc_low != 0)
                    // if (a.Is_tag)//打印支撑区 
                    // {
                    //      Console.WriteLine(i.num);
                    //    Console.ReadKey();
                    // }
                }
            Console.WriteLine("试探函数"+ oTime.Elapsed.Minutes + "分" + oTime.Elapsed.Seconds + "秒");

            //进场
            foreach (BID b in Bids)
                {
                    foreach (AUDUSD_1H i in Customers)//检测是否符合能进场
                    {
                        if (b.Fin < i.num)
                        {
                            bool result;
                            result = b.Enter(i, 48, (double)zs, (double)zy); //反转测试
                            if (result)
                            {
                                break;
                            }
                        }
                    }
                }
            Console.WriteLine("进场函数"+oTime.Elapsed.Minutes + "分" + oTime.Elapsed.Seconds + "秒" );

            foreach (BID b in Bids)
                {
                    if (b.Is_Enter)//只要已进场点
                    {
                        foreach (AUDUSD_1H i in Customers)//检测是否最终结果
                        {
                            if (b.Enter_No < i.num)
                            {
                                int result;
                                result = b.OutLine(i,1);
                                if (result != -1) { break; }//有结果则跳出循环
                            }
                        }
                    }
                }
            Console.WriteLine("出场函数" + oTime.Elapsed.Minutes + "分" + oTime.Elapsed.Seconds + "秒");

                
              foreach (BID b in Bids)
             {
                 if(b.Is_Enter&&b.Result!=-1)
                  {
                        result_detail detail = new result_detail();
                        detail.bz = "AUDUSD";
                        detail.tx = "1H";
                        detail.Fin = b.Fin;
                        detail.Zero = b.Zero;
                        detail.bid = b.bid;
                        detail.Zc_high = b.Zc_high;
                        detail.Zc_low = b.Zc_low;
                        detail.zc_mid = b.Zc_mid;
                        detail.Expire = b.Expire;
                        detail.Out_No = b.Out_No;
                        detail.outer_date = b.Outer_date;
                        detail.outer_time = b.Outer_time;
                        detail.enter_date = b.Enter_date;
                        detail.enter_time = b.Enter_time;
                        detail.out_bid = b.Outer_bid;
                        
                        
                        detail.Profit = b.Profit * 10000;
                        db.result_detail.InsertOnSubmit(detail);
                        db.SubmitChanges();
                        Console.WriteLine("产生入场位置序列{0}，支撑位置产生序列{1}，入场位置{2},支撑区域高点{3}，支撑区域低点{4}，离进场位置{5}，出场位置{6},利润点数{7}", b.Fin, b.Zero, b.bid,b.Zc_high,b.Zc_low,b.Expire,b.Out_No,b.Profit);
                  
                   }
              }
              

                /***修改止盈算法***/
                int jc = Bids.Where(p => p.Is_Enter).Count();
                int zycs = Bids.Where(p => p.Result == 2 && p.Is_Enter&&p.Profit > 0).Count();
                int pgcs = Bids.Where(p => p.Result == 2 && p.Is_Enter&&p.Profit==0).Count();//平局次数
                int zscs = Bids.Where(p => p.Result == 1 && p.Is_Enter).Count();
                int pg = Bids.Where(p => p.Result == 0 && p.Is_Enter).Count();
                double ykb, avg_profit;
                if (zscs != 0)
                {
                    ykb = zycs * 1.0 / (zscs + zycs);
                }
                else
                {
                    ykb = zycs * 1.0;
                }
                double profit = 0; //(int)Bids.ToList().Sum(p=>p.Profit) * 10000;
                foreach (var i in Bids)
                {
                    profit = profit + i.Profit * 10000;
                }
                if (jc != 0)
                {
                    avg_profit = (int)profit / jc;
                }
                else
                {
                    avg_profit = (int)profit;
                }
                Console.WriteLine("统计函数" + oTime.Elapsed.Minutes + "分" + oTime.Elapsed.Seconds + "秒");

                // Console.WriteLine("共进场{0},止赢{1},止损{2},平局{3}", jc, zycs, zscs, pg);
                Console.WriteLine("共进场{0},止赢{1},止损{2},平局{3},盈亏比{4},利润点数{5}，平均每单利润{6}", jc, zycs, zscs, pg, ykb, profit, avg_profit);


                result r = new result();
                r.bz = "AUDUSD";
                r.tx = "1H";
                r.yx = yx;
                r.st = st;
                r.zs = zs;
                r.zy = zy;
                r.xdzs = jc;
                r.zycs = zycs;
                r.zscs = zscs;
                r.ykb = ykb;
                r.profit = (int)profit;
                r.avg_profit = avg_profit;
                db.result.InsertOnSubmit(r);
                db.SubmitChanges();
                

            Console.WriteLine("写入函数" + oTime.Elapsed.Minutes + "分" + oTime.Elapsed.Seconds + "秒");
            // Bids.Where(p => p.Result == 2&&p.Is_Enter).Count();

            // Console.WriteLine("共有入场位{0}，实际进场{1}",Bids.Count(),count);
              }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Customers"></param>
        /// <param name="yx">影线长度</param>
        /// <param name="st">成功试探次数</param>
        /// <param name="zs">止损</param>
        /// <param name="zy">止盈</param>
        public static void Test_SAM( double zs, double zy)
        {

            //using (DataContext db = new DataContext(@"Data Source=.\SQLEXPRESS; Uid=dellmen;pwd=asd123456789+;database=test;"))
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                System.Diagnostics.Stopwatch oTime = new System.Diagnostics.Stopwatch();
                oTime.Start();
                Table<AUDUSD_1H> Customers = Customers = db.GetTable<AUDUSD_1H>();
                Console.WriteLine("取数函数" + oTime.Elapsed.Minutes + "分" + oTime.Elapsed.Seconds + "秒");
                List<BID> Bids = new List<BID>();
                //计时，测试

              
                bool set_num;//接受试探函数返回结果
                foreach (AUDUSD_1H i in Customers)
                {
                    BID b = new BID();
                    set_num = b.Enter_SAM(i, Customers, zs, zy);
                    if (set_num == true)
                    {
                        Bids.Add(b);
                    }
                }
                
                Console.WriteLine("进场函数" + oTime.Elapsed.Minutes + "分" + oTime.Elapsed.Seconds + "秒");

                foreach (BID b in Bids)
                {
                    if (b.Is_Enter)//只要已进场点
                    {
                        foreach (AUDUSD_1H i in Customers)//检测是否最终结果
                        {
                            if (b.Enter_No < i.num)
                            {
                                int result;
                                result = b.OutLine(i, 1);
                                if (result != -1) { break; }//有结果则跳出循环
                            }
                        }
                    }
                }
                Console.WriteLine("出场函数" + oTime.Elapsed.Minutes + "分" + oTime.Elapsed.Seconds + "秒");

                /*
              foreach (BID b in Bids)
             {
                 if(b.Is_Enter&&b.Result!=-1)
                  {
                        result_detail detail = new result_detail();
                        detail.bz = "EURAUD";
                        detail.tx = "1DZC3";
                        detail.Fin = b.Fin;
                        detail.Zero = b.Zero;
                        detail.bid = b.bid;
                        detail.Zc_high = b.Zc_high;
                        detail.Zc_low = b.Zc_low;
                        detail.zc_mid = b.Zc_mid;
                        detail.Expire = b.Expire;
                        detail.Out_No = b.Out_No;
                        detail.Profit = b.Profit * 10000;
                        db.result_detail.InsertOnSubmit(detail);
                        db.SubmitChanges();
                        Console.WriteLine("产生入场位置序列{0}，支撑位置产生序列{1}，入场位置{2},支撑区域高点{3}，支撑区域低点{4}，离进场位置{5}，出场位置{6},利润点数{7}", b.Fin, b.Zero, b.bid,b.Zc_high,b.Zc_low,b.Expire,b.Out_No,b.Profit);
                  
                   }
              }
              */

                int jc = Bids.Where(p => p.Is_Enter).Count();
                int zycs = Bids.Where(p => p.Result == 2 && p.Is_Enter).Count();
                int zscs = Bids.Where(p => p.Result == 1 && p.Is_Enter).Count();
                int pg = Bids.Where(p => p.Result == 0 && p.Is_Enter).Count();
                double ykb, avg_profit;
                if (zscs != 0)
                {
                    ykb = zycs * 1.0 / (zscs + zycs);
                }
                else
                {
                    ykb = zycs * 1.0;
                }
                double profit = 0; //(int)Bids.ToList().Sum(p=>p.Profit) * 10000;
                foreach (var i in Bids)
                {
                    profit = profit + i.Profit * 10000;
                }
                if (jc != 0)
                {
                    avg_profit = (int)profit / jc;
                }
                else
                {
                    avg_profit = (int)profit;
                }
                Console.WriteLine("统计函数" + oTime.Elapsed.Minutes + "分" + oTime.Elapsed.Seconds + "秒");

                // Console.WriteLine("共进场{0},止赢{1},止损{2},平局{3}", jc, zycs, zscs, pg);
                Console.WriteLine("共进场{0},止赢{1},止损{2},平局{3},盈亏比{4},利润点数{5}，平均每单利润{6}", jc, zycs, zscs, pg, ykb, profit, avg_profit);


                result r = new result();
                r.bz = "EURCAD";
                r.tx = "1DSAM";
               // r.yx = yx;
               // r.st = st;
                r.zs = zs;
                r.zy = zy;
                r.xdzs = jc;
                r.zycs = zycs;
                r.zscs = zscs;
                r.ykb = ykb;
                r.profit = (int)profit;
                r.avg_profit = avg_profit;
                db.result.InsertOnSubmit(r);
                db.SubmitChanges();


                Console.WriteLine("写入函数" + oTime.Elapsed.Minutes + "分" + oTime.Elapsed.Seconds + "秒");
                // Bids.Where(p => p.Result == 2&&p.Is_Enter).Count();

                // Console.WriteLine("共有入场位{0}，实际进场{1}",Bids.Count(),count);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            



            //List<double> yx = new List<double> { 0.0015, 0.002, 0.0025, 0.003, 0.004 };
            //List<int> st = new List<int> { 3, 4, 5 };
            //List<double> zs = new List<double> { 0.0025, 0.0030, 0.0035, 0.0040 };
            //List<double> zy = new List<double> { 0.0030, 0.006, 0.0070, 0.0080, 0.009, 0.01, 0.012, 0.015, 0.018, 0.02, 0.022, 0.025, 0.03, 0.035, 0.04 };



            List<double> yx = new List<double> { 0.0015, 0.002, 0.0025 };
            List<int> st = new List<int> { 2,3,5,7 };
            List<double> zs = new List<double> { 0.003 };
            List<double> zy = new List<double> { 0.080 };

            ////List<double> zs = new List<double> {   0.0030 };
            ////List<double> zy = new List<double> {  0.006 };


            ////// 一般结果测试
            foreach (var a in yx)
            {
                foreach (var b in st)
                {
                    foreach (var c in zs)
                    {
                        Parallel.ForEach(zy, d => { Test(a, b, c, d); }); //并行处理 
                        //foreach (var d in zy) { Test(a, b, c, d); } //一般处理
                    }
                }
            }

            ////时段测试

            //foreach (var c in zs)
            //{
            //    Parallel.ForEach(zy, d => { Test_SD(c, d); }); //并行处理 
            //    //foreach (var d in zy) { Test_SD(c, d); } //一般处理
            //}



            ////均线结果测试
            ////foreach (var c in zs)
            ////{
            ////    Parallel.ForEach(zy, d => { Test_SAM(c, d); }); //并行处理 
            ////}








            ////using (DataClasses1DataContext db = new DataClasses1DataContext())
            ////{
            ////    Table<AUDUSD_1H> c = db.GetTable<AUDUSD_1H>();

            ////    Algorithm al = new Algorithm();
            ////    al.Add_SMA(c);


            ////    // Console.ReadKey();
            ////}


        }




    }


    class BID //入场位类
    {
        public BID()
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

        public string Zc_date { get; set; } //支撑位日期
        public string Enter_date { get; set; } //入场日期
        public string Outer_date {  get;set;}  //出场日期

        public string Zc_time { get; set; } //支撑位时间
        public string Enter_time { get; set; } //入场时间
        public string Outer_time { get; set; }  //出场时间
        public double Outer_bid { get; set; }  //出场位  

        /**以下是趋势线坐标属性 **/
        public double Zc_x { get; set; } //横坐标
        public double Zc_y { get; set; } //纵坐标

        public double Now_a { get; set; } //近期趋势线a常数  y=ax+b
        public double Now_b { get; set; } //近期趋势线a常数  y=ax+b
        public double Last_a { get; set; } //远期趋势线a常数  y=ax+b
        public double Last_b { get; set; } //远期趋势线a常数  y=ax+b
        public double Now_rate { get; set; } //近期趋势线斜率
        public double Last_rate { get; set; } //远期趋势线斜率

       
        

        /// <summary>
        /// 设置近期趋势线
        /// </summary>
        /// <param name="i"></param>
        /// <param name="b">蜡烛线周期</param>
        public void Set_Now(Table<AUDUSD_1H> i,AUDUSD_1H now, int b) //先测试10天趋势
        {
            List<AUDUSD_1H> l = i.Where(v => v.num < now.num&&v.num>=(now.num-b)).OrderByDescending(c=>c.sp).ToList();  //取10天内的点
            //foreach(var )
        }

        /// <summary>
        /// 均线下单函数
        /// </summary>
        /// <param name="i"></param>
        /// <param name="expire">入场位有效蜡烛</param>
        public bool Enter_SAM(AUDUSD_1H i, Table<AUDUSD_1H> l, double zs, double zy)
        {
            //if (i.sam_5 >= i.sam_10 && i.sam_10 >= i.sam_20 &&i.zd<=i.sam_20)  //增加均线判断           
            //{
            //    Is_Enter = true;
            //    Enter_No = i.num;
            //    bid =(double) i.sam_20;
            //    Target = bid + zy;
            //    Stop = bid - zs;
            //    return true;
            //}
            
            return false;
        }

        /// <summary>
        /// 均线出场函数
        /// </summary>
        /// <param name="i"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public int OutLine_SAM(AUDUSD_1H i, Table<AUDUSD_1H> l)
        {
            AUDUSD_1H last = l.Where(c => c.num == Enter_No).First();
            if (last.zd <= Stop)
            {
                Profit = (Stop - bid);
                Result = 1;
                Out_No = Enter_No;
                return 1;
            }


            if (i.zg >= Target)
            {
                if (i.zd > Stop)//止赢：最高点触及止盈位，且未触及止损位
                {
                    Profit = (Target - bid);
                    Result = 2;
                    Out_No = i.num;
                    return 2;
                }
                else if (i.zd <= Stop) //平局：止损止盈都触及
                {
                    Profit = 0;
                    Result = 0;
                    Out_No = i.num;
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
                    Result = 1;
                    Out_No = i.num;
                    return 1;
                }
            }
            return -1;
        }


        /// <summary>
        /// 下单函数
        /// </summary>
        /// <param name="i"></param>
        /// <param name="expire">入场位有效蜡烛</param>
        public bool Enter(AUDUSD_1H i,int expire,double zs,double zy)
        {
            Expire = Expire + 1;//
            if (Expire< expire)
            {
                //if (i.zd <= bid&&i.kp>=i.sam_5 && i.sam_5 >= i.sam_10 && i.sam_10 >= i.sam_20)  //增加均线判断
                 if (i.zd <= bid)   //最低点在买入点以下，可买入
                {

                    Is_Enter = true;
                    Enter_No = i.num;
                    Target = bid + zy;
                    Stop = bid - zs;
                    this.Enter_date = i.date;
                    this.Enter_time = i.time;
                    return true;
                }
            }
            return false;
        }

        public bool Enter_SAM(AUDUSD_1H i, int expire, double zs, double zy)
        {
            Expire = Expire + 1;//
            //if (Expire < expire)
            //{
            //    if (i.zd <= bid&&i.kp>= i.sam_20)  //增加均线判断
            //    //if (i.zd <= bid)   //最低点在买入点以下，可买入
            //    {

            //        Is_Enter = true;
            //        Enter_No = i.num;
            //        Target = bid + zy;
            //        Stop = bid - zs;
            //        return true;
            //    }
            //}
            return false;
        }

        /// <summary>
        /// 下单反转测试
        /// </summary>
        /// <param name="i"></param>
        /// <param name="expire"></param>
        /// <param name="zs"></param>
        /// <param name="zy"></param>
        /// <returns></returns>
        public bool Enter(AUDUSD_1H i, int expire, double zs, double zy,int test)
        {
            Expire = Expire + 1;//
            if (Expire < expire)
            {
                //if (i.zd <= bid && i.kp >= i.sam_5 && i.kp >= i.sam_10 && i.kp >= i.sam_20)  //增加均线判断
                 if (i.zd <= bid)   //最低点在买入点以下，可买入
                {

                    Is_Enter = true;
                    Enter_No = i.num;
                    Target = bid - zy;
                    Stop = bid + zs;
                    this.Enter_date = i.date;
                    this.Enter_time = i.time;

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
                    Outer_date = i.date;
                    Outer_time = i.time;
                    Outer_bid = Target;
                    return 2;
                }
                else if (i.zd <= Stop) //平局：止损止盈都触及
                {
                    Profit = 0;
                    Result = 0;
                    Out_No = i.num;
                    Outer_date = i.date;
                    Outer_time = i.time;
                    Outer_bid = bid;
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
                    Result = 1;
                    Out_No = i.num;
                    Outer_date = i.date;
                    Outer_time = i.time;
                    Outer_bid = Stop;
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
        public int OutLine(AUDUSD_1H i,int a)
        {

            if(BapPing==-1)
            { 
                if (i.zg >= Target)
                {
                    if (i.zd > Stop)//止赢：最高点触及止盈位，且未触及止损位
                    {
                        Profit = (Target - bid);
                        Result = 2;
                        Out_No = i.num;
                        Outer_date = i.date;
                        Outer_time = i.time;
                        Outer_bid = Target;
                        return 2;
                    }
                    else if (i.zd <= Stop) //平局：止损止盈都触及
                    {
                        Profit = 0;
                        Result = 0;
                        Out_No = i.num;
                        Outer_date = i.date;
                        Outer_time = i.time;
                        Outer_bid = bid;
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
                        Outer_date = i.date;
                        Outer_time = i.time;
                        Outer_bid = Stop;
                        return 1;
                    }
                    else if (i.zd > Stop) //下一次：最高点未触及止赢，且最低点未触及止损位
                    {
                        //设置平保位
                        if (i.sp - bid>= 0.004)
                        {
                            BapPing=bid;
                        }
                        if (i.sp - bid >= 0.008)
                        {
                            BapPing = bid+ 0.004;
                        }
                        if (i.sp - bid >= 0.012)
                        {
                            BapPing = bid+ 0.008;
                        }
                        if (i.sp - bid >= 0.016)
                        {
                            BapPing = bid+ 0.012;
                        }
                        if (i.sp - bid >= 0.02)
                        {
                            BapPing = bid+ 0.016;
                        }
                        if (i.sp - bid >= 0.024)
                        {
                            BapPing = bid+ 0.02;
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
                        Outer_date = i.date;
                        Outer_time = i.time;
                        Outer_bid = Target;
                        return 2;
                    }
                    else if (i.zd <= BapPing) //保平平局：止盈保平都触及
                    {
                        Profit = BapPing-bid;
                        Result = 2;
                        Out_No = i.num;
                        Outer_date = i.date;
                        Outer_time = i.time;
                        Outer_bid = BapPing;
                        if (Profit==0)
                            { return 0; }

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
                        Outer_date = i.date;
                        Outer_time = i.time;
                        Outer_bid = BapPing;
                        if (Profit == 0)
                        { return 0; }

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
        public int OutLine(AUDUSD_1H i, int a,int b)
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
                        Outer_date = i.date;
                        Outer_time = i.time;
                        Outer_bid = Target;
                        return 2;
                    }
                    else if (i.zd <= Stop) //平局：止损止盈都触及
                    {
                        Profit = 0;
                        Result = 0;
                        Out_No = i.num;
                        Outer_date = i.date;
                        Outer_time = i.time;
                        Outer_bid = bid;
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
                        Outer_date = i.date;
                        Outer_time = i.time;
                        Outer_bid = Stop;
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
                        Outer_date = i.date;
                        Outer_time = i.time;
                        Outer_bid = Target;
                        return 2;
                    }
                    else if (i.zd <= BapPing) //保平平局：止盈保平都触及
                    {
                        Profit = BapPing - bid;
                        Result = 2;
                        Out_No = i.num;
                        Outer_date = i.date;
                        Outer_time = i.time;
                        Outer_bid = BapPing;
                        if (Profit == 0)
                        { return 0; }

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
                        Outer_date = i.date;
                        Outer_time = i.time;
                        Outer_bid = BapPing;
                        if (Profit == 0)
                        { return 0; }

                        return 2;
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
                    Outer_date = i.date;
                    Outer_time = i.time;
                    Outer_bid = Target;
                    return 2;
                }
                else if (i.zg >= Stop) //平局：最低点触及止赢，且最高点且触及止损位
                {
                    Profit = 0;
                    Result = 0;
                    Out_No = i.num;
                    Outer_date = i.date;
                    Outer_time = i.time;
                    Outer_bid = bid;
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
                    Outer_date = i.date;
                    Outer_time = i.time;
                    Outer_bid = Stop;
                    return 1;
                }
            }
            return -1;
        }
    }

    class PL
    {
        //初始化参数
        public PL()
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
        }

        public double Zc_low { get; set; }  // 支撑区域底部
        public double Zc_high { get; set; } // 支撑区域顶部
        public double Zc_mid { get; set; } // 支撑区域中位线
        public int Num { get; set; } //试探成功次数
        public int Yz { get; set; } //一致性，0：没有 1：早盘 2：欧盘 3：美盘
        public int No { get; set; }//支撑区确立后蜡烛条数
        public bool Is_PL { get; set; } //是否可以试探

        public bool Is_tag { get; set; } //是否为目标

        public double BID { get; set; }  //入场位 
        public int Zero { get; set; }//支撑区域成立的蜡烛位置


        /// <summary>
        /// 设置支撑区
        /// </summary>
        public void Set_Zc(AUDUSD_1H now, double area)  
        {

            //影线长度大于指定数，且大于K线实体
            if ((Math.Min((decimal)now.kp,(decimal) now.sp) -  (decimal)now.zd) >= Math.Abs((decimal)(now.kp - now.sp)) && (Math.Min((double)now.kp, (double)now.sp) - now.zd)>=area
                )
            {
                Zc_low =(double) now.zd;
                Zc_high = (double)(Math.Min((decimal)now.kp, (decimal)now.sp));
                Zc_mid = (Zc_high + Zc_low) /2;
                Is_PL = true;
                Is_tag = true;
                Zero =now.num;
            }


        }

        /// <summary>
        /// 设置支撑区，
        /// </summary>
        /// <param name="now"></param>
        /// <param name="area"></param>
        /// <param name="Mul">影线长度是实体的长度</param>
        public void Set_Zc(AUDUSD_1H now, double area,double Mul)
        {
            
                //影线长度大于指定数，且大于K线实体
                if (Math.Abs((double)(now.kp - now.sp)) <=Mul&& (Math.Min((double)now.kp, (double)now.sp) - now.zd) >= area )
            {
                Zc_low = (double)now.zd;
                Zc_high = (double)(Math.Min((decimal)now.kp, (decimal)now.sp));
                Zc_mid = (Zc_high + Zc_low) / 2;
                Is_PL = true;
                Is_tag = true;
                Zero = now.num;
            }


        }


        /// <summary>
        /// 设置支撑区，
        /// </summary>
        /// <param name="now"></param>
        /// <param name="area"></param>
        /// <param name="Mul">影线长度是实体的长度</param>
        public void Set_Zc2(AUDUSD_1H now, double area, double Mul, Table<AUDUSD_1H> c)
        {
            /*
            //影线长度大于指定数，且大于K线实体
            if (Math.Abs((double)(now.kp - now.sp)) <= Mul && (Math.Min((double)now.kp, (double)now.sp) - now.zd) >= area)
            {
                Zc_low = (double)now.zd;
                Zc_high = (double)(Math.Min((decimal)now.kp, (decimal)now.sp));
                Zc_mid = (Zc_high + Zc_low) / 2;
                Is_PL = true;
                Is_tag = true;
                Zero = now.num;
                return;
            }
            */
            if (now.num>2)//对面前几天的K线情况，寻找反向K
            {
                AUDUSD_1H a = c.First(v => v.num == (now.num - 1));
                if ((a.kp - a.sp) >= 0.002 && now.sp >= a.kp)
                {
                    Zc_low = (double)now.zd;
                    Zc_high = (double)now.sp;
                    Zc_mid = (Zc_high + Zc_low) / 2;
                    Is_PL = true;
                    Is_tag = true;
                    Zero = now.num;
                    return;
                }
            }


        }

        /// <summary>
        /// 设置支撑区，
        /// </summary>
        /// <param name="now"></param>
        /// <param name="area"></param>
        /// <param name="Mul">影线长度是实体的长度</param>
        public void Set_Zc3(AUDUSD_1H now, double area, double Mul)
        {

            //影线长度大于指定数
            if ( (Math.Min((double)now.kp, (double)now.sp) - now.zd) >= area)
            {
                Zc_low = (double)now.zd;
                Zc_high = (double)(Math.Min((decimal)now.kp, (decimal)now.sp));
                Zc_mid = (Zc_high + Zc_low) / 2;
                Is_PL = true;
                Is_tag = true;
                Zero = now.num;
            }


        }


        /// <summary>
        /// 设置支撑区，
        /// </summary>
        /// <param name="now"></param>
        /// <param name="area"></param>
        /// <param name="Mul">影线长度是实体的长度</param>
        public void Set_Zc4(AUDUSD_1H now, double area, double Mul)
        {

            //影线长度大于指定数
           
                Zc_low = (double)now.zd;
                Zc_high = (double)(Math.Min((decimal)now.kp, (decimal)now.sp));
                Zc_mid = (Zc_high + Zc_low) / 2;
                Is_PL = true;
                Is_tag = true;
                Zero = now.num;
            


        }

        /// <summary>
        /// 设置支撑区(时段算法)
        /// </summary>
        /// <param name="now"></param>
        /// <param name="area"></param>
        /// <param name="Mul">影线长度是实体的长度</param>
        public void Set_Zc_SD(AUDUSD_1H now, double area, double Mul)
        {

            //影线长度大于指定数

            Zc_low = (double)now.zd;
            Zc_high = (double)(Math.Min((decimal)now.kp, (decimal)now.sp));
            Zc_mid = (Zc_high + Zc_low) / 2;
            Is_PL = true;
            Is_tag = true;
            Zero = now.num;
        }

        /// <summary>
        /// 试探函数，返回true则该时间无需调用支撑函数
        /// </summary>
        /// <param name="now"></param>
        /// <param name="no"></param>
        public bool Set_Num(AUDUSD_1H now) //问题，试探函数无效
        {
            if (Is_PL)
            {
                if (now.sp > Zc_low) //收盘在支撑区上
                {

                    if (now.zd >= Zc_low
                       &&now.zd<=Zc_high
                        ) //试探成功（最低点支撑区内）
                    {
                        if (now.sp >= Zc_mid)//收盘在1/2支撑上
                        {
                            Num = Num + 1;//试探成功+1
                            No = No + 1;//蜡烛时点+1

                            //修改支撑最高点(可选)
                        }
                        else if (now.sp < Zc_mid)//试探收盘位在支撑区域1/2以下
                        {
                            Num = Num + 0;//试探成功+1
                            No = No + 1;//蜡烛时点+1
                            //修改支撑最高点(可选)

                        }
                   

                    }

                    else if ( now.zd > Zc_high) //未进行试探（最低点在支撑区域以上）
                   {
                        
                        No = No + 1;//蜡烛时点+1
                   }

                    else if(now.zd < Zc_low) //试探半成功（最低点支撑区下）
                    {
                        if (now.sp >= Zc_mid)//收盘在1/2区域上，试探成功
                        {
                            Num = Num + 1;//试探成功+1
                            No = No + 1;//蜡烛时点+1
                            Zc_low = (double)now.zd;//修改支撑区域最低点
                            Zc_mid = (Zc_low + Zc_high) / 2;
                            //修改支撑最高点(可选)
                        }
                        else if (now.sp < Zc_mid)//试探收盘位在支撑区域1/2以下，试探失败
                        {
                            Init();
                            return false;
                        }
                    }
                    return true; 
                 
                }
                else if (now.sp <= Zc_low)     //收盘在支撑区下  ，试探失败，全部初始化  
                {
                    Init();
                    return false;
                }
            }
         

            else  //不具备试探条件
            {
               
                return false;
            }
            return false;
        }

        /// <summary>
        /// 试探函数，返回true则该时间无需调用支撑函数
        /// </summary>
        /// <param name="now"></param>
        /// <param name="no"></param>
        public bool Set_Num3(AUDUSD_1H now, decimal yx) //问题，试探函数无效
        {
            if (Is_PL)
            {
                if (now.sp > Zc_low) //收盘在支撑区上
                {

                    if (now.zd >= Zc_low
                       && now.zd <= Zc_high
                        ) //试探成功（最低点支撑区内）
                    {
                        if (now.sp >= Zc_mid)//收盘在1/2支撑上
                        {
                            Num = Num + 1;//试探成功+1
                            No = No + 1;//蜡烛时点+1

                            //修改支撑最高点(可选)
                        }
                        else if (now.sp < Zc_mid)//试探收盘位在支撑区域1/2以下
                        {
                            Num = Num + 0;//试探成功+1
                            No = No + 1;//蜡烛时点+1
                            //修改支撑最高点(可选)

                        }


                    }

                    else if (now.zd > Zc_high) //未进行试探（最低点在支撑区域以上）
                    {

                        No = No + 1;//蜡烛时点+1
                    }

                    else if (now.zd < Zc_low) //试探半成功（最低点支撑区下）
                    {
                        Init();
                        Set_Zc(now, (double)yx, 0.002);
                    }
                    return true;

                }
                else if (now.sp <= Zc_low)     //收盘在支撑区下  ，试探失败，全部初始化  
                {
                    Init();
                    return false;
                }
            }


            else  //不具备试探条件
            {

                return false;
            }
            return false;
        }



        /// <summary>
        /// 试探函数，返回true则该时间无需调用支撑函数
        /// </summary>
        /// <param name="now"></param>
        /// <param name="no"></param>
        public bool Set_Num2(AUDUSD_1H now) //问题，试探函数无效
        {
            if (Is_PL)
            {
                if (now.sp > Zc_low) //收盘在支撑区上
                {

                    if (now.zd >= Zc_low
                       && now.zd <= Zc_high
                        ) //试探成功（最低点支撑区内）
                    {
                        if (now.sp >= Zc_high)//收盘在支撑位上
                        {
                            Num = Num + 1;//试探成功+1
                            No = No + 1;//蜡烛时点+1

                            //修改支撑最高点(可选)
                        }
                        else if (now.sp >= Zc_mid)//收盘在支撑位上
                        {
                            Num = Num + 1;//试探成功+1
                            No = No + 1;//蜡烛时点+1

                            //修改支撑最高点(可选)
                        }
                        else if (now.sp < Zc_mid)//试探收盘位在支撑区域1/2以下
                        {
                            Init();

                        }


                    }

                    else if (now.zd > Zc_high) //试探成功（最低点在支撑区域以上）
                    {
                        Num = Num + 0;//试探成功+1
                        No = No + 1;//蜡烛时点+1
                    }

                    else if (now.zd < Zc_low) //试探失败（最低点支撑区下） 将初始化改成将该点作为新的试探原点
                    {
                        Init();
                        
                    }
                    return true;

                }
                else if (now.sp <= Zc_low)     //收盘在支撑区下  ，试探失败，全部初始化  
                {
                    Init();
                    return false;
                }
            }


            else  //不具备试探条件
            {

                return false;
            }
            return false;
        }

        /// <summary>
        /// 设置入场位
        /// </summary>
        /// <param name="num">设置试探成功次数</param>
        /// <param name="seq">设置试探蜡烛次数要求</param>
        /// <param name="zs">设置止损点数要求</param>
        /// <param name="zs">入场位序列</param>
        /// <param name="l">入场位类列表</param>
        public void Set_Enter(int num,int seq,double zs,int no,List<BID> l,double beyond)
        {
            if (Num == num) //试探限制要求，时区要求暂未实现
            {
                //设置入场位置=支撑最低点-超出点+zs限制
                BID = Math.Min( Zc_low - beyond + zs,Zc_high); //考虑踏空次数，看是否需要取两者大值 
            //    Console.WriteLine("产生入场位置序列{0}，支撑位置产生序列{1}，入场位置{2}",no,Zero,BID);
                BID b = new BID();
                b.Zc_high = Zc_high;
                b.Zc_low = Zc_low;
                b.Zc_mid = Zc_mid;
                b.Zero = Zero;
                
                b.Fin = no;
                b.bid = BID;
                b.No = No;
                l.Add(b);
                //清零，重新开始
                Init();
            }
            if (No==seq)//到达蜡烛图次数限制
            {
                Init();
            }
            return;
        }

        /// <summary>
        /// 设置入场位
        /// </summary>
        /// <param name="num">设置试探成功次数</param>
        /// <param name="seq">设置试探蜡烛次数要求</param>
        /// <param name="zs">设置止损点数要求</param>
        /// <param name="zs">入场位序列</param>
        /// <param name="l">入场位类列表</param>
        public void Set_Enter2(int num, int seq, double zs, int no, List<BID> l, double beyond)
        {
            if (Num == num) //试探限制要求，时区要求暂未实现
            {
                //设置入场位置=支撑最低点-超出点+zs限制
                BID = Math.Min(Zc_low - beyond + zs, Zc_mid); //考虑踏空次数，看是否需要取两者大值 
                                                               //    Console.WriteLine("产生入场位置序列{0}，支撑位置产生序列{1}，入场位置{2}",no,Zero,BID);
                BID b = new BID();
                b.Zc_high = Zc_high;
                b.Zc_low = Zc_low;
                b.Zc_mid = Zc_mid;
                b.Zero = Zero;
                b.Fin = no;
                b.bid = BID;
                b.No = No;
                l.Add(b);
                //清零，重新开始
                Init();
            }
            if (No == seq)//到达蜡烛图次数限制
            {
                Init();
            }
            return;
        }
    }
}
