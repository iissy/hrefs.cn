#基于 microsoft/dotnet 来构建我们的镜像
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0

#拷贝项目publish文件夹中的所有文件到docker容器中的publish文件夹中
COPY . /publish

#设置工作目录为 /publish 文件夹，即容器启动默认的文件夹
WORKDIR /publish

#设置Docker容器对外暴露80端口
EXPOSE 80/tcp

#使用dotnet www.hrefs.cn.dll来运行应用程序，www.hrefs.cn.dll是你网站的启动dll
CMD ["dotnet", "hrefs.cn.dll"]