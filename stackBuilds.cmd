xcopy /y O:\users\tobya\source\repos\SharpPhysics\SharpPhysics\bin\Debug\net7.0\SharpPhysics.* D:\tobya\Programs\
set buildnum=g
for /f "delims=" %%x in (O:\users\tobya\source\repos\SharpPhysics\buildnumber) do set buildnum=%%x
md D:\tobya\Programs\%buildnum%
xcopy /y D:\tobya\Programs\* D:\tobya\Programs\%buildnum%\
del /q D:\tobya\Programs\*.*