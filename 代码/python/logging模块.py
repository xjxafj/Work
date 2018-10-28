#_*_coding:utf-8_*_
# created by fujia on 8/19/18
'''
博客地址 https://www.cnblogs.com/Nicholas0707/p/9021672.html

DEBUG	最详细的日志信息，典型应用场景是 问题诊断
INFO	信息详细程度仅次于DEBUG，通常只记录关键节点信息，用于确认一切都是按照我们预期的那样进行工作
WARNING	当某些不期望的事情发生时记录的信息（如，磁盘可用空间较低），但是此时应用程序还是正常运行的
ERROR	由于一个更严重的问题导致某些功能不能正常运行时记录的信息
CRITICAL	当发生严重错误，导致应用程序不能继续运行时记录的信息

事件发生时间
事件发生位置
事件的严重程度--日志级别
事件内容

logging.basicConfig()函数包含参数说明
参数名称                	描述
filename	指定日志输出目标文件的文件名（可以写文件名也可以写文件的完整的绝对路径，写文件名日志放执行文件目录下，写完整路径按照完整路径生成日志文件），指定该设置项后日志信心就不会被输出到控制台了
filemode	指定日志文件的打开模式，默认为'a'。需要注意的是，该选项要在filename指定时才有效
format	指定日志格式字符串，即指定日志输出时所包含的字段信息以及它们的顺序。logging模块定义的格式字段下面会列出。
datefmt	指定日期/时间格式。需要注意的是，该选项要在format中包含时间字段%(asctime)s时才有效
level	指定日志器的日志级别
stream	指定日志输出目标stream，如sys.stdout、sys.stderr以及网络stream。需要说明的是，stream和filename不能同时提供，否则会引发 ValueError异常
style	Python 3.2中新添加的配置项。指定format格式字符串的风格，可取值为'%'、'{'和'$'，默认为'%'
handlers	Python 3.3中新添加的配置项。该选项如果被指定，它应该是一个创建了多个Handler的可迭代对象，这些handler将会被添加到root logger。需要说明的是：filename、stream和handlers这三个配置项只能有一个存在，不能同时出现2个或3个，否则会引发ValueError异常。


logging模块中定义好的可以用于format格式字符串说明
字段/属性名称	使用格式	描述
asctime	%(asctime)s	将日志的时间构造成可读的形式，默认情况下是‘2016-02-08 12:00:00,123’精确到毫秒
name	%(name)s	所使用的日志器名称，默认是'root'，因为默认使用的是 rootLogger
filename	%(filename)s	调用日志输出函数的模块的文件名； pathname的文件名部分，包含文件后缀
funcName	%(funcName)s	由哪个function发出的log， 调用日志输出函数的函数名
levelname	%(levelname)s	日志的最终等级（被filter修改后的）
message	%(message)s	日志信息， 日志记录的文本内容
lineno	%(lineno)d	当前日志的行号， 调用日志输出函数的语句所在的代码行
levelno	%(levelno)s	该日志记录的数字形式的日志级别（10, 20, 30, 40, 50）
pathname	%(pathname)s	完整路径 ，调用日志输出函数的模块的完整路径名，可能没有
process	%(process)s	当前进程， 进程ID。可能没有
processName	%(processName)s	进程名称，Python 3.1新增
thread	%(thread)s	当前线程， 线程ID。可能没有
threadName	%(thread)s	线程名称
module	%(module)s	调用日志输出函数的模块名， filename的名称部分，不包含后缀即不包含文件后缀的文件名
created	%(created)f	当前时间，用UNIX标准的表示时间的浮点数表示； 日志事件发生的时间--时间戳，就是当时调用time.time()函数返回的值
relativeCreated	%(relativeCreated)d	输出日志信息时的，自Logger创建以 来的毫秒数； 日志事件发生的时间相对于logging模块加载时间的相对毫秒数
msecs	%(msecs)d	日志事件发生事件的毫秒部分。logging.basicConfig()中用了参数datefmt，将会去掉asctime中产生的毫秒部分，可以用这个加上



logging的高级用法
第二种是一个模块级别的函数是logging.getLogger([name])（返回一个logger对象，如果没有指定名字将返回root logger）。
logging日志模块四大组件
在介绍logging模块的日志流处理流程之前，我们先来介绍下logging模块的四大组件：
组件名称	对应类名	功能描述
日志器	Logger	提供了应用程序可一直使用的接口
处理器	Handler	将logger创建的日志记录发送到合适的目的输出
过滤器	Filter	提供了更细粒度的控制工具来决定输出哪条日志记录，丢弃哪条日志记录
格式器	Formatter	决定日志记录的最终输出格式
logging模块就是通过这些组件来完成日志处理的，上面所使用的logging模块级别的函数也是通过这些组件对应的类来实现的。
这些组件之间的关系描述：
日志器（logger）需要通过处理器（handler）将日志信息输出到目标位置，如：文件、sys.stdout、网络等；
不同的处理器（handler）可以将日志输出到不同的位置；
日志器（logger）可以设置多个处理器（handler）将同一条日志记录输出到不同的位置；
每个处理器（handler）都可以设置自己的过滤器（filter）实现日志过滤，从而只保留感兴趣的日志；
每个处理器（handler）都可以设置自己的格式器（formatter）实现同一条日志以不同的格式输出到不同的地方。
简单点说就是：日志器（logger）是入口，真正干活儿的是处理器（handler），处理器（handler）还可以通过过滤器（filter）和格式器（formatter）对要输出的日志内容做过滤和格式化等处理操作。
logging日志模块相关类及其常用方法介绍
与logging四大组件相关的类：Logger, Handler, Filter, Formatter。



'''
import logging
from logging import handlers
# 设置日志输出的相关配置 格式等
logging.basicConfig(filename='log_test.log',
                    level=logging.DEBUG,
                    format='%(asctime)s %(message)s %(levelno)s',
                    datefmt='%m/%d/%Y %I:%M:%S %p')

