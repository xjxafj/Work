#encoding=utf-8
import os
path=r"d:\1.txt"
print("当前工作目录,即当前Python脚本工作的目录路径:",os.getcwd())
print("返回指定目录下的文件夹和文件名:",os.listdir('J:\my_repository\我的工作桌面\Pyhont\爬虫案例\通用网页爬虫\CommonCrawl'))
# 删除文件
# os.remove(r"d:\1\1.txt")
# 删除多个目录
# os.removedirs(r"d:\1")
# 判断给定的路径是否是文件
print(os.path.isfile(path))
# 判断给定的路径是否是文件夹
print(os.path.isdir(path))
# 判断给定的路径是否是绝对路径
print(os.path.isabs(path))
# 判断给定的路径或者文件夹是否是否存在
print(os.path.exists(r'd:\1'))
# 分离给定的路径或者文件路径
print(os.path.split(r'd:\1\1.txt'))
# 分离给定的路径或者文件路路径的扩展名称
print(os.path.splitext(r'd:\1\1.txt'))

# 获取给定的路径或者文件路路径的当前目录
print(os.path.dirname(r'd:\2\1.txt'))

# 获取给定的路径或者文件路路径的当前目录
print(os.path.abspath(r'd:\2\1.txt'))

# 获取文件大小
print(os.path.getsize(path))

# 拼接目录
print("拼接目录",os.path.join(r"D:\1","1.txt"))

# 获取当前系统的所有环境变量
print(os.environ)

# 获取当前环境变量的HOME值
print(os.getenv("JAVA_HOME"))

# 设置当前系统的所有环境变量(注意:没有就添加有就返回,而且只在当前pyhton环境中有效)
print(os.environ.setdefault("test","te"))

# 运行Shell命令
os.system("ipconfig")

# 获得当前使用的操作系统，Windows 是 NT 内核，所以会得到nt，而 Linux/Unix 用户则会得到posix
os.name

# 当前系统的行终止符
print("当前系统的终止符:",os.linesep)

# 获取文件的状态属性
print("获取文件的状态属性",os.stat(path))
# os.stat_result(st_mode=33206, st_ino=184647584722196073, st_dev=2853885809, st_nlink=1, st_uid=0, st_gid=0, st_size=2, st_atime=1526829259, st_mtime=1527093675, st_ctime=1526829259)
# st_mode 权限 st_ctime 创建时间 st_size(文件大小,单位字节)


#
# os.get_terminal_size()
help(os.get_terminal_size)


# 杀死进程 pid,sig
# os.kill(10884,1)
