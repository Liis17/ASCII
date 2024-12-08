using System.Drawing;
using System.Text;
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        if (args.Length < 1 || args[0].ToLower() == "help")
        {
            ShowHelp();
            return;
        }

        string imagePath = args[0];
        string charsetName = "default";
        int width = 80;
        string savePath = null;

        for (int i = 1; i < args.Length; i++)
        {
            if (args[i].StartsWith("charset=", StringComparison.OrdinalIgnoreCase))
                charsetName = args[i].Substring("charset=".Length).ToLower();
            else if (args[i].StartsWith("width=", StringComparison.OrdinalIgnoreCase) && int.TryParse(args[i].Substring("width=".Length), out int w))
                width = w;
            else if (args[i].StartsWith("save=", StringComparison.OrdinalIgnoreCase))
                savePath = args[i].Substring("save=".Length);
        }

        if (!File.Exists(imagePath))
        {
            Console.WriteLine("Файл изображения не найден.");
            return;
        }

        string asciiChars = LoadCharset(charsetName);
        if (asciiChars == null)
        {
            Console.WriteLine($"Набор символов '{charsetName}' не найден. Проверьте папку 'Charsets'.");
            return;
        }

        try
        {
            using var image = new Bitmap(imagePath);
            string asciiArt = ConvertToColorAscii(image, asciiChars, width);

            if (savePath != null)
            {
                File.WriteAllText(savePath, asciiArt);
                Console.WriteLine($"ASCII-арт сохранен в файл: {savePath}");
            }
            else
            {
                Console.WriteLine(asciiArt);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обработке изображения: {ex.Message}");
        }
    }

    static void ShowHelp()
    {
        Console.WriteLine("Использование:");
        Console.WriteLine("ascii.cli.dll <путь_к_изображению> [charset=<имя>] [width=<число>] [save=<путь_к_файлу>]");
        Console.WriteLine();
        Console.WriteLine("Параметры:");
        Console.WriteLine("  <путь_к_изображению>    Путь к изображению (обязательный параметр).");
        Console.WriteLine("  charset=<имя>           Имя набора символов из папки 'Charsets' (по умолчанию 'default').");
        Console.WriteLine("  width=<число>           Ширина ASCII-арта (по умолчанию 80).");
        Console.WriteLine("  save=<путь_к_файлу>     Путь для сохранения результата в текстовый файл.");
        Console.WriteLine();
        Console.WriteLine("Примеры:");
        Console.WriteLine("  ascii.cli.dll image.jpg");
        Console.WriteLine("  ascii.cli.dll image.jpg charset=blocks width=100 save=output.txt");
    }

    static string LoadCharset(string charsetName)
    {
        string charsetsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Charsets");
        string filePath = Path.Combine(charsetsPath, $"{charsetName}.txt");

        if (!Directory.Exists(charsetsPath))
        {
            Directory.CreateDirectory(charsetsPath);
            File.WriteAllText(Path.Combine(charsetsPath, "default.txt"), "@%#*+=-:. ");
        }

        return File.Exists(filePath) ? File.ReadAllText(filePath, encoding: Encoding.UTF8).Trim() : null;
    }

    static string ConvertToColorAscii(Bitmap image, string asciiChars, int width)
    {
        int height = (int)(image.Height / (double)image.Width * width * 0.55);
        var resizedImage = new Bitmap(image, new Size(width, height));
        var asciiBuilder = new StringBuilder();

        for (int y = 0; y < resizedImage.Height; y++)
        {
            for (int x = 0; x < resizedImage.Width; x++)
            {
                Color pixelColor = resizedImage.GetPixel(x, y);
                int grayValue = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                int index = grayValue * (asciiChars.Length - 1) / 255;
                string colorCode = $"\x1b[38;2;{pixelColor.R};{pixelColor.G};{pixelColor.B}m";
                asciiBuilder.Append(colorCode + asciiChars[index]);
            }
            asciiBuilder.AppendLine("\x1b[0m");
        }

        return asciiBuilder.ToString();
    }
}
