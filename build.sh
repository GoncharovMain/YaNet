
dotnet new sln

dotnet new classlib -o YaNet
dotnet new classlib -o YaNet.Test

dotnet sln add YaNet
dotnet sln add YaNet.Test

cd YaNet.Test
dotnet add reference ../YaNet/YaNet.csproj

cd ../YaNet.Sample
dotnet add reference ../YaNet/YaNet.csproj

cd ..

dotnet add package xunit
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit.runner.visualstudio

dotnet test --logger "console;verbosity=detailed"
