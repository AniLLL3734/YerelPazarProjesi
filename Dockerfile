# 1. Aşama: Derleme (Build)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proje dosyalarını kopyala ve bağımlılıkları yükle
COPY ["Proje.csproj", "./"]
RUN dotnet restore "Proje.csproj"

# Tüm kodları kopyala ve yayınla (Publish)
COPY . .
RUN dotnet publish "Proje.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Aşama: Çalıştırma (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# SQLite veritabanı dosyasını kopyala (Eğer varsa)
COPY app.db . 

# Bulut platformları için port ayarı
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Proje.dll"]
