# ASCII Art Generator

---
![image](https://github.com/user-attachments/assets/c3474bad-4560-4ccf-a237-86b11dfaa205)
---

## 📛 Описание

Эта программа предназначена для создания **ASCII-артов из изображений**, поддерживает различные наборы символов и цветовую палитру. Пользователь может задавать параметры через командную строку, включая ширину арта, набор символов и сохранение результата в файл.

---

## 🚀 **Особенности программы**  

- Преобразует изображения в цветной или черно-белый ASCII-арт (только при выводе в файл).
- Поддерживает кастомизацию ширины ASCII-вывода.
- Позволяет выбрать набор символов для создания арта из папки `Charsets`.
- Сохраняет результаты в текстовом файле по желанию.
- Легко добавляются собственные наборы символов.

---

## 📜 **Примеры использования**

Используйте команду:

```bash
dotnet ascii.cli.dll imagePath [charset=<charset>] [width=<number>] [save=<output.txt>] [mode=<режим>]
```
или
```bash
ascii.cli.exe imagePath [charset=<charset>] [width=<number>] [save=<output.txt>] [mode=<режим>]
```

| Параметр   | Описание                                              |
|-------------|-------------------------------------------------------|
| `imagePath`  | Путь до входного изображения (обязательный).         |
| `charset`   | Название набора символов из папки `Charsets`.         |
| `width`     | Ширина ASCII-арта (по умолчанию 80).                |
| `save`      | Полный путь для сохранения результата в файл `.txt`.   |
| `mode`      | Режим вывода: 'color' (по умолчанию) или 'bw' (черно-белый).  |
| `help`      | Вывод инструкции по использованию программы.           |

---
## 🖼️ Добавление собственных наборов символов
1. Перейдите в папку /Charsets.
2. Создайте новый текстовый файл с именем набора (например, custom.txt).
3. Внутри файла напишите набор символов, например:
```
@%#*+=-:. 
```
Теперь в программе можно использовать:
```bash
dotnet ascii.cli.dll "example.jpg" charset=custom
```
или
```bash
ascii.cli.exe "example.jpg" charset=custom
```
---
## 🎨 **Примеры наборов символов**

| Charsets   |  Пример названия                                       |
|-------------|-------------------------------------------------------|
| `★☆✶✷✴✵✧✦ `  | Stars         |
| `←↑→↓↔↕⇄⇅⇆⇇ `   | Arrows       |
| `▉▊▋▌▍▎▏ `     | Bars               |
| `█▓▒░  `      | Blocks   |
| `⠁⠃⠇⣿⣷⣯⣟⢿⣻⣽`      | BrailleDense   |
| `♜♞♝♛♚♟♙♖♘♗♕♔ `      | Chess   |
| `@%#*+=-:. `      | ClassicGrayscale   |
| `@&%£$!?*+:;,. `      | CustomGrunge           |
| `o. `      | Dots          |
| `▲▼◆◇○●□■ `      | Geometric         |
| `abcdefgABCDEFG `      |Letters          |
| `█▇▆▅▄▃▂▁ `      | Lines           |
| `@#$- `      | Minimal      |
| `9876543210 `      | Numbers |
| `&$#@!?><~ `      | Symbols          |
| `~≈≋≃= `      | Wave         |

---
## 🗒️ **Предполагаемые сценарим использования**
- Папка с программой `ascii.cli` находятся в `ProgramData` или другом месте
- Папка с программой `ascii.cli` добавлена в переменные среды в `Path` [по желанию]

 Вот код для `.cmd` файла на который достаточно перетянуть необходимую картинку и выбрать настройки
```bash
@echo off
chcp 65001 > nul
Title ASCII
setlocal EnableDelayedExpansion
set "charsetDir=C:\ProgramData\ascii.cli\Charsets"
if not exist "%charsetDir%" (
    echo Папка %charsetDir% не найдена.
    pause
    exit /b
)
if "%~1"=="" (
    echo Ошибка: Файл не указан, перетащите его на этот .cmd/.bat.
    pause
    exit /b
) else (
    set "inputFile=%~1"
)
echo Доступные charset файлы:
set i=0
for %%f in ("%charsetDir%\*.txt") do (
    set /a i+=1
    set "charset[!i!]=%%~nf"
    echo !i!: %%~nf
)
set /p selectedNumber=Выберите номер charset: 
if not defined charset[%selectedNumber%] (
    echo Неверный номер.
    pause
    exit /b
)
set /p width=Введите значение width: 
if "%~1"=="" (
    set /p inputFile=Введите путь к изображению: 

    set "inputFile=%inputFile:"=%"

    if not exist "%inputFile%" (
        echo Ошибка: Файл %inputFile% не найден.
        pause
        exit /b
    )
) else (
    set "inputFile=%~1"
)
echo Выберите режим:
echo 1. colored
echo 2. bw
set /p modeChoice=Введите номер режима (1 или 2): 
if "%modeChoice%"=="1" (
    set "mode=colored"
) else if "%modeChoice%"=="2" (
    set "mode=bw"
) else (
    echo Ошибка: Некорректный выбор. Попробуйте снова.
    goto ask_mode
)
echo Выполняем команду...
cls
"C:\ProgramData\ascii.cli\ascii.cli.exe" "%inputFile%" width=%width% charset=!charset[%selectedNumber%]! mode=%mode%
pause
exit /b
```
  

