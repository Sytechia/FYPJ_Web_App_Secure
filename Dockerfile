#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["FYPJ_Web_App_Insecure/FYPJ_Web_App_Insecure.csproj", "FYPJ_Web_App_Insecure/"]
RUN dotnet restore "FYPJ_Web_App_Insecure/FYPJ_Web_App_Insecure.csproj"
COPY . .
WORKDIR "/src/FYPJ_Web_App_Insecure"
RUN dotnet build "FYPJ_Web_App_Insecure.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FYPJ_Web_App_Insecure.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FYPJ_Web_App_Insecure.dll"]
