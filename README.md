# Проект сервера: ASP.NET Web API

## Описание
Этот проект представляет собой серверную часть приложения, построенную с использованием ASP.NET Web API и Entity Framework.


## Установка

1. **Клонирование репозитория**

    ```bash
    git clone https://github.com/your-username/your-repo.git
    ```

2. **Настройка базы данных**

    Обновите строку подключения в `appsettings.json`:

    ```json
    "ConnectionStrings": {
        "DefaultConnection": "your-connection-to-database"
    }
    ```

3. **Применение миграций**

    ```bash
    dotnet ef database update
    ```

4. **Запуск проекта**

    ```bash
    dotnet run
    ```

## Миграции

### Создание новой миграции

```bash
dotnet ef migrations add MigrationName
