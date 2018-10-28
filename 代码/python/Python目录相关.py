# encoding: utf-8
import sys,os
sys.path.append("J:\my_repository\我的工作桌面\Pyhont\爬虫案例\通用网页爬虫\CommonCrawl")   #将路径添加的python 的环境变量中
print(__file__)   #当前文件的路径,此为相对路径
print(os.path.abspath(__file__))  #当前py文件所在绝对路径所在的路径
print(os.path.dirname(__file__))  #当前py文件所在相对路径所在的目录
print(os.path.dirname(os.path.abspath(__file__)))
