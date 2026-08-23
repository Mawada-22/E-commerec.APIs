@echo off
REM ============================================================
REM  Starts the full e-commerce stack in three windows:
REM    1. Redis        (basket storage,  port 6379)
REM    2. .NET API     (https://localhost:7100)
REM    3. Angular app  (http://localhost:4200 - opens browser)
REM  Close each window (or Ctrl+C in it) to stop that piece.
REM ============================================================

echo Starting Redis...
start "Redis" cmd /k "D:\Mawada\Redis-x64-5.0.14.1\redis-server.exe"

echo Starting API (https://localhost:7100)...
start "E-commerce API" cmd /k "cd /d %~dp0E-commerce.Apis && dotnet run --launch-profile https"

echo Waiting a few seconds for the API to boot...
timeout /t 8 /nobreak >nul

echo Starting Angular client (http://localhost:4200)...
start "Angular Client" cmd /k "cd /d %~dp0Client\client && ng serve -o"

echo.
echo All three are launching in their own windows.
echo   Storefront:  http://localhost:4200
echo   Swagger:     https://localhost:7100/swagger
