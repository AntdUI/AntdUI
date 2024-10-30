[Home](Home.md)・[UpdateLog](UpdateLog.md)・[Config](Config.md)・[Theme](Theme.md)・[SVG](SVG.md)

### Installing old versions of Visual Studio (.NET Framework 4.0 and 4.5)

***

There is no single component in the Visual Studio 2022 installation program **.NET Framework4.0** or **.NET Framework4.5**

> Other NET versions can be downloaded directly for Visual Studio Developer toolkit in NET SDK

![1](Img/InstallOldVersionFramework_1.png)

#### Solution:

**Download the 4.0 installation package through Nuget**
Download link: [https://www.nuget.org/packages/Microsoft.NETFramework.ReferenceAssemblies.net40](https://www.nuget.org/packages/Microsoft.NETFramework.ReferenceAssemblies.net40)

**Download the 4.5 installation package through Nuget**
Download link: [https://www.nuget.org/packages/microsoft.netframework.referenceassemblies.net45](https://www.nuget.org/packages/microsoft.netframework.referenceassemblies.net45)

![2](Img/InstallOldVersionFramework_2.png)

Download the installation package `.nupkg`

![3](Img/InstallOldVersionFramework_3.png)

Then change the suffix name to `.zip` and extract the contents inside

![4](Img/InstallOldVersionFramework_4.png)

After decompression, enter `build/.NETFramework`, find v4.5 folder

![5](Img/InstallOldVersionFramework_5.png)

Copy the `v4.5` folder to **`C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework`** directly overrides replacement folders and files

![6](Img/InstallOldVersionFramework_6.png)

Then restart Visual Studio and you will be able to see 4.0 and 4.5