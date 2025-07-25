﻿public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public string? ErrorMessage { get; private set; }
    public int? StatusCode { get; private set; }
    public T? Data { get; private set; }

    public static Result<T> Success(T data) =>
        new Result<T> { IsSuccess = true, Data = data };

    public static Result<T> Failure(string message, int statusCode = 400) =>
        new Result<T> { IsSuccess = false, ErrorMessage = message, StatusCode = statusCode };
}

public class Result
{
    public bool IsSuccess { get; protected set; }
    public string? ErrorMessage { get; protected set; }
    public int StatusCode { get; protected set; }

    public static Result Success() => new Result { IsSuccess = true, StatusCode = 200 };

    public static Result Failure(string message, int statusCode = 400) =>
        new Result { IsSuccess = false, ErrorMessage = message, StatusCode = statusCode };
}
