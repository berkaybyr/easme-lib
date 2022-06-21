﻿namespace EasMe
{
    /// <summary>
    /// Error helper for EasLog also contains enum ErrorList
    /// </summary>
    public static class EasError
    {

        public static string? EnumGetKeybyValue(int value)
        {
            return Enum.GetName(typeof(Error), value);
        }
        /// <summary>
        /// Convert an error code to a readable string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string? ConvertEnumStringToReadable(string value)
        {
            var result = value.Replace("_", " ").ToLower();
            var firstChar = char.ToUpper(result[0]);
            return firstChar + result.Substring(1) + ".";
        }
        /// <summary>
        /// Convert an error code to a readable string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string? ConvertEnumStringToReadable(Error value)
        {
            var toStr = value.ToString();
            return ConvertEnumStringToReadable(toStr);
        }

    }
    public enum LogType
    {
        DEBUG = -2,
        ERROR = -1,
        BASE = 0,
        WEB = 1,
        CLIENT = 2,

    }
    public enum Severity
    {
        SUCCESS = 100,
        INFO = 101,
        WARN = 102,
        ERROR = 103,
        DEBUG = 104,
        EXCEPTION_OCCURED = 107,
    }
    public enum Error
    {
        SUCCESS = 100,
        INFO = 101,
        WARN = 102,
        ERROR = 103,
        DEBUG = 104,
        INVALID_VALUE = 105,
        INVALID_MODEL = 106,
        EXCEPTION_OCCURED = 107,
        NULL_REFERENCE = 108,
        TIMEOUT = 109,

        SQL_ERROR = 200,
        SQL_UPDATE_FAILED = 201,
        SQL_INSERT_FAILED = 202,
        SQL_DELETE_FAILED = 203,
        SQL_TABLE_CREATE_FAILED = 204,
        SQL_TABLE_DELETE_FAILED = 205,
        SQL_TABLE_TRUNCATE_FAILED = 206,
        SQL_FAILED_GET_TABLE = 207,
        SQL_FAILED_EXEC_NONQUERY = 208,
        SQL_FAILED_EXEC_SCALAR = 209,
        SQL_FAILED_BACKUP_DATABASE = 210,
        SQL_FAILED_SHRINK_DATABASE = 211,
        SQL_FAILED_DROP_DATABASE = 212,
        SQL_FAILED_DROP_TABLE = 213,
        SQL_FAILED_RESTORE_DATABASE = 214,
        SQL_FAILED_CREATE_DATABASE = 215,
        SQL_FAILED_GET_ALL_TABLE_NAME = 216,
        SQL_CONNECTION_NOT_INITIALIZED = 217,
        SQL_NO_PERMISSION = 218,
        SQL_NO_ROWS_FOUND = 219,
        SQL_NO_ROWS_AFFECTED = 220,
        SQL_NO_TABLE_FOUND = 221,

        LOGGING_ERROR = 300,
        SERIALIZATION_ERROR = 301,
        DESERIALIZATION_ERROR = 302,
        FILE_NOT_EXIST = 310,
        FOLDER_NOT_EXIST = 311,
        FILE_ALREADY_EXIST = 312,
        FOLDER_ALREADY_EXIST = 313,
        FILE_IS_BEING_USED = 314,
        FAILED_TO_CREATE_FILE = 315,
        FAILED_TO_CREATE_FOLDER = 316,
        FAILED_TO_CREATE_LOG = 317,
        FAILED_TO_DELETE_FILE = 318,
        FAILED_TO_DELETE_FOLDER = 319,
        FAILED_TO_READ_FILE = 320,
        FAILED_TO_WRITE_FILE = 321,
        FAILED_TO_WRITE_LOG = 322,
        LOG_FILE_CONTENT_NULL = 323,
        NO_LOGS_FOUND = 324,

        NOT_EXISTS = 400,
        NOT_FOUND = 405,
        NOT_ALLOWED = 406,
        NOT_SUPPORTED = 407,
        NOT_AVAILABLE = 408,
        NOT_ENABLED = 409,
        NOT_DISABLED = 410,
        NOT_CONNECTED = 411,
        NOT_LOGGED_IN = 412,
        NOT_LOGGED_OUT = 413,
        NOT_AUTHORIZED = 414,
        NOT_AUTHENTICATED = 415,
        NOT_READY = 416,
        NOT_INITIALIZED = 417,
        ALREADY_EXISTS = 418,
        ALREADY_USED = 419,
        ALREADY_IN_USE = 420,
        EXPIRED = 421,
        NOT_EXPIRED = 422,

        AUTHENTICATION_FAILED = 500,
        AUTHORIZATION_FAILED = 501,
        TOKEN_INVALID = 502,
        PASSWORD_INCORRECT = 503,
        USERNAME_INCORRECT = 504,
        EMAIL_INCORRECT = 505,
        EMAIL_ALREADY_SENT = 506,
        EMAIL_SENT_FAILED = 507,
        NO_ONLINE_NETWORK = 508,
        FAILED_TO_CONNECT = 509,
        FAILED_TO_GET_RESPONSE = 510,
        FAILED_TO_READ_RESPONSE = 511,
        FAILED_TO_WRITE_RESPONSE = 512,
        FAILED_TO_SEND_RESPONSE = 513,

        FAILED_TO_CONVERT_EXCEPTION_TO_LOG_MODEL = 700,
        FAILED_TO_CREATE_BASE_MODEL = 701,
        FAILED_TO_CREATE_WEB_MODEL = 702,
        FAILED_TO_LOAD_CONFIGURATION = 703,
        NOT_FOUND_LOADED_CONFIGURATION_ERROR = 704,


    }
}
