namespace MapBul.SharedClasses.Constants
{
    public class Error
    {
        private readonly int _number;
        private readonly string _message;

        public int Number { get { return _number; } }
        public string Message { get { return _message; } }

        public Error(int number, string message)
        {
            _number = number;
            _message = message;
        }
    }

    public static class Errors
    {

        public static Error UserNotFound = new Error(1, "Пользователь не найден");

        public static Error UserExists = new Error(2, "Пользователь существует");

        public static Error UnknownError = new Error(3, "Неизвестная ошибка");

        public static Error NotFound = new Error(4, "Не найдено");

        public static Error UserBlocked=new Error(5, "Пользователь заблокирован");

        public static Error UserNotAuthorized =new Error(6,"Пользователь не авторизован");
    }
}