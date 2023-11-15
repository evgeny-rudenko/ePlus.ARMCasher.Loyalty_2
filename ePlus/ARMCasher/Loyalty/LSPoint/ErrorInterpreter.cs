// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.LSPoint.ErrorInterpreter
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.LSPoint
{
  internal class ErrorInterpreter
  {
    public const short RET_CODE_OK = 0;
    public const short RET_CODE_NO_TERM_CONNECTION = 1;
    public const short RET_CODE_COMMUNICATION_ERROR = 2;
    public const short RET_CODE_SERVER_SEND_DECLINE = 3;
    public const short RET_CODE_TECHNICAL_PROBLEM = 4;
    public const short RET_CODE_FILE_CREATE_FAILED = 5;
    public const short RET_CODE_PROTO_SEQ_ERROR = 6;
    public const short RET_CODE_NOT_IMPLEMENTED = 7;
    public const short RET_CODE_FC_PROBLEM = 8;
    public const short RET_CODE_UNKNOWN_COMMAND = 9;
    public const short RET_CODE_ILLEGAL_PARAMETERS_SET = 10;
    public const short RET_CODE_PAY_TYPE_NOT_ALLOWED = 11;
    public const short RET_CODE_OPERATION_CANCELED = 12;
    public const short RET_CODE_TERMINAL_TECH_PROBLEM = 13;
    public const short RET_CODE_TERMINAL_BLOCKED = 14;
    public const short RET_CODE_NOT_FOUND = 15;
    public const short RET_CODE_REJECTED_CANCELED = 16;
    public const short RET_CODE_TYPES_MISMATCH = 17;
    public const short RET_CODE_NOT_SUPPORTED = 18;
    public const short RET_CODE_ANSWER_TYPE_ERROR = 19;
    public const short RET_CODE_ILLEGAL_ANSWER_TYPE = 20;
    public const short RET_CODE_CONFIG_PARAMS_INVALID = 21;
    public const short RET_CODE_CONFIG_PARAMS_ABSENT = 22;
    public const short RET_CODE_EXTRA_MESSAGES_ABSENT = 24;
    public const short RET_CODE_EM_PROBLEM = 25;
    public const short RET_CODE_PAYMENTS_PROBLEM = 26;
    public const short RET_CODE_CONNECT_BREAK_OFF = 27;
    public const short RET_CODE_SERVER_COMMUNICATION_ERROR = 28;
    public const short RET_CODE_MESSAGE_CERTIFICATE_ERROR = 29;
    public const short RET_CODE_IN_MESSAGE_CONTAIN_CERTIFICATE = 30;
    public const short RET_CODE_IN_CANCEL_BY_USER = 31;

    public static void OutputErrorInfo(ErrorInterpreter.ReturnCode code, string errorMessage)
    {
      switch (code)
      {
        case ErrorInterpreter.ReturnCode.NoTermConnection:
          int num1 = (int) MessageBox.Show("Нет соединения с терминалом", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        case ErrorInterpreter.ReturnCode.CommunicationError:
          int num2 = (int) MessageBox.Show("Ошибка связи." + Environment.NewLine + "Перезапустите кассовую систему!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          break;
        case ErrorInterpreter.ReturnCode.ServerSendDecline:
          int num3 = (int) MessageBox.Show("Сервер отклонил запрос", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        case ErrorInterpreter.ReturnCode.TechnicalProblem:
          int num4 = (int) MessageBox.Show("Технические проблемы." + Environment.NewLine + "Перезапустите кассовую систему!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          break;
        case ErrorInterpreter.ReturnCode.FileCreateFailed:
          int num5 = (int) MessageBox.Show("Ошибки работы с файловой системой." + Environment.NewLine + "Перезапустите кассовую систему!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          break;
        case ErrorInterpreter.ReturnCode.ProtoSeqError:
          int num6 = (int) MessageBox.Show("Ошибка последовательности обслуживания." + Environment.NewLine + "Перезапустите кассовую систему!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          break;
        case ErrorInterpreter.ReturnCode.NotImplemented:
          int num7 = (int) MessageBox.Show("Данная функция не доступна при выбранной схеме взаимодействия!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        case ErrorInterpreter.ReturnCode.FcProblem:
          int num8 = (int) MessageBox.Show("Проблема с запаковкой фискального чека. Возможно, отсутствуют товары или файл с чеком!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          break;
        case ErrorInterpreter.ReturnCode.UnknownCommand:
          int num9 = (int) MessageBox.Show("Неизвестный тип команды!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          break;
        case ErrorInterpreter.ReturnCode.IllegalParametersSet:
          int num10 = (int) MessageBox.Show("Неверный набор параметров!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          break;
        case ErrorInterpreter.ReturnCode.PayTypeNotAllowed:
          int num11 = (int) MessageBox.Show("Выбранный тип оплаты запрещён!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          break;
        case ErrorInterpreter.ReturnCode.OperationCanceled:
          int num12 = (int) MessageBox.Show("Операция отменена!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        case ErrorInterpreter.ReturnCode.TerminalTechProblem:
          int num13 = (int) MessageBox.Show("Технические проблемы в терминале!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        case ErrorInterpreter.ReturnCode.TerminalBlocked:
          int num14 = (int) MessageBox.Show("Терминал заблокирован!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        case ErrorInterpreter.ReturnCode.NotFound:
          int num15 = (int) MessageBox.Show("Операция отмены отклонена – оригинальная операция не найдена в журнале терминала!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        case ErrorInterpreter.ReturnCode.RejectedCanceled:
          int num16 = (int) MessageBox.Show("Повтор/Rollback отклонён – оригинальная операция была отменена!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        case ErrorInterpreter.ReturnCode.TypesMismatch:
          int num17 = (int) MessageBox.Show("Повтор операции отклонён – тип найденной операции не соответствует типу в запросе от ККМ!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        case ErrorInterpreter.ReturnCode.NotSupported:
          int num18 = (int) MessageBox.Show("Повтор/Rollback данной операции не поддерживается!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        case ErrorInterpreter.ReturnCode.AnswerTypeError:
          int num19 = (int) MessageBox.Show("На POS произошла внештатная ситуация.", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        case ErrorInterpreter.ReturnCode.IllegalAnswerType:
          int num20 = (int) MessageBox.Show("Недопустимый тип ответа!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        case ErrorInterpreter.ReturnCode.ConfigParamsInvalid:
          int num21 = (int) MessageBox.Show("Некорректные конфигурационные параметры!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        case ErrorInterpreter.ReturnCode.ConfigParamsAbsent:
          int num22 = (int) MessageBox.Show("Отсутствуют конфигурационные параметры!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        case ErrorInterpreter.ReturnCode.ExtraMessagesAbsent:
          int num23 = (int) MessageBox.Show("Отсутствуют экстра сообщения!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        case ErrorInterpreter.ReturnCode.EmProblem:
          int num24 = (int) MessageBox.Show("Ошибка работы с экстра сообщением!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        case ErrorInterpreter.ReturnCode.PaymentsProblem:
          int num25 = (int) MessageBox.Show("Ошибка работы с тегом Payments!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        case ErrorInterpreter.ReturnCode.ConnectBreakOff:
          int num26 = (int) MessageBox.Show("Клиент разорвал установленное соединение!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        case ErrorInterpreter.ReturnCode.ServerCommunicationError:
          int num27 = (int) MessageBox.Show("Ошибка связи с сервером обслуживания!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        default:
          int num28 = (int) MessageBox.Show("Ошибка обслуживания: " + (object) (int) code + ".\r\nПерезапустите кассовую систему!", errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          break;
      }
    }

    public enum ReturnCode
    {
      Ok = 0,
      NoTermConnection = 1,
      CommunicationError = 2,
      ServerSendDecline = 3,
      TechnicalProblem = 4,
      FileCreateFailed = 5,
      ProtoSeqError = 6,
      NotImplemented = 7,
      FcProblem = 8,
      UnknownCommand = 9,
      IllegalParametersSet = 10, // 0x0000000A
      PayTypeNotAllowed = 11, // 0x0000000B
      OperationCanceled = 12, // 0x0000000C
      TerminalTechProblem = 13, // 0x0000000D
      TerminalBlocked = 14, // 0x0000000E
      NotFound = 15, // 0x0000000F
      RejectedCanceled = 16, // 0x00000010
      TypesMismatch = 17, // 0x00000011
      NotSupported = 18, // 0x00000012
      AnswerTypeError = 19, // 0x00000013
      IllegalAnswerType = 20, // 0x00000014
      ConfigParamsInvalid = 21, // 0x00000015
      ConfigParamsAbsent = 22, // 0x00000016
      ExtraMessagesAbsent = 24, // 0x00000018
      EmProblem = 25, // 0x00000019
      PaymentsProblem = 26, // 0x0000001A
      ConnectBreakOff = 27, // 0x0000001B
      ServerCommunicationError = 28, // 0x0000001C
    }
  }
}
