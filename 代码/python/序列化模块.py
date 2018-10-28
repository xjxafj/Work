#_*_coding:utf-8_*_

'''
    参考博客:http://www.cnblogs.com/wupeiqi/articles/4963027.html
    用于序列化的两个模块
    json，用于字符串 和 python数据类型间进行转换
    pickle，用于python特有的类型 和 python的数据类型间进行转换
    shelve, 对pickle进行了再次封装

    Json模块提供了四个功能：dumps(返回字符串)、dump(持久化到文件)、loads、load(读取序列化的文件)
    pickle模块提供了四个功能：dumps(返回字节)、dump(持久化到文件)、loads、load(读取序列化的文件)
    shelve模块提供了: open(返回一个字典对象) close(关闭并保存)
    dumps :将数据类型转换成字符窜
    dump:将数据类型转换成字符窜,并写入文件(注意文件是字节流)
    load:序列化成文件后的反序列化

    pickle和json模块的区别
    json:支持序列化数据类型为:str int tuple list dict,需要的是字符串
    pickle:支持序列化数据类型为:所有python数据类型包含函数,需要是字节流






'''
import  pickle,json,shelve

dic={"age":1}
names={"name1":"张三","name2":"李四"}
new_names={"name1":"张三1","name2":"李四1"}

ages={"age1":20,"age2":30}

# 将字典对象写入到文件 序列化成字符串
# f=open("tet","w")
# f.write(str(dic))


# 是用eval()将对象反序列化
# f2=open("tet","r")
# data=eval(f2.read())
# print(data["age"])



# 将对象序列化到文件
# f3=open("tet2","wb")
# pickle.dump(dic,f3)


# 将序列化后的文件,反序列成对象
# f4=open("tet2","rb")
# tt=pickle.load(f4)
# print(tt["age"])



# 将对象序列化到文件
# f5=open("tet3","w")
# json.dump(dic,f5)


# 将序列化后的文件,反序列成对象
# f6=open("tet3","r")
# tt=json.load(f6)
# print(tt["age"])



shelve_dic=shelve.open('shelve_db')
# 持久化到文件
# shelve_dic["names"]=names
# shelve_dic["ages"]=ages
# shelve_dic.close()

# 获取
print(list(shelve_dic.keys()))
print(list(shelve_dic.items()))
# print(shelve_dic.get("names")["name1"])

# 修改
# shelve_dic['names']=new_names

# 删除
# del shelve_dic['names']

# add
shelve_dic["names"]=new_names

# 关闭并保存
shelve_dic.close()


