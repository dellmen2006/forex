using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq;
using System.Text;
using System.Collections;

namespace ConsoleApp1
{

    public class Algorithm
    {
        //时区判断集合
        public Hashtable Sds { get; set; }

        public Algorithm()
        {
            Sds = new Hashtable();
            Sds.Add("00:00", 0);
            Sds.Add("01:00", 0);
            Sds.Add("02:00", 0);
            Sds.Add("03:00", 0);
            Sds.Add("04:00", 0);
            Sds.Add("05:00", 0);
            Sds.Add("06:00", 0);
            Sds.Add("07:00", 0);
            Sds.Add("08:00", 0);
            Sds.Add("09:00", 1);
            Sds.Add("10:00", 1);
            Sds.Add("11:00", 1);
            Sds.Add("12:00", 1);
            Sds.Add("13:00", 1);
            Sds.Add("14:00", 1);
            Sds.Add("15:00", 2);
            Sds.Add("16:00", 2);
            Sds.Add("17:00", 2);
            Sds.Add("18:00", 2);
            Sds.Add("19:00", 2);
            Sds.Add("20:00", 2);
            Sds.Add("21:00", 2);
            Sds.Add("22:00", 2);
            Sds.Add("23:00", 2);

        }

        /// <summary>
        /// 移动平均线及时段计算类 ，时段： 0-亚盘 1-欧盘 2-美盘
        /// </summary>
        /// <typeparam name="T">源计算类</typeparam>
        /// <typeparam name="U">目标计算类</typeparam>
        /// <param name="t"></param>
        /// <param name="u"></param>
        public void Add_SMA(Table<EURUSD_1H> source)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                
                //  target.InsertOnSubmit();
                //db.EURUSD_1H_NEW.InsertAllOnSubmit(target);


                foreach (var i in source)
                {
                    EURUSD_1H_NEW u = new EURUSD_1H_NEW();
                    u.cjl = i.cjl;
                    u.date = i.date;
                    u.time = i.time;
                    u.kp = i.kp;
                    u.sp = i.sp;
                    u.zd = i.zd;
                    u.zg = i.zg;
                    //计算时段
                    foreach (var j in Sds)
                    {
                        u.sd = (int)Sds[i.time];
                    }
                    //计算平均线
                    //ma5
                    if (i.num >= 5)
                    {
                        
                        u.sam_5 = (source.Where(k => k.num == (i.num - 1)).First<EURUSD_1H>().sp+
                            source.Where(k => k.num == (i.num - 2)).First<EURUSD_1H>().sp+
                            source.Where(k => k.num == (i.num - 3)).First<EURUSD_1H>().sp+
                            source.Where(k => k.num == (i.num - 4)).First<EURUSD_1H>().sp+
                            source.Where(k => k.num == (i.num )).First<EURUSD_1H>().sp)/5;
                            
                
                    }
                    
                    if (i.num >= 10)
                    {

                        u.sam_10 = (source.Where(k => k.num == (i.num - 1)).First<EURUSD_1H>().sp + 
                            source.Where(k => k.num == (i.num - 2)).First<EURUSD_1H>().sp +
                            source.Where(k => k.num == (i.num - 3)).First<EURUSD_1H>().sp +
                            source.Where(k => k.num == (i.num - 4)).First<EURUSD_1H>().sp + 
                            source.Where(k => k.num == (i.num - 5)).First<EURUSD_1H>().sp + 
                            source.Where(k => k.num == (i.num - 6)).First<EURUSD_1H>().sp + 
                            source.Where(k => k.num == (i.num - 7)).First<EURUSD_1H>().sp +
                            source.Where(k => k.num == (i.num - 8)).First<EURUSD_1H>().sp + 
                            source.Where(k => k.num == (i.num - 9)).First<EURUSD_1H>().sp+
                             source.Where(k => k.num == (i.num )).First<EURUSD_1H>().sp)/10;
                    }

                    if (i.num >= 20)
                    {

                        u.sam_20 = (source.Where(k => k.num == (i.num - 1)).First<EURUSD_1H>().sp +
                            source.Where(k => k.num == (i.num - 2)).First<EURUSD_1H>().sp +
                            source.Where(k => k.num == (i.num - 3)).First<EURUSD_1H>().sp +
                            source.Where(k => k.num == (i.num - 4)).First<EURUSD_1H>().sp +
                            source.Where(k => k.num == (i.num - 5)).First<EURUSD_1H>().sp +
                            source.Where(k => k.num == (i.num - 6)).First<EURUSD_1H>().sp +
                            source.Where(k => k.num == (i.num - 7)).First<EURUSD_1H>().sp +
                            source.Where(k => k.num == (i.num - 8)).First<EURUSD_1H>().sp +
                            source.Where(k => k.num == (i.num - 9)).First<EURUSD_1H>().sp +
                             source.Where(k => k.num == (i.num - 10)).First<EURUSD_1H>().sp+
                             source.Where(k => k.num == (i.num - 11)).First<EURUSD_1H>().sp +
                            source.Where(k => k.num == (i.num - 12)).First<EURUSD_1H>().sp +
                            source.Where(k => k.num == (i.num - 13)).First<EURUSD_1H>().sp +
                            source.Where(k => k.num == (i.num - 14)).First<EURUSD_1H>().sp +
                            source.Where(k => k.num == (i.num - 15)).First<EURUSD_1H>().sp +
                            source.Where(k => k.num == (i.num - 16)).First<EURUSD_1H>().sp +
                            source.Where(k => k.num == (i.num - 17)).First<EURUSD_1H>().sp +
                            source.Where(k => k.num == (i.num - 18)).First<EURUSD_1H>().sp +
                            source.Where(k => k.num == (i.num - 19)).First<EURUSD_1H>().sp +
                             source.Where(k => k.num == (i.num )).First<EURUSD_1H>().sp)/20;
                    }


                        db.EURUSD_1H_NEW.InsertOnSubmit(u);
                    Console.WriteLine("本次执行完成：{0}",i.num);
                }

                db.SubmitChanges();

            }


            //“思路：以美盘定义区间，但要区别从上打下还是从下打上，经2个美盘时间守稳，从两端下单”


            
        }


    }
}
