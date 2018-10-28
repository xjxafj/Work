#_*_coding:utf-8_*_
# created by fujia on 8/19/18
'''

'''
import hashlib
md5=hashlib.md5()
f1=open("config_test.ini","rb")

# 计算fujia字符串的md5值
md5.update(b"fujia")
# 计算文件字节流的md5值
md5.update(f1.read())
print(md5.hexdigest())
