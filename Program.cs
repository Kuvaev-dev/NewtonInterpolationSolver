using System.Text;

namespace NewtonInterpolationSolver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.OutputEncoding = Encoding.Unicode;

            while (true)
            {
                Console.Clear();

                Console.WriteLine("Оберіть спосіб введення даних:");
                TextViewer.ChangeColor("\t1. Вручну", "blue");
                TextViewer.ChangeColor("\t2. З файлу", "yellow");
                TextViewer.ChangeColor("\t3. Вийти", "red");
                Console.WriteLine("Ваш вибір: ");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    TextViewer.ChangeColor("\nПОМИЛКА: Некоректний ввід. Введіть число від 1 до 3", "red");
                    Console.ReadLine();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        ProcessInput(new ConsoleInputProvider());
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Введіть шлях до файлу:");
                        string filePath = Console.ReadLine();
                        ProcessInput(new FileInputProvider(filePath));
                        break;
                    case 3:
                        return;
                    default:
                        TextViewer.ChangeColor("\nПОМИЛКА: Опції не існує. Повторіть спробу, будь-ласка", "red");
                        break;
                }

                Console.ReadLine();
            }
        }

        static void ProcessInput(IInputProvider inputProvider)
        {
            double[] xValues, yValues;
            do
            {
                TextViewer.ChangeColor("Значення X", "blue");
                xValues = inputProvider.GetData();
            } while (xValues == null);

            do
            {
                TextViewer.ChangeColor("Значення Y", "blue");
                yValues = inputProvider.GetData();
            } while (yValues == null);

            if (xValues.Length != yValues.Length)
            {
                TextViewer.ChangeColor("\nПОМИЛКА: Кількість значень Х не відповідає кількості значень Y.", "red");
                return;
            }

            double value;
            string input;
            do
            {
                TextViewer.ChangeColor("Значення інтерполяції", "blue");
                Console.WriteLine("Введіть значення, для якого потрібно здійснити інтерполяцію:");
                input = Console.ReadLine();
                if (!double.TryParse(input, out value))
                {
                    TextViewer.ChangeColor("\nПОМИЛКА: Некоректне значення. Будь ласка, введіть число.", "red");
                    TextViewer.ChangeColor("\nНатисніть будь-яку клавішу, щоб спробувати знову...\n", "blue");
                    Console.ReadKey();
                }
            }
            while (!double.TryParse(input, out value));

            int choice;
            do
            {
                Console.WriteLine("Виберіть метод інтерполяції:");
                TextViewer.ChangeColor("\t1. Перша формула Ньютона", "blue");
                TextViewer.ChangeColor("\t2. Друга формула Ньютона", "yellow");
                Console.WriteLine("Ваш вибір: ");
                if (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2))
                {
                    TextViewer.ChangeColor("\nПОМИЛКА: Некоректне значення. Будь ласка, введіть 1 або 2", "red");
                    TextViewer.ChangeColor("\nНатисніть будь-яку клавішу, щоб спробувати знову...\n", "blue");
                    Console.ReadKey();
                }
            }
            while (choice != 1 && choice != 2);

            double result;
            if (choice == 1)
            {
                result = Interpolator.NewtonFirstInterpolation(xValues, yValues, value);
            }
            else
            {
                result = Interpolator.NewtonSecondInterpolation(xValues, yValues, value);
            }

            TextViewer.ChangeColor($"\n\tРезультат інтерполяції: {result}", "yellow");
            TextViewer.ChangeColor("\nНатисніть будь-яку клавішу, для продовження...\n", "blue");
        }
    }
}
