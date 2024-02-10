using NewtonInterpolationSolver.Data;

namespace NewtonInterpolationSolver.Input
{
    // Інтерфейс для введення даних
    public interface IInputProvider
    {
        // Метод для отримання даних. Повертає об'єкт типу DataArrays, який містить значення x, y та інтерполяції.
        DataArrays GetData();
    }
}
