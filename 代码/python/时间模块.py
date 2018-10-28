#encoding=utf-8
'''
python中时间日期格式化符号：
%y 两位数的年份表示（00-99）
%Y 四位数的年份表示（000-9999）
%m 月份（01-12）
%d 月内中的一天（0-31）
%H 24小时制小时数（0-23）
%I 12小时制小时数（01-12）
%M 分钟数（00=59）
%S 秒（00-59）
%a 本地简化星期名称
%A 本地完整星期名称
%b 本地简化的月份名称
%B 本地完整的月份名称
%c 本地相应的日期表示和时间表示
%j 年内的一天（001-366）
%p 本地A.M.或P.M.的等价符
%U 一年中的星期数（00-53）星期天为星期的开始
%w 星期（0-6），星期天为星期的开始
%W 一年中的星期数（00-53）星期一为星期的开始
%x 本地相应的日期表示
%X 本地相应的时间表示
%Z 当前时区的名称
%% %号本身
'''

# time主要用于时间转换
# datetime主要用户时间计算
import  time,datetime
# 当前系统时间
currTimeDt=time.localtime()
# 当前系统时间戳(Shíjiān chuō)
timestamp=time.time()
# 时间戳转换时间对象
temDT=time.localtime(timestamp)
# 时间对象转换成时间戳
timestamp2=time.mktime(currTimeDt)
# 时间对象转换成字符串
strDate=time.strftime('%Y %m %d',temDT)
# 字符串转换成时间对象
dt=time.strptime(strDate,'%Y %m %d')



'''
%a 本地简化星期名称
%A 本地完整星期名称
%b 本地简化的月份名称
%B 本地完整的月份名称
%c 本地相应的日期表示和时间表示
%j 年内的一天（001-366）
%p 本地A.M.或P.M.的等价符
%U 一年中的星期数（00-53）星期天为星期的开始
%w 星期（0-6），星期天为星期的开始
%W 一年中的星期数（00-53）星期一为星期的开始
%x 本地相应的日期表示
%X 本地相应的时间表示
%Z 当前时区的名称
%% %号本身


datetime模块用于是date和time模块的合集，datetime有两个常量，MAXYEAR和MINYEAR，分别是9999和1.

datetime模块定义了5个类，分别是

1.datetime.date：表示日期的类

2.datetime.datetime：表示日期时间的类

3.datetime.time：表示时间的类

4.datetime.timedelta：表示时间间隔，即两个时间点的间隔

5.datetime.tzinfo：时区的相关信息

'''
# ：返回当前日期时间的日期部分
datetime.datetime.now().date()
# ：返回当前日期时间的时间部分
datetime.datetime.now().time()
# 时间戳转换成
dt=datetime.date.fromtimestamp(time.time())
print(dt)

#由日期格式转化为字符串格式
datetime.datetime.now().strftime('%b-%d-%Y %H:%M:%S')
# 　　'Apr-16-2017 21:01:35'
# 由字符串格式转化为日期格式
datetime.datetime.strptime('Apr-16-2017 21:01:35', '%b-%d-%Y %H:%M:%S')

# 时间运算
temDT2=datetime.datetime.now()-datetime.timedelta(days=1,hours=1,minutes=1,milliseconds=1,microseconds=1,weeks=1)
print(temDT2)
# 回到指定的时间
temDT3=temDT2.replace(year=2018,month=2,day=1)
print(temDT3)
# datetime.time.replace()

