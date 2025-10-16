[首页](Home.md)・[更新日志](UpdateLog.md)・[配置](Config.md)・[主题](Theme.md)

## 安装

### NuGet安装

> 👏 推荐使用NuGet快速安装

#### 通过Visual Studio可视化安装
![nuget](Img/NuGet.png)

#### 通过PM命令安装
PM> `Install-Package AntdUI`

---

### 下载源码

> 打开AntdUI的码云地址：[https://gitee.com/antdui/AntdUI](https://gitee.com/antdui/AntdUI)
![downcode](Img/DownCode.png)

解压后双击打开 `AntdUI.sln` 解决方案，将 `examples/Demo` 项目设为启动项目，`F5` 启动

#### 源码下载无法编译？

> 编译器要求 **Visual Studio 2022** 以及以上

[Visual Studio 安装 旧版本(.NET Framework 4.0 和 4.5)](InstallOldVersionFramework.md)

#### 看不到工具箱？

需将 `AntdUI.csproj` 内 `TargetFrameworks` 只保留自己项目使用的框架版本，然后重新生成

> 操作完还是无法显示，重启VS让其重新加载，**多重新生成确保dll是最新的**