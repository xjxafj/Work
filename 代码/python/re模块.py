#!/usr/bin/python
# -*- coding: UTF-8 -*-
# created by fujia on 8/19/18
'''
re.search(pattern, string, flags=0)               扫描整个字符串并返回第一个成功的匹配。
re.match(pattern, string, flags=0)                尝试从字符串的起始位置匹配一个模式，如果不是起始位置匹配成功的话，match()就返回none。
re.sub(pattern, repl, string, count=0, flags=0)   Python 的 re 模块提供了re.sub用于替换字符串中的匹配项。          以下实例中将字符串中的匹配的数字乘以 2：
re.findall(string[, pos[, endpos]])                  在字符串中找到正则表达式所匹配的所有子串，并返回一个列表，如果没有找到匹配的，则返回空列表。注意： match 和 search 是匹配一次 findall 匹配所有。
re.finditer(pattern, string, flags=0)             和 findall 类似，在字符串中找到正则表达式所匹配的所有子串，并把它们作为一个迭代器返回。
re.split(pattern, string[, maxsplit=0, flags=0])  split 方法按照能够匹配的子串将字符串分割后返回列表，它的使用形式如下：
re.compile(pattern,flags=0)                       先定义规则,后重复调用匹配

函数参数说明：
pattern	匹配的正则表达式
string	要匹配的字符串。
flags	标志位，用于控制正则表达式的匹配方式，如：是否区分大小写，多行匹配等等。 写法  :re.M|re.I

        re.I 忽略大小写
        re.L 表示特殊字符集 \w, \W, \b, \B, \s, \S 依赖于当前环境
        re.M 多行模式
        re.S 即为 . 并且包括换行符在内的任意字符（. 不包括换行符） print(re.search('.','\n'),re.S)
        re.U 表示特殊字符集 \w, \W, \b, \B, \d, \D, \s, \S 依赖于 Unicode 字符属性数据库
        re.X 为了增加可读性，忽略空格和 # 后面的注释
repl : 替换的字符串，也可为一个函数。
string : 要被查找替换的原始字符串。
count : 模式匹配后替换的最大次数，默认 0 表示替换所有的匹配。

string : 待匹配的字符串。
pos : 可选参数，指定字符串的起始位置，默认为 0。
endpos : 可选参数，指定字符串的结束位置，默认为字符串的长度。



(?P<province>\d+) 对匹配的地方取别名 ,group('province')取值

返回结果处理函数
span()
group()
group(1)
group('province')
groups()
groupdict()
'''
import  re

# match
print(re.match('www', 'www.runoob.com').span())  # 在起始位置匹配
print(re.match('com', 'www.runoob.com'))         # 不在起始位置匹配

# search

print(re.search('(?P<province>\d{3})(?P<city>\d{3})(?P<born_year>\d{4})','130704200005250613').groupdict())
print(re.search('(?P<province>\d{3})(?P<city>\d{3})(?P<born_year>\d{4})','130704200005250613').group('province'))
print(re.search('www', 'www.runoob.com').span())  # 在起始位置匹配
print(re.search('com', 'www.runoob.com').span())         # 不在起始位置匹配
line = "Cats are smarter than dogs";
searchObj = re.search(r'(.*) are (.*?) .*', line, re.M | re.I)

if searchObj:
    print("searchObj.group() : ", searchObj.group())
    print("searchObj.group(1) : ", searchObj.group(1))
    print("searchObj.group(2) : ", searchObj.group(2))
else:
    print
    "Nothing found!!"



# re.findall()
pattern = re.compile(r'\d+')   # 查找数字
result1 = pattern.findall('runoob 123 google 456')
result2 = pattern.findall('run88oob123google456', 0, 10)


# re.finditer()
it = re.finditer(r"\d+","12a32bc43jf3")
for match in it:
    print (match.group() )









# 将匹配的数字乘以 2
def double(matched):
    value = int(matched.group('value'))
    return str(value * 2)
s = 'A23G4HFD567A'
print(re.sub('A','f',s))
print(re.sub('(?P<value>\d+)', double, s))


# re.split()
print(re.split('\W+', 'runoob, runoob, runoob.'))


# re.pullmatch  完全匹配
print(re.fullmatch('(\w+)\.(\w+)\.(\w+)','www.baidu.com'))

# re.compile()
pattern=re.compile('(\w+)\.(\w+)\.(\w+)')
print(pattern.fullmatch("www.baidu.com"))

