# HTML Analyzer



###  Требования

- [.NET 5](https://dotnet.microsoft.com/download/dotnet/5.0)



### Сборка и запуск

##### Используя .NET CLI:

Из корневой директории,

`dotnet run -p src/CLI -- --help` - получить помощь по использованию.

`dotnet run -p src/CLI -- path/to/the/file` или `dotnet run -p src/CLI -- /path/to/the/file` - анализировать файл из локальной файловой системы.

`dotnet run -p src/CLI -- http://www.example.com` - получить и анализировать файл из интернета.



Относительный путь к файлу возможно указать только для локальной файловой системы.



`-l` и `-v` флаги могут быть указаны для отображения логов в файл и/или консоль соответственно.

