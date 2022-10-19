# Sny

<p align="center">
    Staň se skoro nejlepším člověkem na světě.
 
</p>

   ![logo-png](https://user-images.githubusercontent.com/28567403/193453152-0ab3a513-be03-4a3f-984e-ef1de8720571.png)


## Popis aplikace

Aplikace pro seberozvoj - správu snů, cílů a motivace k dosažení milníků.

## Screenshoty

![image](https://user-images.githubusercontent.com/28567403/193452037-a8cf5898-6a9c-46fb-8832-3ab7eba71f7e.png)

## Poznámky

Příkaz na vytvoření migrace pro projekt Sny.DB:
```
dotnet ef migrations add InitialCreate --project ..\Sny.DB --startup-project ..\Sny.Api
```
Příkaz na aktualizaci databáze pro projekt Sny.DB:
```
dotnet ef database update --project ..\Sny.DB --startup-project ..\Sny.Api
```

