# -*- coding: utf-8 -*-
# 无参数函数装饰器
def login1(fun):
    def wrapper(*args, **kwargs):
        print('执行了新代码login1')
        fun(*args, **kwargs)
    return wrapper

# 有参数函数装饰器
def login2(*arg, **kwarg):
    def outer(fun):
        def wrapper(*args, **kwargs):
            print('执行了新代码login2',*arg,*kwarg)
            fun(*args, *kwargs)
        return wrapper
    return outer

@login1
def guiZhou(*args, **kwargs):
    print("看贵州", *args,*kwargs)

# 直接调用函数
# guiZhou('dd')

# 函数进行包装
# outer = login2('dd')
# guiZhou = outer(guiZhou)
# guiZhou()
guiZhou("dt")
