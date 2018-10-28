#encoding=utf-8
import random,string
#获取随机数,结尾包含3
a=random.randint(1,3)

# 获取随机数,结尾不包含3
b=random.randrange(1,3)

# 获取随机数,结尾不包含3
c=random.random()

# 给定字符中获取一个字符
d=random.choice('123456')

# help()random.choices()
# 给定字符中获取多个个字符
e=random.sample('123',3)
print(a,b,c,d,e)

# 参数随机验证码
f=string.printable
j=''.join(random.sample(f,3))
print(f,j)

# 将列表随机打乱顺序
temList=list(range(10))
random.shuffle(temList)
print(temList)

