# Stage 1 : Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# نسخ ملف المشروع
COPY backend.csproj .

# استرجاع الحزم
RUN dotnet restore

# نسخ بقية الملفات
COPY . .

# نشر التطبيق
RUN dotnet publish -c Release -o /app/publish

# Stage 2 : Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "backend.dll"]