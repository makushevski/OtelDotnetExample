# OtelDotnetExample

Тестовый пример .NET 10 сервисов с OpenTelemetry (логи/трейсы/метрики) и готовым `docker-compose`, который поднимает всё окружение включая SigNoz.

В стеке:
- `gateway` — входной API (Swagger) и вызов `api`
- `api` — пример сервиса с БД (PostgreSQL) + задержки/ошибки для демонстрации
- `postgres` — данные приложения
- `signoz` + `otel-collector` + `clickhouse` — хранение и UI для телеметрии

## Быстрый старт

1. Запуск: `docker compose up --build`
2. Gateway Swagger: http://localhost:5000/swagger
3. SigNoz UI: http://localhost:8080

## Демонстрационные запросы

Через gateway:
- обычный: `GET http://localhost:5000/v1/weather`
- долгий: `GET http://localhost:5000/v1/weather?delayMs=3000`
- ошибка: `GET http://localhost:5000/v1/weather?throwException=true`

## Где смотреть в SigNoz

- Трейсы: `APM` / `Traces` — фильтрация по `service.name`, поиск медленных запросов по `duration`, проваливание в waterfall.
- Метрики: `Metrics` — `http.server.request.duration`, `http.server.request.count`, `process.runtime.dotnet.*` и т.п.
- Логи: `Logs` — логи приложений, корреляция с трейсами через `trace_id` (если отображается в UI/полях лога).

## Конфигурация

Приложения настраиваются стандартными переменными окружения OpenTelemetry (`OTEL_*`). См. `docker-compose.yml`.
