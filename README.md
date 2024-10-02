# RentHall.API
Проект представляет собой RESTful API на C# с использованием подхода Code First и Entity Framework Core. Архитектура проекта придерживается принципов Чистой архитектуры Роберта Мартина (Дяди Боба). В проекте используется богатая доменная модель с подходом DDD (Domain-Driven Design). В основе API лежит четкое разделение слоев: Домен (объекты значений, сущности, агрегаты и абстракции), Приложение (сервисы и бизнес-правила), Инфраструктура (доступ к данным и внешние сервисы) и Презентация (контроллеры). Внедрение зависимостей (DI) через IoC-контейнер позволяет управлять зависимостями и изолировать логику для легкого тестирования и расширяемости. Все слои четко разделены, с минимальными связями между ними, а доменная логика сконцентрирована в богатых моделях, которые включают бизнес-правила и инварианты. Также в проекте есть аутентификация, хеширование пароля и логика авторизации на основе ролей. Аутентификация происходит с помощью создания JWT-токена, который затем хранится в cookie. 
## Что делает проект 
RentHall.API разработан как API для управления конференц-залами, их бронирования, расчёта стоимости услуг, а также аналитики. Клиент может найти свободный зал и забронировать его на определённое время, при этом узнать точную стоимость бронирования. Стоимость бронирования зависит от длительности аренды зала, а также дополнительных услуг и времени бронирования. У админа есть возможность проводить стандартные CRUD операции с залами, а также дополнительными услугами и доступ к аналитике.   
## Архитектура проекта 
На основе принципов Чистой архитектуры Дяди Боба была разработана следующая архитектура : 
<br><br>
![image](https://github.com/user-attachments/assets/038fbf02-46f9-4b7d-9f82-56e2f2cf854e)
