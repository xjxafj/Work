#_*_coding:utf-8_*_
# 高级的 文件、文件夹、压缩包 处理模块
# 相关博客地址  http://www.cnblogs.com/wupeiqi/articles/4963027.html
import shutil,zipfile,tarfile

f1=open(r"d:\1.txt",'r')
f2=open(r"d:\2.txt","w")
# 将文件内容拷贝到另一个文件中，可以部分内容
# f1文件对象一 f2文件对象2   10每次读取10
# shutil.copyfileobj(f1,f2,10)
# shutil.copyfileobj(f1,f2)
#拷贝文件(文件路径)
# shutil.copyfile(r"d:\1.txt",r"d:\3.txt")
# 仅拷贝权限。内容、组、用户均不变
# shutil.copymode(r"d:\1.txt",r"d:\3.txt")
# 拷贝文件和权限
# shutil.copy(r"d:\1.txt",r"d:\3.txt")


# 拷贝文件和权限(全拷贝)
# shutil.copy2(r"d:\1.txt",r"d:\3.txt")

#拷贝目录(ignore 拷贝时忽略的文件)
# shutil.copytree(r"d:\1",r"d:\2",ignore=shutil.ignore_patterns("1.txt"))

# 递归的去删除文件
# shutil.rmtree(r"d:\2")

#递归式的移动文件
# shutil.move("d:\1","d:\2")

# 创建压缩包并返回文件路径，例如：zip、tar
# base_name： 压缩包的文件名，也可以是压缩包的路径。只是文件名时，则保存至当前目录，否则保存至指定路径，
# # 如：www                        =>保存至当前路径
# # 如：/Users/wupeiqi/www =>保存至/Users/wupeiqi/
# # format：	压缩包种类，“zip”, “tar”, “bztar”，“gztar”
# # root_dir：	要压缩的文件夹路径（默认当前目录）
# # owner：	用户，默认当前用户
# # group：	组，默认当前组
# # logger：	用于记录日志，通常是logging.Logger对象
# r"d:\2"(压缩到的目录)  r"d:\1"(要压缩的目录)
shutil.make_archive(r"d:\2","tar",root_dir=r"d:\1")


# 压缩
z = zipfile.ZipFile('laxi.zip', 'w')
z.write('a.log')
z.write('data.data')
z.close()

# 解压
z = zipfile.ZipFile('laxi.zip', 'r')
z.extractall()
z.close()



# 打包(文件大小不变)
tar = tarfile.open('your.tar','w')
tar.add('/Users/wupeiqi/PycharmProjects/bbs2.zip', arcname='bbs2.zip')
tar.add('/Users/wupeiqi/PycharmProjects/cmdb.zip', arcname='cmdb.zip')
tar.close()

# 解包(文件大小不变)
tar = tarfile.open('your.tar','r')
tar.extractall()  # 可设置解压地址
tar.close()


