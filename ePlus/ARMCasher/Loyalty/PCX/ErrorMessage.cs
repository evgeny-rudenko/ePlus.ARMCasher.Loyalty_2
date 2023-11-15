// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.PCX.ErrorMessage
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System.Collections.Generic;

namespace ePlus.ARMCasher.Loyalty.PCX
{
  internal class ErrorMessage
  {
    private static readonly Dictionary<int, string> errorsDict = new Dictionary<int, string>();

    static ErrorMessage()
    {
      ErrorMessage.errorsDict.Add(2, "Сеанс связи с ПЦ выполнен успешно, но истек лимит времени, требуется повторный вызов Flush.");
      ErrorMessage.errorsDict.Add(1, "Не удалось выполнить операцию по причине отсутствия доступа к ПЦ. Данный код ошибки возможен для операций: AuthPoints(начисление баллов), Refund(возврат начисления баллов) и Reverse.");
      ErrorMessage.errorsDict.Add(0, "Функция выполнена успешно.");
      ErrorMessage.errorsDict.Add(-598, "Ошибка при сохранении отката в базу данных. PCX – не сможет отменить операцию самостоятельно, после восстановления доступа к базе данных PCX, необходимо вызвать функцию Reverse.");
      ErrorMessage.errorsDict.Add(-595, "Вызвана функция Flush в момент выполнения автоматического сеанса связи.");
      ErrorMessage.errorsDict.Add(-509, "Истек таймаут при ожидании ответа от ПЦ.");
      ErrorMessage.errorsDict.Add(-481, "Истек таймаут при подключении к ПЦ");
      ErrorMessage.errorsDict.Add(-587, "Неверный тип идентификатора карты.");
      ErrorMessage.errorsDict.Add(-586, "Не задано ни одно из свойств (CertSubjectName, CertFilePath, KeyFilePath, KeyPassword).");
      ErrorMessage.errorsDict.Add(-585, "Ошибка при инициализации модуля SSL. (неверный KeyPassword)");
      ErrorMessage.errorsDict.Add(-584, "Неверный формат данных запроса (отрицательная сумма, в текстовых параметрах присутствуют недопустимые символы и т.п.)");
      ErrorMessage.errorsDict.Add(-579, "PCX не инициализирован (не был вызван метод Init).");
      ErrorMessage.errorsDict.Add(-578, "Неверные аргументы (в качестве объектов-параметров были переданы объекты со значением null).");
      ErrorMessage.errorsDict.Add(-569, "Ошибка при обращении к базе данных.");
      ErrorMessage.errorsDict.Add(-529, "Не удалось загрузить gcframework.dll");
      ErrorMessage.errorsDict.Add(-525, "PCX уже был инициализирован.");
      ErrorMessage.errorsDict.Add(-519, "Сертификат CertSubjectName не найден.");
      ErrorMessage.errorsDict.Add(-591, "Профиль уже используется другим приложением");
      ErrorMessage.errorsDict.Add(-518, "Неверный формат сертификата CertSubjectName.");
      ErrorMessage.errorsDict.Add(-151, "Запрошено списание суммы, превышающей текущий остаток на счете.");
      ErrorMessage.errorsDict.Add(-152, "Недопустимое списание. Например, в чеке есть запрещенные товары.");
      ErrorMessage.errorsDict.Add(-162, "Карта заблокирована.");
      ErrorMessage.errorsDict.Add(-163, "Карта не активирована");
      ErrorMessage.errorsDict.Add(-164, "Карта просрочена.");
      ErrorMessage.errorsDict.Add(-165, "Карта уже активирована");
      ErrorMessage.errorsDict.Add(-136, "Счет карты заблокирован. В ПЦ возможна привязка нескольких карт к одному счету.");
      ErrorMessage.errorsDict.Add(-157, "Карта ограничена. Списание запрещено.");
      ErrorMessage.errorsDict.Add(-203, "Неизвестный идентификатор партнера (Участника коалиции).");
      ErrorMessage.errorsDict.Add(-214, "Неизвестная (незарегистрированная в ПЦ) карта.");
      ErrorMessage.errorsDict.Add(-258, "Неизвестный терминал (касса).");
      ErrorMessage.errorsDict.Add(-320, "Неизвестное критическое расширение в запросе.");
      ErrorMessage.errorsDict.Add(-321, "Плохой формат известного расширения.");
      ErrorMessage.errorsDict.Add(-330, "Некорректное значение аргумента операции. В ответе с таким статусом тестовое сообщение description содержит имя некорректного параметра.");
      ErrorMessage.errorsDict.Add(-340, "Запрошенная операция не поддерживается ПЦ.");
      ErrorMessage.errorsDict.Add(-389, "Ошибка аутентификации.  (Пример – запрос операции с параметром (PCX.Location или PCX.PartnerID или другим)  - не соответствующим выданному CertSubjectName на данную торговую точку)");
      ErrorMessage.errorsDict.Add(-991, "Внутренняя ошибка ПЦ. Состояние счета не было изменено, откат не требуется.");
    }

    public static string GetErrorMessage(int errorCode, string errorMessage) => ErrorMessage.errorsDict.ContainsKey(errorCode) ? string.Format("Код ошибки:{0}\r\n{1}", (object) errorCode, (object) ErrorMessage.errorsDict[errorCode]) : string.Format("Код ошибки: {0}\r\n{1}", (object) errorCode, (object) errorMessage);
  }
}
