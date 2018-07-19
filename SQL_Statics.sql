select enter_time,sum(case when Profit>0 then 1 when Profit<0 then 0 end) yl,COUNT(*)  zs,
sum(case when Profit>0 then 1 when Profit<0 then 0 end)*1.0/COUNT(*) yll
from result_detail
where Profit<>0 and bz='EURUSD'
group by enter_time 
order by enter_time

select sum(case when Profit>0 then 1 when Profit<0 then 0 end) yl,COUNT(*)  zs,sum(profit),
sum(case when Profit>0 then 1 when Profit<0 then 0 end)*1.0/COUNT(*) yll
from result_detail
where Profit<>0 and enter_date<>outer_date  and bz='EURUSD'


select count(*) from result_detail where Profit=0 and bz='EURUSD'

--truncate table result
--truncate table result_detail

select * from result_detail where enter_date=outer_date  and bz='EURUSD'

select enter_time,sum(case when Profit>0 then 1 when Profit<0 then 0 end) yl,COUNT(*)  zs,
sum(case when Profit>0 then 1 when Profit<0 then 0 end)*1.0/COUNT(*) yll
from result_detail
where Profit<>0 and  enter_date=outer_date  and bz='EURUSD'
group by enter_time 
order by yll desc 
