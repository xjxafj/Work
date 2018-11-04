# Work
#设置全局参数
git config --global user.name 'fujia'
git config --global user.email 'xjxafj@126.com'
git config --list

cd I:\Work-master
 #1.执行命令初始化该目录为git本地文件夹
git init                        
#2.配置本地仓库与github站点创建链接 origin代表远程仓库
git remote add origin git@github.com:xjxafj/Work.git
git add .  
gti commit '优化目录结构'
 #强制送到github上
git push -u origin master -f 




#克隆gihub上的项目到本地
git clone git@github.com:xjxafj/Work.git


--备份到本地

git fetch     //1.拷贝远程到本地版本
git reset --hard origin/master // 2.更新到本地最新的master版本
git fetch --all
git fetch
