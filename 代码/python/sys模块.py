#encoding=utf-8
import sys
#获取执行的参数
# >python sys模块.py test 'dd'
print("执行的参数:",sys.argv)

# python的版本
print("python的版本:",sys.version)

# 最大的Int的值
print("int最大值:",sys.maxsize)

# 读取输入的到屏幕的内容
# val=sys.stdin.read()

# 返回当前系统平台名称
print("当前系统平台名称:",sys.platform)



# 设置当前递归的最大次数(注意只对本程序有效)
sys.setrecursionlimit(1200)

# 获取当前递归的最大次数
print("当前递归的最大次数:",sys.getrecursionlimit())


# 获取系统的字符编码
print("当前系统的字符编码是:",sys.getdefaultencoding())
