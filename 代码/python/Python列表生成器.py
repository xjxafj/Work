#列表生成器
a=list(range(10))
print(a)
a=[i*i if i>1 else i for i in a]
print(a)


#生成器定义
a2=(i for i in range(5))
a2=(i for i in range(5))
#调用next(a2) 获取生成器的下一个元素
#超出定义会报错  StopIteration
print(next(a2))
