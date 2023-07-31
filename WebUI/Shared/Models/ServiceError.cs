using System.Text.Json.Serialization;

namespace Shared.Models;

/// <summary>
/// All errors contained in ServiceResult objects must return an error of this type
/// Error codes allow the caller to easily identify the received error and take action.
/// Error messages allow the caller to easily show error messages to the end user.
///
/// Taken from https://github.com/iayti/CleanArchitecture/blob/master/src/Common/CleanArchitecture.Application/Common/Models/ServiceError.cs
/// </summary>
[Serializable]
public class ServiceError
{
    [JsonConstructor]
    public ServiceError(string message, int code)
    {
        Message = message;
        Code = code;
    }

    public ServiceError()
    {
    }

    /// <summary>
    /// Human readable error message
    /// </summary>
    public string Message { get; } = string.Empty;

    /// <summary>
    /// Machine readable error code
    /// </summary>
    public int Code { get; }

    /// <summary>
    /// Default error for when we receive an exception
    /// </summary>
    public static ServiceError DefaultError => new("An unknown exception occured.", 999);

    /// <summary>
    /// Default validation error. Use this for invalid parameters in controller actions and service methods.
    /// </summary>
    public static ServiceError ModelStateError(string validationError)
    {
        return new ServiceError(validationError, 998);
    }

    /// <summary>
    /// Use this for unauthorized responses.
    /// </summary>
    public static ServiceError ForbiddenError => new("You are not authorized to call this action.", 998);

    /// <summary>
    /// Use this to send a custom error message
    /// </summary>
    public static ServiceError CustomMessage(string errorMessage)
    {
        return new ServiceError(errorMessage, 999);
    }

    public static ServiceError DatabaseSaveFailed => new ServiceError("Database save failed", 800);

    //public static ServiceError NotReissue =>
    //    new ServiceError("Not reissue, registering a new card", (int)CardReturnCode.NotReissue);

    public static ServiceError UserNotFound => new("Card with this id does not exist", 996);

    public static ServiceError UserFailedToCreate => new("Failed to create User.", 995);

    public static ServiceError Canceled => new("The request canceled successfully!", 994);

    public static ServiceError NotFound => new("The specified resource was not found.", 990);

    public static ServiceError ValidationFormat => new("Request object format is not true.", 901);

    public static ServiceError Validation => new("One or more validation errors occurred.", 900);

    public static ServiceError SearchAtLeastOneCharacter =>
        new("Search parameter must have at least one character!", 898);

    /// <summary>
    /// Default error for when we receive an exception
    /// </summary>
    public static ServiceError ServiceProviderNotFound =>
        new("Service Provider with this name does not exist.", 700);

    public static ServiceError ServiceProvider => new("Service Provider failed to return as expected.", 600);

    public static ServiceError DateTimeFormatError =>
        new("Date format is not true. Date format must be like yyyy-MM-dd (2019-07-19)", 500);

    #region Override Equals Operator

    /// <summary>
    /// Use this to compare if two errors are equal
    /// Ref: https://msdn.microsoft.com/ru-ru/library/ms173147(v=vs.80).aspx
    /// </summary>
    public override bool Equals(object? obj)
    {
        // If parameter cannot be cast to ServiceError or is null return false.
        var error = obj as ServiceError;

        // Return true if the error codes match. False if the object we're comparing to is null
        // or if it has a different code.
        return Code == error?.Code;
    }

    public bool Equals(ServiceError error)
    {
        // Return true if the error codes match. False if the object we're comparing to is null
        // or if it has a different code.
        return Code == error?.Code;
    }

    public override int GetHashCode()
    {
        return Code;
    }

    public static bool operator ==(ServiceError? a, ServiceError? b)
    {
        // If both are null, or both are same instance, return true.
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        // If one is null, but not both, return false.
        if (a is null || b is null)
        {
            return false;
        }

        // Return true if the fields match:
        return a.Equals(b);
    }

    public static bool operator !=(ServiceError a, ServiceError b)
    {
        return !(a == b);
    }

    #endregion
}