logging.error("teea")
logging.warning("警告")



# 自定义logging

# 生成日志log对象
view_log=logging.getLogger("chat.gui")
# 设置全局日志输出级别(默认INFO)
# view_log.setLevel(logging.INFO)


# 生成hanle对象,输出控制handler
# 屏幕输出Handler
streamhadler=logging.StreamHandler()
# 日志文件不切断
# filehadler=logging.FileHandler("web.log")
# 日志文件切断输出(文件大小切断)  maxBytes 最大字节  backupCount 最多的日志文件的数量
# filehadler=handlers.RotatingFileHandler("web.log",maxBytes=10,backupCount=3)
# 日志文件切断输出(日志时间切断)  when 时间类型 interval 时间间隔  backupCount 最多的日志文件的数量
filehadler=handlers.TimedRotatingFileHandler("web.log",when='S',interval=5,backupCount=3)


# 设置局部日志输出级别默认INFO)
# streamhadler.setLevel(logging.INFO)
# filehadler.setLevel(logging.ERROR)


# 定义输出格式对象
stream_formatter=logging.Formatter('%(asctime)s %(message)s %(levelno)s')
file_formatter=logging.Formatter('%(asctime)s %(message)s ')


# 格式设置关联到handler
streamhadler.setFormatter(stream_formatter)
filehadler.setFormatter(file_formatter)


#设置局部日志级别
# streamhadler.setLevel(logging.ERROR)
# filehadler.setLevel(logging.ERROR)


# handler关联到log对象
view_log.addHandler(streamhadler)
view_log.addHandler(filehadler)

# 添加过滤Filter
class IgnoreBackupLogFilter(logging.Filter):
    def filter(self,record):
        return "backu" not in record.getMessage()
view_log.addFilter(IgnoreBackupLogFilter())







view_log.info("backup")

view_log.debug("tetetstst")











