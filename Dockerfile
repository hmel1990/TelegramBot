# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем проект и восстанавливаем зависимости
COPY *.csproj ./
RUN dotnet restore

# Копируем остальные файлы и публикуем
COPY . ./
RUN dotnet publish -c Release -o /out

# Финальный образ (runtime)
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /out ./

# Устанавливаем переменную окружения
ENV TELEGRAM_TOKEN=${TELEGRAM_TOKEN}

# Запуск сборки
CMD ["dotnet", "TelegramBot.dll"]
